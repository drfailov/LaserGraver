using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LaserDrawerApp
{
    public partial class MainWindow : Form
    {
        private bool debug = false;
        private SerialCommunicator serialCommunicator = new SerialCommunicator();
        public String logText = "";
        public Label logLabel = null;
        private Thread engravingThread = null;
        private CenterScalablePanel centerScalablePanel = null;
        public Point resolution = new Point(-1, -1); //текущий размер проекта (полный)
        public Bitmap renderedImage = null;
        public Thread renderThread = null;
        public Thread previewThread = null;
        private int previewSpeed = 1; //1x, 2x, 5x, 10x, 20x, 50x, 100x...
        //структуры для кластеризации
        public Bitmap clusteringPreviewImage = null;
        public Bitmap clusteringMaskImage = null;
        //кластер содержит зону рисунка и маску. то что чёрным цветом относится к этому кластеру
        public List<Tuple<Rectangle, Bitmap>> clusters = new List<Tuple<Rectangle, Bitmap>>();
        public List<BurnMark> burnMarks = new List<BurnMark>();
        private int engravingNumberOfTimes = 0; //Количество проходов прожига

        public MainWindow()
        {
            InitializeComponent();
            serialCommunicator.setStatusDelegate(new SerialCommunicator.LogDelegate(status));
            serialCommunicator.setMessageBoxDelegate(new SerialCommunicator.LogDelegate(messageBox));
            serialCommunicator.setOnConnected(new SerialCommunicator.ActionDelegate(onConnected));
            serialCommunicator.setOnDisconnected(new SerialCommunicator.ActionDelegate(onDisconnected));
            serialCommunicator.setOnPositionUpdated(new SerialCommunicator.PositionDelegate(onPositionUpdate));
            serialCommunicator.setOnProgressUpdated(new SerialCommunicator.ProgressDelegate(updateEngravingProgress));
            
        }
        private String status(String text)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { status(text); });
                return text;
            }
            log(text);
            statusLabel.Text = text;
            Application.DoEvents();
            return text;
        }
        private void progress(int percent)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { progress(percent); });
                return;
            }
            toolStripProgressBar1.ProgressBar.Value = percent;
            Application.DoEvents();
        }

        private void buttonLaserTestHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Чем больше время прожига, тем контрастнее будет картинка в результате, и для каждого материала это значение может отличаться очень сильно." +
                "\nЧтобы узнать какое время прожига точки подходит для определённого материала, найдите на материале небольшой фрагмент на котором можно провести испытание нарисовав несколько линий. "+
                            "\n" +
                            "\nНажатие на кнопку \"Прожечь линию\" запустит рисование линии с заданными настройками на несколько сантиметров вправо от текущего положения лазера."+
                            "\n" +
                            "\n- Если линия незаметна, можно попробовать время побольше." +
                            "\n- Если линия прожигает материал, используйте время меньше.", "Справка", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void toolStripButtonRefreshComList_Click(object sender, EventArgs e)
        {
            toolStripComboBoxComList.Items.Clear();
            List<string> COMs = new List<string>();
            status("Получение портов...");
            progress(0);
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Caption like '%(COM%'"))
            {
                var portnames = SerialPort.GetPortNames();
                var ports = searcher.Get().Cast<ManagementBaseObject>().ToList().Select(p => p["Caption"].ToString());

                var portList = portnames.Select(n => n + " - " + ports.FirstOrDefault(s => s.Contains(n))).ToList();

                foreach (string s in portList)
                {
                    Console.WriteLine(s);
                    COMs.Add(s);
                }
            }

            int sel = 0;
            for (int i = 0; i < COMs.Count; i++)
            {
                String com = COMs[i];
                if (com.Contains("CH340"))
                    sel = i;
                toolStripComboBoxComList.Items.Add(com);
            }
            if (toolStripComboBoxComList.Items.Count > 0)
                toolStripComboBoxComList.SelectedIndex = sel;
            toolStripButtonConnect.Enabled = true;
            status("Готово.");
            progress(0);
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            //version
            {
                FileInfo f = new FileInfo(Application.ExecutablePath);
                Text = Text + " (Версия от " + f.LastWriteTime.ToShortDateString() + ")";
            }
        }

        private void toolStripButtonNewProject_Click(object sender, EventArgs e)
        {
            if (serialCommunicator.isConnected())
            {
                Size size = serialCommunicator.size();
                if(size.Width != 0 && size.Height != 0)
                {
                    new NewProjectWindow(this, size.Width, size.Height).ShowDialog();
                    return;
                }
            }
            new NewProjectWindow(this, -1, -1).ShowDialog();

        }

        public void newProject(int width, int height)
        {
            status("Инициализация изображения...");
            float maxCSize = Math.Min(panelContainer.Width, panelContainer.Height);
            float maxISize = Math.Max(width, height);
            float coef = maxCSize / maxISize;

            centerScalablePanel = new CenterScalablePanel();
            centerScalablePanel.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.panelContainer_MouseDoubleClick);
            panelContainer.Controls.Clear();
            panelContainer.Controls.Add(centerScalablePanel);
            centerScalablePanel.Size = new Size((int)(width * coef), (int)(width * coef));
            resolution.X = width;
            resolution.Y = height;
            toolStripLabelSize.Text = "Размер проекта:  " + width + " пикс. x " + height + " пикс.";
            toolStripButtonRender.Enabled = true;
            toolStripButtonZoomFit.Enabled = true;
            toolStripButtonZoomIn.Enabled = true;
            toolStripButtonZoomOut.Enabled = true;
            toolStripButtonText.Enabled = true;
            toolStripButtonImage.Enabled = true;
            toolStripButtonPlaceholder.Enabled = true;
            toolStripTextBoxBurnTimeMs.Enabled = true;
            toolStripComboBoxEngraveTimes.Enabled = true;
            status("Готово.");
            fitScale();
        }

        private void pictureBoxMoveHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Используйте перемещение лазера для точного позиционирования изображения при подготовке к гравировке." +
                "\nПозиция лазера в реальном времени отображается на экране проекта.", "Справка", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void toolStripButtonImage_Click(object sender, EventArgs e)
        {

            if (centerScalablePanel != null)
            {

                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "Изображения(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    Bitmap file = new Bitmap(open.FileName);

                    ImageFloatingControl sizeable = new ImageFloatingControl(file, this);
                    sizeable.Location = new Point(10, 10);
                    sizeable.Size = new System.Drawing.Size(100, 80);
                    centerScalablePanel.Controls.Add(sizeable);
                    sizeable.BringToFront();
                }
            }
        }

        private void toolStripButtonText_Click(object sender, EventArgs e)
        {
            if (centerScalablePanel != null)
            {
                TextInputWindow open = new TextInputWindow();
                open.ShowDialog();
                if (open.DialogResult == DialogResult.OK)
                {
                    String text = open.text;

                    TextFloatingControl sizeable = new TextFloatingControl(text);
                    sizeable.Location = new Point(10, 10);
                    sizeable.Size = new System.Drawing.Size(100, 80);
                    centerScalablePanel.Controls.Add(sizeable);
                    sizeable.BringToFront();
                }
            }
        }

        void setScale(float coef)
        {
            if (centerScalablePanel == null)
                return;
            if (coef > 0)
                centerScalablePanel.Size = new Size((int)((float)resolution.X * coef), (int)((float)resolution.Y * coef));
            toolStripLabelScale.Text = "Машстаб: " + Math.Round(coef * 100) + "%";
        }
        float getScale()
        {
            return (float)centerScalablePanel.Size.Width / (float)resolution.X;
        }
        float fitScale()
        {
            float coef = Math.Min((float)panelContainer.Size.Width / (float)resolution.X, (float)panelContainer.Size.Height / (float)resolution.Y);
            return coef;
        }
        private void MainWindow_Resize(object sender, EventArgs e)
        {
            if (centerScalablePanel != null)
            {
                float fit = fitScale();
                if (getScale() < fit * 2)
                    setScale(fit);
            }
        }

        private void sleep(int ms)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (stopwatch.ElapsedMilliseconds < ms)
                Application.DoEvents();
        }

        private void generateInitialImage()
        {
            status("Разрешение: " + resolution.X + "x" + resolution.Y);
            renderedImage = centerScalablePanel.generateImage(resolution.X, resolution.Y);
        }
        private void showRendered(Bitmap bitmap)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { showRendered(bitmap); });
                Thread.Sleep(100);
                return;
            }

            try
            {
                int coef = Math.Min(bitmap.Width / pictureBoxRendered.Width, bitmap.Height / pictureBoxRendered.Height);
                if (coef < 1) coef = 1;
                Size newSize = new Size(bitmap.Width / coef, bitmap.Height / coef);
                pictureBoxRendered.Image = new Bitmap(bitmap, newSize);
                //pictureBoxRendered.Location = new Point(0, 0);
                //pictureBoxRendered.Size = bitmap.Size;
                tabControl1.SelectedTab = tabPageRendered;
            }
            catch(Exception e)
            {
                log(e.Message);
            }
            Application.DoEvents();
        }
        private void updateRendered(Bitmap bitmap)
        {
            pictureBoxRendered.Image = bitmap;
            Application.DoEvents();
        }

        private void generateGrayscaleImage()
        {
            status("Генерирование обесцвеченного изображения...");
            for (int y = 0; y < renderedImage.Height; y++)
            {
                for (int x = 0; x < renderedImage.Width; x++)
                {
                    Color color = renderedImage.GetPixel(x, y);
                    int intensity = (int)((0.21f * color.R) + (0.72f * color.G) + (0.07f * color.B)); //0...255
                    renderedImage.SetPixel(x, y, Color.FromArgb(255, intensity, intensity, intensity));
                }
                if (y % 50 == 0)
                {
                    updateRendered(renderedImage);
                    float total = renderedImage.Height;
                    float complete = y;
                    int percent = (int)(100f * complete / total);
                    status("Генерирование обесцвеченного изображения  (" + percent + "%)...");
                }
            }
        }

        private Bitmap generateGrayscaleImageQuick(Bitmap bitmap)
        {
            status("Генерирование обесцвеченного изображения...");

            LockBitmap lockBitmap = new LockBitmap(bitmap);
            lockBitmap.LockBits();
            
            for (int y = 0; y < lockBitmap.Height; y++)
            {
                for (int x = 0; x < lockBitmap.Width; x++)
                {
                    Color color = lockBitmap.GetPixel(x, y);
                    int intensity = (int)((0.21f * color.R) + (0.72f * color.G) + (0.07f * color.B)); //0...255
                    lockBitmap.SetPixel(x, y, Color.FromArgb(255, intensity, intensity, intensity));
                }
                if (y % 10 == 0)
                {
                    showHorizontalLineMark(y);
                    float total = lockBitmap.Height;
                    float complete = y;
                    int percent = (int)(100f * complete / total);
                    status("Генерирование обесцвеченного изображения  (" + percent + "%)...");
                }
            }
            lockBitmap.UnlockBits();
            hideHorizontalLineMark();
            return bitmap;
        }

        private Bitmap generateBWImageQuick(Bitmap bitmap)
        {
            status("Генерирование ЧБ изображения, поиск минимума и максимума...");

            LockBitmap lockBitmap = new LockBitmap(renderedImage);
            lockBitmap.LockBits();

            int min = 255;
            int max = 0;
            for (int y = 0; y < lockBitmap.Height; y++)
            {
                for (int x = 0; x < lockBitmap.Width; x++)
                {
                    Color color = lockBitmap.GetPixel(x, y);
                    if (color.R < min) min = color.R;
                    if (color.R > max) max = color.R;

                }
                if (y % 50 == 0)
                {
                    showHorizontalLineMark(y);
                    float total = lockBitmap.Height;
                    float complete = y;
                    int percent = (int)(100f * complete / total);
                    status("Генерирование ЧБ изображения, поиск минимума и максимума  (" + percent + "%)...");
                }
            }
            status("Генерирование ЧБ изображения, бинаризация цветов...");
            long avg = (min + max) / 2;
            for (int y = 0; y < lockBitmap.Height; y++)
            {
                for (int x = 0; x < lockBitmap.Width; x++)
                {
                    Color color = lockBitmap.GetPixel(x, y);
                    int intensity = (color.R + color.G + color.B) / 3; //0...255
                    if (intensity > avg)
                        lockBitmap.SetPixel(x, y, Color.FromArgb(255, 255, 255, 255));
                    else
                        lockBitmap.SetPixel(x, y, Color.FromArgb(255, 0, 0, 0));
                }
                if (y % 50 == 0)
                {
                    showHorizontalLineMark(y);
                    float total = lockBitmap.Height;
                    float complete = y;
                    int percent = (int)(100f * complete / total);
                    status("Генерирование ЧБ изображения, бинаризация цветов  (" + percent + "%)...");
                }
            }
            hideHorizontalLineMark();
            lockBitmap.UnlockBits();
            return bitmap;
        }
        private Rectangle fillFromQuick(Bitmap bitmap, Bitmap copy, Point startingPoint, Color color)
        {
            int minX = startingPoint.X;
            int maxX = startingPoint.X;
            int minY = startingPoint.Y;
            int maxY = startingPoint.Y;
            Rectangle imageRect = new Rectangle(Point.Empty, bitmap.Size);
            QueuePoints points = new QueuePoints(Math.Max(bitmap.Width, bitmap.Height) * 2);
            LockBitmap lockBitmap = new LockBitmap(bitmap);
            lockBitmap.LockBits();
            LockBitmap lockCopyBitmap = null;
            if (copy != null && copy.Width >= bitmap.Width && copy.Height >= bitmap.Height)
            {
                lockCopyBitmap = new LockBitmap(copy);
                lockCopyBitmap.LockBits();
            }
            Color initialColor = lockBitmap.GetPixel(startingPoint.X, startingPoint.Y);
            points.Enqueue(startingPoint.X, startingPoint.Y);
            long stepsCounter = 0;
            while (points.Count() > 0)
            {
                stepsCounter++;
                int X = points.DequeueX();
                int Y = points.DequeueY();
                lockBitmap.SetPixel(X, Y, color);
                if(lockCopyBitmap != null)
                    lockCopyBitmap.SetPixel(X, Y, color);
                if (X < minX) minX = X;
                if (Y < minY) minY = Y;
                if (X > maxX) maxX = X;
                if (Y > maxY) maxY = Y;

                if (imageRect.Contains(X + 1, Y) && lockBitmap.GetPixel(X + 1, Y).Equals(initialColor) && !points.Contains(X + 1, Y))
                    points.Enqueue(X + 1, Y);

                if (imageRect.Contains(X - 1, Y) && lockBitmap.GetPixel(X - 1, Y).Equals(initialColor) && !points.Contains(X - 1, Y))
                    points.Enqueue(X - 1, Y);

                if (imageRect.Contains(X, Y + 1) && lockBitmap.GetPixel(X, Y + 1).Equals(initialColor) && !points.Contains(X, Y + 1))
                    points.Enqueue(X, Y + 1);

                if (imageRect.Contains(X, Y - 1) && lockBitmap.GetPixel(X, Y - 1).Equals(initialColor) && !points.Contains(X, Y - 1))
                    points.Enqueue(X, Y - 1);

                if (stepsCounter % 10000 == 0)
                {
                    showPointMark(X, Y);
                    status("Заливка...  Пикселов в очереди: " + points.Count());
                }
            }
            lockBitmap.UnlockBits();
            if (lockCopyBitmap != null)
                lockCopyBitmap.UnlockBits();
            hidePointMark();
            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }
        private void generateClusters()
        {
            clusters.Clear();
            status("Кластеризация изображения, копирование изображения...");
            clusteringPreviewImage = new Bitmap(renderedImage);
            status("Кластеризация изображения, покраска маски...");
            clusteringMaskImage = new Bitmap(renderedImage.Width, renderedImage.Height);
            using (Graphics gfx = Graphics.FromImage(clusteringMaskImage))
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255, 255)))
                gfx.FillRectangle(brush, 0, 0, renderedImage.Width, renderedImage.Height);
            showRendered(clusteringPreviewImage);

            status("Кластеризация изображения, создание рамки...");
            for (int y = 0; y < clusteringPreviewImage.Height; y++)
            {
                clusteringPreviewImage.SetPixel(0, y, Color.White);
                clusteringPreviewImage.SetPixel(1, y, Color.White);
                clusteringPreviewImage.SetPixel(clusteringPreviewImage.Width - 2, y, Color.White);
                clusteringPreviewImage.SetPixel(clusteringPreviewImage.Width - 1, y, Color.White);
            }
            for (int x = 0; x < clusteringPreviewImage.Width; x++)
            {
                clusteringPreviewImage.SetPixel(x, 0, Color.White);
                clusteringPreviewImage.SetPixel(x, 1, Color.White);
                clusteringPreviewImage.SetPixel(x, clusteringPreviewImage.Height - 2, Color.White);
                clusteringPreviewImage.SetPixel(x, clusteringPreviewImage.Height - 1, Color.White);
            }
            showRendered(clusteringPreviewImage);

            status("Кластеризация изображения, заливка фона...");
            fillFromQuick(clusteringPreviewImage, clusteringMaskImage, Point.Empty, Color.Black);
            showRendered(clusteringMaskImage);

            status("Кластеризация изображения, выделение кластеров...");
            clusteringPreviewImage = new Bitmap(renderedImage);
            for (int y = 0; y < clusteringMaskImage.Height; y++)
            {
                for (int x = 0; x < clusteringMaskImage.Width; x++)
                {
                    Color color = clusteringMaskImage.GetPixel(x, y);
                    if (color.ToArgb().Equals(Color.White.ToArgb()))
                    {
                        using (Graphics g = Graphics.FromImage(clusteringPreviewImage))
                            g.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, clusteringPreviewImage.Width, clusteringPreviewImage.Height));
                        Rectangle rect = fillFromQuick(clusteringMaskImage, clusteringPreviewImage, new Point(x, y), Color.Black);
                        showRendered(clusteringMaskImage);
                        if(rect.Width > 5 && rect.Height > 5)
                        {
                            Bitmap mask = clusteringPreviewImage.Clone(rect, clusteringMaskImage.PixelFormat);
                            Tuple<Rectangle, Bitmap> cluster = new Tuple<Rectangle, Bitmap>(rect, mask);
                            clusters.Add(cluster);
                        }
                    }
                }
                if (y % 50 == 0)
                {
                    showHorizontalLineMark(y);
                    float total = clusteringMaskImage.Height;
                    float complete = y;
                    int percent = (int)(100f * complete / total);
                    status("Кластеризация изображения, выделение кластеров (" + percent + "%, выделено "+clusters.Count+" шт)...");
                }
            }
            //в этом месте у нас есть массив кластеров, в котором есть квадраты с зонами и маски участка рисунка
            //нарисовать реконструкцию
            using (Graphics g = Graphics.FromImage(clusteringPreviewImage)) {
                g.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, clusteringPreviewImage.Width, clusteringPreviewImage.Height));
                for (int i = 0; i < clusters.Count; i++)
                {
                    Tuple<Rectangle, Bitmap> cluster = clusters[i];
                    g.DrawImage(cluster.Item2, cluster.Item1);
                }
                for (int i = 0; i < clusters.Count; i++)
                {
                    Tuple<Rectangle, Bitmap> cluster = clusters[i];
                    g.DrawRectangle(new Pen(Color.Blue, 3), cluster.Item1);
                }
            }

            showRendered(clusteringPreviewImage);
            hidePointMark();
        }
        private List<BurnMark> multiplyPaths(List<BurnMark> input, int times)
        {
            List<BurnMark> burnMarks = new List<BurnMark>();
            for (int i=0; i < times; i++)
            {
                for(int j=0; j<input.Count; j++)
                    burnMarks.Add(input[j]);
            }
            return burnMarks;
        }
        private List<BurnMark> generatePaths(Bitmap bitmap, int burnTime)
        {
            List<BurnMark> burnMarks = new List<BurnMark>();
            for (int y = 0; y < bitmap.Height; y++)
            {
                int lineStart = -1;
                int lineBurnTime = -1;
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color imagePixel = bitmap.GetPixel(x, y);
                    int pixelLuminocity = (imagePixel.R + imagePixel.G + imagePixel.B) / 3;
                    pixelLuminocity = (pixelLuminocity / 20) * 20;//добавить немного дискретности
                    int pixelValue = 255 - pixelLuminocity;    //0...255
                    if (pixelValue < 20) pixelValue = 0;
                    int pixelBurnTime = (burnTime * pixelValue) / 255;
                    //если там нихуя нет, а линия начата и она ненулевая, значить закончить линию 
                    if (pixelBurnTime < 1 && lineStart != -1 && lineBurnTime > 0)
                    {
                        int lineEnd = x;
                        burnMarks.Add(new BurnMark(lineBurnTime, lineStart, lineEnd, y, y));
                        lineStart = -1;
                        lineBurnTime = -1;
                    }
                    //если там что-то есть, а до этого нихуя не было, значит начать линию
                    else if (pixelBurnTime > 0 && lineStart == -1)
                    {
                        lineStart = x;
                        lineBurnTime = pixelBurnTime;
                    }
                    //если там что-то есть, и до этого что-то было, и оно отличается, значить закончить старую линию и начать новую
                    else if (pixelBurnTime > 0 && lineStart != -1 && lineBurnTime > 0 && pixelBurnTime != lineBurnTime)
                    {
                        int lineEnd = x;
                        burnMarks.Add(new BurnMark(lineBurnTime, lineStart, lineEnd, y, y));
                        lineStart = x;
                        lineBurnTime = pixelBurnTime;
                    }
                    //если там что-то есть, и до этого что-то было, и оно совпадает по интенсивности, значит нихуя не делаем, шагаем дальше
                    else
                    {
                    }
                }
                //если строка закончилась, а линия начата и она нелулевая, закончить линию
                if (lineStart != -1 && lineBurnTime > 0)
                {
                    int lineEnd = bitmap.Width - 1;
                    burnMarks.Add(new BurnMark(lineBurnTime, lineStart, lineEnd, y, y));
                }
                if (y % 14 == 0)
                {
                    int percent = 100 * y / bitmap.Height;
                    status("Рендеринг инструкций ("+percent+"%)...");
                    showHorizontalLineMark(y);
                }
            }
            return burnMarks;
        }
        private List<BurnMark> movePaths(List<BurnMark> burnMarks, int plusX, int plusY)
        {
            for (int i = 0; i < burnMarks.Count; i++)
            {
                burnMarks[i].Xfrom += plusX;
                burnMarks[i].Xto   += plusX;
                burnMarks[i].Yfrom += plusY;
                burnMarks[i].Yto += plusY;
            }
            return burnMarks;
        }
        private void drawPreview(Bitmap bitmap, List<BurnMark> burnMarks)
        {
            //нарисовать реконструкцию маршрутов
            int pathOpacity = 30;
            int burnOpacity = 100;
            float maxTime = 0;
            for (int i = 0; i < burnMarks.Count; i++)
                if (burnMarks[i].burnTimeMs > maxTime)
                    maxTime = burnMarks[i].burnTimeMs;
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                Pen burnPen = new Pen(Color.FromArgb(255, 255, 0, 0), bitmap.Width / 1000);
                Pen pathPen = new Pen(Color.FromArgb(pathOpacity, 0, 0, 255), bitmap.Width / 1000);
                g.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                int lastX = 0;
                int lastY = 0;
                for (int i = 0; i < burnMarks.Count; i++)
                {
                    if (i % 240 == 0 || debug)
                    {
                        status("Реконструкция инструкций, обработка инструкции " + i + " из " + burnMarks.Count + "...");
                        if(debug)
                            updateRendered(bitmap);
                    }
                    BurnMark burnMark = burnMarks[i];
                    float time = burnMarks[i].burnTimeMs;
                    float coef = time / maxTime;
                    float opacity = burnOpacity * coef;
                    burnPen.Color = Color.FromArgb((int)opacity, 255, 0, 0);
                    g.DrawLine(pathPen, lastX, lastY, burnMark.Xfrom, burnMark.Yfrom);
                    g.DrawLine(burnPen, burnMark.Xfrom, burnMark.Yfrom, burnMark.Xto, burnMark.Yto);
                    lastX = burnMark.Xto;
                    lastY = burnMark.Yto;
                }
            }
        }
        private void startRendering()
        {
            try
            {
                //обнулить текстовые поля
                labelEngraveTimeRemaining.Text = showTime(0);
                labelEngraveTime.Text = showTime(0);
                labelBurnMarskTotal.Text = "0";
                //проверка правильно ли введено время обжига
                try
                {
                    int result = Int32.Parse(toolStripTextBoxBurnTimeMs.Text);
                    if (result <= 0)
                    {
                        messageBox("Время обжига введено неверно. Чисто не должно быть меньше 1 мс.");
                        return;
                    }
                    if (result > 5000)
                    {
                        messageBox("Время обжига введено неверно. Чисто не должно быть больше 5000 мс (5 секунд).");
                        return;
                    }
                }
                catch (Exception xe)
                {
                    messageBox("Время обжига введено неверно. Поле должно содержать только цифры.");
                    return;
                }
                //проверка правильно ли введено количество циклов обжига
                try
                {
                    engravingNumberOfTimes = Int32.Parse(toolStripComboBoxEngraveTimes.Text);
                    if (engravingNumberOfTimes <= 0)
                    {
                        messageBox("Количество циклов прожига введено неверно. Чисто не должно быть меньше 1 раза.");
                        return;
                    }
                    if (engravingNumberOfTimes > 100)
                    {
                        messageBox("Количество циклов прожига введено неверно. Чисто не должно быть больше 100 раз.");
                        return;
                    }
                }
                catch (Exception xe)
                {
                    messageBox("Количество циклов прожига введено неверно. Поле должно содержать только цифры.");
                    return;
                }

                generateInitialImage();
                status("Изображение сгенерировано, размер изображения " + renderedImage.Width + "px X " + renderedImage.Height + "px.");
                progress(10);
                showRendered(renderedImage);

                renderThread = new Thread(render);
                renderThread.Start();
                onStartRendering();
            }
            catch (Exception) { }
        }
        private void stopRendering()
        {
            renderThread = null;
            onRendered();
        }
        private void render()
        {
            try
            {
                renderedImage = generateGrayscaleImageQuick(renderedImage);
                status("Изображение обесцвечено, размер изображения " + renderedImage.Width + "px X " + renderedImage.Height + "px.");
                progress(20);
                showRendered(renderedImage);
                if (Thread.CurrentThread != renderThread) return;

                if (!использоватьПолутонаToolStripMenuItem.Checked)
                {
                    renderedImage = generateBWImageQuick(renderedImage);
                    status("Изображение теперь монохромное, размер изображения " + renderedImage.Width + "px X " + renderedImage.Height + "px.");
                    progress(40);
                    showRendered(renderedImage);
                    if (Thread.CurrentThread != renderThread) return;
                }

                burnMarks = generatePaths(renderedImage, Int32.Parse(toolStripTextBoxBurnTimeMs.Text));
                status("План прожига сформирован, выделено " + burnMarks.Count + " инструкций.");
                progress(60);
                showRendered(renderedImage);
                if (Thread.CurrentThread != renderThread) return;

                if(гравировкаПоЗигзагуToolStripMenuItem.Checked)
                    burnMarks = zigzagWay(burnMarks, renderedImage.Height);
                if(гравировкаВСлучайномПорядкеToolStripMenuItem.Checked)
                    burnMarks = randomizeWay(burnMarks);
                if (гравировкаПоКратчайшемуМаршрутуToolStripMenuItem.Checked)
                {
                    burnMarks = divideWay(burnMarks);
                    burnMarks = randomizeWay(burnMarks);
                    burnMarks = optimizeWay(burnMarks);
                }
                //на этом месте "количество проходов"
                burnMarks = multiplyPaths(burnMarks, engravingNumberOfTimes);

                status("План прожига оптимизирован, выделено " + burnMarks.Count + " инструкций.");
                progress(90);
                showRendered(renderedImage);
                if (Thread.CurrentThread != renderThread) return;

                drawPreview(renderedImage, burnMarks);
                status("Реконструкция подготовлена, нарисовано " + burnMarks.Count + " инструкций.");
                showRendered(renderedImage);
                hidePointMark();
            }
            finally
            {
                onRendered();
                status("Готово.");
            }

        }

        private List<BurnMark> divideWay(List<BurnMark> old)
        {
            //разделить один длинный прожиг на несколько меньших, по 3 пикселя
            int countInitial = old.Count;
            int periodic = countInitial / 200; //используется для оптимизации вывода лога
            if (periodic == 0) periodic = 1;
            List<BurnMark> optimised = new List<BurnMark>();
            while (old.Count > 0)
            {
                if (debug)
                    log("Dividing... Remaining " + old.Count + " of " + countInitial);
                if (old.Count % periodic == 0)
                {
                    int percent = 100 * old.Count / countInitial;
                    percent = 100 - percent;
                    status("Нарезка маршрута (" + percent + "%) ...");
                }
                BurnMark minMark = old[0];
                old.Remove(minMark);
                //Если у нас на входе обратный штрих, сделать его прямым
                if (minMark.Xto - minMark.Xfrom < 0)
                    minMark = revert(minMark);
                //разделить один длинный прожиг на несколько меньших, по 3 пикселя
                //откусываем от метки по 3 пикселя каждую итерацию
                while (minMark.Xto - minMark.Xfrom > 3)
                {
                    BurnMark part = new BurnMark(minMark.burnTimeMs, minMark.Xfrom, minMark.Xfrom + 3, minMark.Yfrom, minMark.Yfrom);
                    minMark.Xfrom = minMark.Xfrom + 3;
                    optimised.Add(part);
                }
                optimised.Add(minMark);
            }
            return optimised;
        }

        private List<BurnMark> optimizeWay(List<BurnMark> old)
        {
            /*
             * Расположить команды так, чтобы граверу было меньше ходить
             * Этот сценарий может быть полезен при порезке, однако при этом важно чтобы команды были короткими и в одну сторону
             * поэтому, нужно сделать так, чтобы длинные команды разделялись на короткие
             * 
             */
            if (old.Count < 3)
                return old;
            int countInitial = old.Count;
            int periodic = countInitial / 200; //используется для оптимизации вывода лога
            if (periodic == 0) periodic = 1;
            List<BurnMark> optimised = new List<BurnMark>();
            BurnMark first = old[0];
            optimised.Add(first);
            double curX = first.Xto;
            double curY = first.Yto;
            old.Remove(first);
            while (old.Count > 0)
            {
                if(debug)
                    log("Optimizing... Remaining " + old.Count + " of " + countInitial);
                if(old.Count % periodic == 0)
                {
                    int percent = 100 * old.Count / countInitial;
                    percent = 100 - percent;
                    status("Оптимизация маршрута (" + percent + "%) ...");
                }
                //найти самый ближний элемент к curX curY
                BurnMark minMark = old[0];
                //bool minMarkdirect = true;
                double minMarkDistance = distanceDirect(curX, curY, minMark);
                for (int i = 0; i < old.Count; i++)
                {
                    BurnMark next = old[i];
                    double dd = distanceDirect(curX, curY, next);
                    //double dr = distanceRevert(curX, curY, next);
                    if (dd < minMarkDistance && (dd > 10 || next.Yfrom != curY))
                    {
                        //minMarkdirect = true;
                        minMark = next;
                        minMarkDistance = dd;
                    }
                    //if (dr < minMarkDistance)
                    //{
                    //    minMarkdirect = false;
                    //    minMark = next;
                    //    minMarkDistance = dr;
                    //}
                }
                //тут мы его уже нашли. удалить их старого массива и добавить в новый, обновить координаты
                old.Remove(minMark);
                //if (!minMarkdirect)
                //    minMark = revert(minMark);
                optimised.Add(minMark);
                curX = minMark.Xto;
                curY = minMark.Yto;
            }
            return optimised;
        }

        private List<BurnMark> zigzagWay(List<BurnMark> old, int height)
        {
            if (old.Count < 3)
                return old;
            int periodic = old.Count / 200;
            if (periodic == 0) periodic = 1;
            List<BurnMark> optimised = new List<BurnMark>();
            for (int y = 0; y < height; y++)
            {
                if (y % periodic == 0)
                {
                    int percent = 100 * y / old.Count;
                    status("Оптимизация маршрута (" + percent + "%) ...");
                }
                List<BurnMark> line = findLine(old, y);
                if (y % 2 == 0)
                    optimised.AddRange(line);
                else
                    optimised.AddRange(revertLine(line));
            }
            return optimised;
        }
        private List<BurnMark> findLine(List<BurnMark> marks, int y)
        {
            List<BurnMark> result = new List<BurnMark>();
            for (int i = 0; i < marks.Count; i++)
            {
                BurnMark minMark = marks[i];
                if (minMark.Yfrom == y)
                    result.Add(minMark);
            }
            return result;
        }
        private List<BurnMark> revertLine(List<BurnMark> marks)
        {
            List<BurnMark> result = new List<BurnMark>();
            for (int i = marks.Count-1; i >= 0; i--)
            {
                result.Add(revert(marks[i]));
            }
            return result;
        }

        private List<BurnMark> randomizeWay(List<BurnMark> old)
        {
            if (old.Count < 3)
                return old;
            Random random = new Random();
            int countInitial = old.Count;
            int periodic = countInitial / 200;
            if (periodic == 0) periodic = 1;
            List<BurnMark> optimised = new List<BurnMark>();

            while (old.Count > 0)
            {
                if (debug)
                    log("Randomizing... Remaining " + old.Count + " of " + countInitial); 
                if (old.Count % periodic == 0)
                {
                    int percent = 100 * old.Count / countInitial;
                    percent = 100 - percent;
                    status("Перемешивание маршрута (" + percent + "%) ...");
                }
                BurnMark minMark = old[random.Next(old.Count)];
                old.Remove(minMark);
                optimised.Add(minMark);
            }
            return optimised;
        }
        private double distanceDirect(double X, double Y, BurnMark burnMark)
        {
            double dx = Math.Abs(X - burnMark.Xfrom);
            double dy = Math.Abs(Y - burnMark.Yfrom);
            return Math.Sqrt(dx * dx + dy * dy);
        }
        private double distanceRevert(double X, double Y, BurnMark burnMark)
        {
            double dx = Math.Abs(X - burnMark.Xto);
            double dy = Math.Abs(Y - burnMark.Yto);
            return Math.Sqrt(dx * dx + dy * dy);
        }
        private BurnMark revert(BurnMark burnMark)
        {
            int tmp = burnMark.Xfrom;
            burnMark.Xfrom = burnMark.Xto;
            burnMark.Xto = tmp;
            tmp = burnMark.Yfrom;
            burnMark.Yfrom = burnMark.Yto;
            burnMark.Yto = tmp;
            return burnMark;
        }

        private void toolStripButtonZoomIn_Click(object sender, EventArgs e)
        {
            setScale(getScale() * 1.1f);
        }

        private void toolStripButtonZoomOut_Click(object sender, EventArgs e)
        {
            float scale = getScale() * 0.9f;
            if (scale > 0)
                setScale(scale);
        }

        private void toolStripButtonZoomFit_Click(object sender, EventArgs e)
        {
            setScale(fitScale());
        }

        private void panelContainer_Paint(object sender, PaintEventArgs e)
        {

        }

        private void toolStripButtonConnect_Click(object sender, EventArgs e)
        {
            if (serialCommunicator.isConnected())
            {
                if (engravingThread != null)
                    messageBox("Сейчас запущена гравировка. Остановите гравировку перед отключением!");
                else
                {
                    toolStripButtonConnect.Text = "Отключение...";
                    disconnect();
                }
            }
            else
            {
                toolStripButtonConnect.Enabled = false;
                toolStripButtonConnect.Text = "Подключение...";
                connect();
            }
        }
        public void connect()
        {
            try
            {
                string selected = toolStripComboBoxComList.SelectedItem.ToString();
                status("Выбранный порт: " + selected);
                sleep(100);
                string name = selected.Split('-')[0].Trim();
                status("Порт для покдлючения: " + name);
                serialCommunicator.connect(name);
            }
            catch (Exception ex)
            {
                status("Ошибка подключения: " + ex.Message);
            }
        }
        public void disconnect()
        {
            try
            {
                serialCommunicator.disconnect();
            }
            catch (Exception ex)
            {
                status("Отключение: " + ex.Message);
            }
        }

        public void hideHorizontalLineMark()
        {
            showHorizontalLineMark(-1);
        }
        public void showHorizontalLineMark(int y)
        {
            float coef = ((float)y) / ((float)resolution.Y);
            if (y == -1)
                coef = -1;
            if (centerScalablePanel != null)
            {
                centerScalablePanel.markPoint.X = -1;
                centerScalablePanel.markPoint.Y = coef;
                centerScalablePanel.Invalidate();
            }
            if (pictureBoxRendered != null)
            {
                pictureBoxRendered.markPoint.X = -1;
                pictureBoxRendered.markPoint.Y = coef;
                pictureBoxRendered.Invalidate();
            }
            Application.DoEvents();
        }
        public void showVerticalLineMark(int x)
        {
            float coef = ((float)x) / ((float)resolution.X);
            if (x == -1)
                coef = -1;
            if (centerScalablePanel != null)
            {
                centerScalablePanel.markPoint.X = coef;
                centerScalablePanel.markPoint.Y = -1;
                centerScalablePanel.Invalidate();
            }
            if (pictureBoxRendered != null)
            {
                pictureBoxRendered.markPoint.X = coef;
                pictureBoxRendered.markPoint.Y = -1;
                pictureBoxRendered.Invalidate();
            }
            Application.DoEvents();
        }
        public void hidePointMark()
        {
            showPointMark(-1, -1);
        }
        public void showPointMark(int x, int y)
        {
            float coefY = ((float)y) / ((float)resolution.Y);
            if (y == -1) coefY = -1;
            float coefX = ((float)x) / ((float)resolution.X);
            if (x == -1) coefX = -1;
            if (centerScalablePanel != null)
            {
                centerScalablePanel.markPoint.X = coefX;
                centerScalablePanel.markPoint.Y = coefY;
                centerScalablePanel.Invalidate();
            }
            if (pictureBoxRendered != null)
            {
                pictureBoxRendered.markPoint.X = coefX;
                pictureBoxRendered.markPoint.Y = coefY;
                pictureBoxRendered.Invalidate();
            }
            Application.DoEvents();
        }

        private void toolStripButtonPlaceholder_Click(object sender, EventArgs e)
        {
            if (centerScalablePanel != null)
            {
                FloatingControl sizeable = new FloatingControl();
                sizeable.setBorderColor(Color.DarkSeaGreen);
                sizeable.setBorderSize(3);
                sizeable.Location = new Point(10, 10);
                sizeable.Size = new System.Drawing.Size(100, 80);
                centerScalablePanel.Controls.Add(sizeable);
                sizeable.BringToFront();
            }
        }

        private String log(String text)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { log(text); });
                return text;
            }
            logText = text + "\n" + logText;
            if(logText.Length > 3000)
                logText = logText.Substring(0, 3000);
            if (logLabel != null)
                logLabel.Text = logText;
            return text;
        }

        private void вывестиОкноЛогаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(logLabel == null)
            {
                log("Отображение лога...");
                new LogWindow(this).Show();
            }
        }
        private void onPositionUpdate(int x, int y)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { onPositionUpdate(x, y); });
                return;
            }
            showPointMark(x, y);
        }
        private void onConnected()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { onConnected(); });
                return;
            }
            toolStripButtonConnect.Enabled = true;
            toolStripButtonConnect.Text = "Отключиться";
            toolStripButtonConnect.Image = new Bitmap(LaserDrawerApp.Properties.Resources.disconnect);
            buttonZero.Enabled = true;
            buttonMoveRightFast.Enabled = true;
            buttonMoveLeftFast.Enabled = true;
            buttonMoveDownFast.Enabled = true;
            buttonMoveUpFast.Enabled = true;
            buttonMoveRightSlow.Enabled = true;
            buttonLaserOn.Enabled = true;
            buttonMoveLeftSlow.Enabled = true;
            buttonMoveDownSlow.Enabled = true;
            buttonMoveUpSlow.Enabled = true;
            buttonLaserTestGo.Enabled = true;
            textBoxLaserTestTime.Enabled = true;
            toolStripComboBoxComList.Enabled = false;
            toolStripButtonRefreshComList.Enabled = false;
            buttonEngrave.Enabled = burnMarks.Count > 0;
            toolStripStatusLabelConnection.Text = "Покдлючено к " + serialCommunicator.deviceName;
        }
        private void onDisconnected()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { onDisconnected(); });
                return;
            }
            toolStripButtonConnect.Enabled = true;
            toolStripButtonConnect.Text = "Покдлючиться";
            toolStripButtonConnect.Image = new Bitmap(LaserDrawerApp.Properties.Resources.web_link);
            buttonZero.Enabled = false;
            buttonMoveRightFast.Enabled = false;
            buttonMoveLeftFast.Enabled = false;
            buttonMoveDownFast.Enabled = false;
            buttonMoveUpFast.Enabled = false;
            buttonMoveRightSlow.Enabled = false;
            buttonLaserOn.Enabled = false;
            buttonMoveLeftSlow.Enabled = false;
            buttonMoveDownSlow.Enabled = false;
            buttonMoveUpSlow.Enabled = false;
            buttonLaserTestGo.Enabled = false;
            textBoxLaserTestTime.Enabled = false;
            toolStripComboBoxComList.Enabled = true;
            toolStripButtonRefreshComList.Enabled = true;
            buttonEngrave.Enabled = false;
            toolStripStatusLabelConnection.Text = "Отключено";
        }

        private void buttonMoveRightSlow_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) serialCommunicator.rightslow();
            label4.Focus();
        }
        private void buttonMoveRightSlow_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) serialCommunicator.stop();
            label4.Focus();
        }
        private void buttonMoveRightFast_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) serialCommunicator.rightfast();
            label4.Focus();
        }
        private void buttonMoveRightFast_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) serialCommunicator.stop();
            label4.Focus();
        }
        private void buttonMoveLeftSlow_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) serialCommunicator.leftslow();
            label4.Focus();
        }
        private void buttonMoveLeftFast_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) serialCommunicator.stop();
            label4.Focus();
        }
        private void buttonMoveUpSlow_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) serialCommunicator.upslow();
            label4.Focus();
        }
        private void buttonMoveUpSlow_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) serialCommunicator.stop();
            label4.Focus();
        }
        private void buttonMoveUpFast_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) serialCommunicator.upfast();
            label4.Focus();
        }
        private void buttonMoveUpFast_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) serialCommunicator.stop();
            label4.Focus();
        }
        private void buttonMoveDownSlow_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) serialCommunicator.downslow();
            label4.Focus();
        }
        private void buttonMoveDownSlow_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) serialCommunicator.stop();
            label4.Focus();
        }
        private void buttonMoveDownFast_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) serialCommunicator.downfast();
            label4.Focus();
        }
        private void buttonMoveDownFast_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) serialCommunicator.stop();
            label4.Focus();
        }
        private void buttonMoveLeftSlow_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) serialCommunicator.stop();
            label4.Focus();
        }
        private void buttonMoveLeftFast_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) serialCommunicator.leftfast();
            label4.Focus();
        }
        private void вывестиОкноЛогаToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (logLabel == null)
            {
                log("Отображение лога...");
                new LogWindow(this).Show();
            }
        }
        private void включитьСветToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (!serialCommunicator.isConnected())
            {
                messageBox("Подключите гравер для выполнения этой операции.");
                return;
            }
            serialCommunicator.ledon();
            status("Подсветка включена.");
        }
        private void выключитьСветToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (!serialCommunicator.isConnected())
            {
                messageBox("Подключите гравер для выполнения этой операции.");
                return;
            }
            serialCommunicator.ledoff();
            status("Подсветка выключена.");
        }
        private void оПрограммеToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new AboutWindow().Show();
        }
        public String messageBox(String text)
        {
            MessageBox.Show(text);
            return text;
        }

        private void buttonLaserTestGo_Click(object sender, EventArgs e)
        {
            try
            {
                int time = Int32.Parse(textBoxLaserTestTime.Text);
                if (time <= 0)
                    log("Время обжига не может быть отрицательным.");
                if (time > 5000)
                    log("Время обжига не может быть больше 5 секунд.");
                serialCommunicator.burntest(time);
            }
            catch(Exception e1)
            {
                log("Время обжига введено неверно.");
            }
        }
       


        long engravingStartTime = 0;
        int engravingStartIndex = 0;
        int engravingEndIndex = 0;
        int engravingProcessingCount = 0;
        int engravingCompleteCount = 0;
        int engravingErrorsCount = 0;
        private void startEngraving()
        {
            try
            {
                engravingStartIndex = startFromSlider.Value;
                engravingEndIndex = endToSlider.Value;
                if (engravingStartIndex < 0 || engravingStartIndex >= engravingEndIndex)
                {
                    messageBox("Начало диапазона гравировки должно быть в пределах от нуля до конца диапазона гравировки");
                    return;
                }
                if (engravingEndIndex < 0 || engravingEndIndex > burnMarks.Count)
                {
                    messageBox("Конец диапазона гравировки должно быть в пределах от начала диапазона гравировки до общего количества команд.");
                    return;
                }
            }
            catch(Exception e)
            {
                messageBox("Текст в строчках диапазона гравировки должен иметь числовой формат");
                return;
            }

            startFromSlider.Enabled = false;
            endToSlider.Enabled = false;
            engravingStartTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            engravingProcessingCount = 0;
            engravingCompleteCount = 0;
            engravingErrorsCount = 0;
            labelEngraveStatus.Text = "Гравируется";
            buttonEngrave.Text = "Остановить";
            buttonEngrave.Image = new Bitmap(LaserDrawerApp.Properties.Resources.stop);
            toolStripButtonRender.Enabled = false;
            buttonPreview.Enabled = false;

            engravingThread = new Thread(engraveAsync);
            engravingThread.Start();
        }
        private void updateEngravingProgress(int count)
        {
            showEngravingStatus(engravingCompleteCount + count);
            status("Гравировка ("+count+") ...");
        }
        private void showEngravingStatus(int complete)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { showEngravingStatus(complete); });
                return;
            }
            labelBurnMarskTotal.Text = burnMarks.Count().ToString();
            labelBurnMarkProcessing.Text = engravingProcessingCount.ToString();
            labelBurnMarksComplete.Text = complete.ToString();
            labelBurnMarksErrors.Text = engravingErrorsCount.ToString();
            long now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            labelEngraveTime.Text = showTime(now - engravingStartTime);
            labelEngraveTimeRemaining.Text = showTime(getTimeRemaining(burnMarks, engravingCompleteCount));
            labelNumberOfEngravings.Text = engravingNumberOfTimes.ToString();
            progress(100 * complete / burnMarks.Count());
            Application.DoEvents();
        }
        private String showTime(long millis)
        {
            String result = "";
            if (millis > (1000 * 60 * 60))
            {
                result += (millis / (1000 * 60 * 60)) + " ч ";
                millis = millis % (1000 * 60 * 60);
            }
            if (millis > (1000 * 60))
            {
                result += (millis / (1000 * 60)) + " мин ";
                millis = millis % (1000 * 60);
            }
            if (millis > (1000))
            {
                result += (millis / (1000)) + " сек ";
                millis = millis % (1000);
            }
            if (result.Length == 0)
                result = "0 сек";
            return result;
        }
        private void engraveAsync()
        {
            List<BurnMark> subList = new List<BurnMark>();
            int engraveParts = 125;//125
            long maxPartTime = 300000;//на гравер одной пачкой нельзя загружать задач на время гравировки более 5 минут
            long partTime = 0;//суммарное время гравировки в миллисекундах загружаемой последовательности команд
            for (int i = engravingStartIndex; i< engravingEndIndex && engravingThread == Thread.CurrentThread; i++)
            {
                subList.Add(burnMarks[i]);
                partTime += burnMarks[i].predictedTime();
                if (debug)
                    status("Добавление в массив элемента " + i + " ...");
                if(subList.Count == 1)
                    status("Гравировка, подготовка данных...");
                if (subList.Count >= engraveParts || partTime > maxPartTime)
                {
                    status("Гравировка, ожидание готовности гравера...");
                    serialCommunicator.waitUntilBufferEmpty();
                    status("Гравировка, отправка на гравер "+ subList.Count + " команд...");
                    int errorsUpload = serialCommunicator.upload(subList);
                    engravingErrorsCount += errorsUpload;
                    showEngravingStatus(engravingCompleteCount);
                    if (errorsUpload >= 10)
                    {
                        status("При отправке команд возникло более " + errorsUpload + " ошибок! Связь прервана.");
                        subList.Clear();
                        partTime = 0;
                        break;
                    }
                    if (errorsUpload < 10)
                    {
                        int errorsExecute = serialCommunicator.execute();
                        engravingErrorsCount += errorsExecute;
                        if (errorsExecute >= 10)
                        {
                            status("При выполнении команд возникло более " + errorsExecute + " ошибок! Связь прервана.");
                            subList.Clear();
                            partTime = 0;
                            break;
                        }

                        if (errorsExecute < 10)
                        {
                            engravingCompleteCount = i;
                            showEngravingStatus(engravingCompleteCount);
                            Thread.Sleep(100);
                            subList.Clear();
                            partTime = 0;
                            if (engravingThread != Thread.CurrentThread)
                                break;
                        }
                    }
                }
            }
            if(subList.Count > 0)
            {
                engraveStatus("Завершение...");
                status("Завершение гравировки, ожидание готовности гравера...");
                serialCommunicator.waitUntilBufferEmpty();
                status("Завершение гравировки, отправка на гравер " + subList.Count + " команд...");
                int errorsUpload = serialCommunicator.upload(subList);
                engravingErrorsCount += errorsUpload;
                if (errorsUpload >= 10) status("При отправке данных возникло более " + errorsUpload + " ошибок! Связь прервана.");
                if (errorsUpload < 10)
                {
                    int errorsExecute = serialCommunicator.execute();
                    engravingErrorsCount += errorsExecute;
                    if (errorsUpload >= 10)
                    {
                        status("При выполнении команд возникло более " + errorsExecute + " ошибок! Связь прервана.");
                    }
                    else
                    {
                        engravingCompleteCount += subList.Count;
                    }
                }
            }
            engraveStatus("Остановка гравировки...");
            showEngravingStatus(engravingCompleteCount);
            stopEngraving();
            Thread.Sleep(1000);
            engraveStatus("Возврат каретки в 0...");
            serialCommunicator.go0();
            Thread.Sleep(3000);
            engraveStatus("Отключение моторов...");
            serialCommunicator.releare();
            Thread.Sleep(1000);
            engraveStatus(status(System.DateTime.Now.ToString("HH:mm:ss") + " Завершена гравировка " + engravingCompleteCount + " команд."));
        }
        private void engraveStatus(String text)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { engraveStatus(text); });
                return;
            }
            labelEngraveStatus.Text = text;
        }
        private long getTimeRemaining(List<BurnMark> subList, int currentIndex)
        {
            status("Расчёт оставшегося времени...");
            float travelDelay = 2; //ms per pixel
            float dataTransferDelay = 60; //ms per command
            long sumTime = 0;
            float lastX = 0;
            float lastY = 0;
            for(int i=currentIndex; i<subList.Count; i++)
            {
                float burningAmount = Math.Abs(subList[i].Xto - subList[i].Xfrom);
                float burningTime = subList[i].burnTimeMs;
                float travelX = Math.Abs(lastX - subList[i].Xfrom);
                float travelY = Math.Abs(lastY - subList[i].Yfrom);
                float travel = travelX + travelY;
                float burningPredictTime = burningAmount * burningTime;
                float dataTransferPredictTime = dataTransferDelay;
                float travelTime = travelDelay * travel;
                lastX = subList[i].Xto;
                lastY = subList[i].Yto;
                sumTime += (long)(dataTransferPredictTime + travelTime + burningPredictTime);
            }
            return (long)(sumTime * 1.5);
        }
        private void stopEngraving()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { stopEngraving(); });
                return;
            }
            engravingThread = null;
            labelEngraveStatus.Text = "Остановлено";
            buttonEngrave.Text = "Гравировка";
            buttonEngrave.Enabled = true;
            toolStripButtonRender.Enabled = true;
            buttonPreview.Enabled = true;
            startFromSlider.Enabled = true;
            endToSlider.Enabled = true;
            buttonEngrave.Image = new Bitmap(LaserDrawerApp.Properties.Resources.fire);
            showEngravingStatus(engravingCompleteCount);
            progress(0);
        }

        public void onStartRendering()
        {
            buttonEngrave.Enabled = false;
            toolStripButtonRender.Text = "Остановить";
            toolStripButtonRender.Image = new Bitmap(LaserDrawerApp.Properties.Resources.stop);
            engravingStartTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            engravingStartIndex = 0;
            engravingEndIndex = 0;
            engravingProcessingCount = 0;
            engravingCompleteCount = 0;
            engravingErrorsCount = 0;
            showEngravingStatus(engravingCompleteCount);
        }

        private void onRendered()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { onRendered(); });
                return;
            }
            renderThread = null;
            autosave();
            toolStripButtonRender.Text = "Рендеринг";
            toolStripButtonRender.Image = new Bitmap(LaserDrawerApp.Properties.Resources.settings_gears);
            progress(0);
            startFromSlider.Enabled = true;
            startFromSlider.Minimum = 0;
            startFromSlider.Maximum = burnMarks.Count - 1;
            startFromSlider.Value = 0;
            startFromLabel.Text = startFromSlider.Value.ToString();
            endToSlider.Enabled = true;
            endToSlider.Minimum = 0;
            endToSlider.Maximum = burnMarks.Count - 1;
            endToSlider.Value = burnMarks.Count - 1;
            endToLabel.Text = endToSlider.Value.ToString();
            labelBurnMarskTotal.Text = (burnMarks.Count).ToString();
            labelEngraveTimeRemaining.Text = showTime(getTimeRemaining(burnMarks, 0));
            engravingStartTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            previewEngrave();
            toolStripButtonSave.Enabled = true;
            buttonPreview.Enabled = true;
            if (serialCommunicator.isConnected())
            {
                labelEngraveStatus.Text = "Готов к гравировке";
                buttonEngrave.Enabled = true;
            }
            else
            {
                labelEngraveStatus.Text = "Рендеринг выполнен";
                buttonEngrave.Enabled = false;
            }
        }

        private void pictureBoxRendered_Click(object sender, EventArgs e)
        {
        }

        private void buttonEngrave_Click(object sender, EventArgs e)
        {
            if (engravingThread != null)
            {
                var confirmResult = MessageBox.Show("Гравировка сейчас работает. Вы уверены, что хотите её остановить?",
                                 "Предупреждение",
                                 MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    serialCommunicator.stop();
                    engravingThread = null;
                    buttonEngrave.Text = "Остановка...";
                    buttonEngrave.Enabled = false;
                }
            }
            else
            {
                startEngraving();
            }
        }

        private void buttonZero_Click(object sender, EventArgs e)
        {
            serialCommunicator.go0();
        }

        private void выводитьОтладочныеСообщенияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            выводитьОтладочныеСообщенияToolStripMenuItem.Checked = debug = serialCommunicator.debug = !выводитьОтладочныеСообщенияToolStripMenuItem.Checked;
        }

        private void информацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!serialCommunicator.isConnected())
            {
                messageBox("Подключите гравер для выполнения этой операции.");
                return;
            }
            status("Проверка связи...");
            serialCommunicator.status();
            status("Проводится тестирование...");
            if (serialCommunicator.selftest())
            {
                status("Тест пройден.");
                messageBox("Само-тестирование пройдено. Механика полностью исправна.");
            }
            else
            {
                status("Тест провален.");
                messageBox("Обнаружены ошибки при само-тестировании! Тест не пройден. " +
                    "Вероятно, посторонние предметы мешают движению каретки. " +
                    "Механика гравера требует ремонта или настройки!");
            }
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            if (burnMarks.Count == 0)
                return;
            // get path to save file
            string fn = System.DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + " rendered paths.mcp"; //this combobox is my report name
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = fn.Replace("/", "-");
            sfd.Filter = "(*.mcp)|*.mcp";
            sfd.ShowDialog();
            string path = sfd.FileName;
            save(path);
            toolStripButtonSave.Enabled = false;
            status("Готово.");
        }
        private void autosave()
        {
            if (burnMarks.Count == 0)
                return;
            if (Directory.Exists("autosave"))
            {
                String[] files = Directory.GetFiles("autosave");
                for (int i = 0; i < files.Length - 50; i++)
                    File.Delete(files[i]);
            }
            else
            {
                Directory.CreateDirectory("autosave");
            }
            string fn = "autosave\\" + System.DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + " autosave rendered paths.mcp";
            save(fn);
        }
        private void save(String filename)
        {
            //Write to a file
            using (StreamWriter writer = new StreamWriter(filename))
            {
                //Add the Header row for CSV file.
                int cnt = 0;
                foreach (BurnMark row in burnMarks)
                {
                    if (cnt % 1234 == 0)
                    {
                        int percent = 100 * cnt / burnMarks.Count;
                        status("Сохранение файла (" + percent + "%)...");
                    }
                    writer.WriteLine(row.ToString());
                    cnt++;
                }
            }

        }

        private void toolStripButtonOpen_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog theDialog = new OpenFileDialog();
                theDialog.Title = "Open Paths File";
                theDialog.Filter = "myCNC paths|*.mcp";
                theDialog.InitialDirectory = Directory.GetCurrentDirectory() + "\\autosave";
                if (theDialog.ShowDialog() == DialogResult.OK)
                {
                    String pathToFile = theDialog.FileName;//doesn't need .tostring because .filename returns a string// saves the location of the selected object

                    if (File.Exists(pathToFile))// only executes if the file at pathtofile exists//you need to add the using System.IO reference at the top of te code to use this
                    {
                        status("Открытие файла...");
                        //method2
                        string text = "";
                        using (StreamReader sr = new StreamReader(pathToFile))
                            text = sr.ReadToEnd();//all text wil be saved in text enters are also saved
                        burnMarks.Clear();
                        string[] lines = text.Split('\n');
                        int maxX = 0;
                        int maxY = 0;
                        int minX = 100;
                        int minY = 100;
                        foreach (string line in lines)
                        {
                            if (line.Length > 0)
                            {
                                BurnMark bm = new BurnMark(line);
                                burnMarks.Add(bm);
                                if (bm.Xfrom > maxX) maxX = bm.Xfrom;
                                if (bm.Xto > maxX) maxX = bm.Xto;
                                if (bm.Yfrom > maxY) maxY = bm.Yfrom;
                                if (bm.Yto > maxY) maxY = bm.Yto;
                                if (bm.Yfrom < minY) minY = bm.Yfrom;
                                if (bm.Yto < minY) minY = bm.Yto;
                                if (bm.Xfrom < minX) minX = bm.Xfrom;
                                if (bm.Xto < minX) minX = bm.Xto;
                            }
                        }
                        int sizeX = maxX + minX;
                        int sizeY = maxY + minY;
                        int size = Math.Max(sizeX, sizeY);
                        newProject(size, size);
                        renderedImage = new Bitmap(size, size);

                        try
                        {
                            drawPreview(renderedImage, burnMarks);
                            status("Реконструкция подготовлена, нарисовано " + burnMarks.Count + " инструкций.");
                            showRendered(renderedImage);
                            hidePointMark();
                        }
                        finally
                        {
                            onRendered();
                        }
                        status("Готово.");
                    }
                }
            }
            catch (Exception ex)
            {
                messageBox("Ошибка открытия файла: " + ex.Message);
            }
        }
        
        void previewEngrave()
        {
            engravingStartIndex = startFromSlider.Value;
            engravingEndIndex = endToSlider.Value;
            List<BurnMark> selected = burnMarks.GetRange(engravingStartIndex, engravingEndIndex - engravingStartIndex);
            drawPreview(renderedImage, selected);
            status("Реконструкция подготовлена, нарисовано " + selected.Count + " инструкций.");
            int maxTime = 0;
            for (int i = 0; i < selected.Count; i++)
                if (selected[i].burnTimeMs > maxTime)
                    maxTime = selected[i].burnTimeMs;
            showRendered(renderedImage);
            labelBurnTime.Text = maxTime + " мс/пикс.";
            labelBurnMarskTotal.Text = burnMarks.Count().ToString();
            labelBurnMarkProcessing.Text = "0";
            labelBurnMarksComplete.Text = "0";
            labelBurnMarksErrors.Text = "0";
            labelEngraveTimeRemaining.Text = showTime(getTimeRemaining(selected, 0));
            labelEngraveTime.Text = showTime(0);
            hidePointMark();
            status("Готово.");
        }

        private void startFromSlider_ValueChanged(object sender, EventArgs e)
        {
            int min = startFromSlider.Value;
            if (endToSlider.Value < min)
                endToSlider.Value = min;
            startFromLabel.Text = min.ToString();
        }

        private void startFromSlider_MouseUp(object sender, MouseEventArgs e)
        {
            previewEngrave();
        }

        private void toolStripButtonRender_Click_1(object sender, EventArgs e)
        {
            if (renderThread == null)
            {
                startRendering();
            }
            else
            {
                stopRendering();
            }
        }

        private void endToSlider_ValueChanged(object sender, EventArgs e)
        {
            int max = endToSlider.Value;
            if (startFromSlider.Value > max)
                startFromSlider.Value = max;
            endToLabel.Text = max.ToString();
        }

        private void endToSlider_MouseUp(object sender, MouseEventArgs e)
        {
            previewEngrave();
        }

        private void buttonLaserOn_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left) serialCommunicator.laseron();
        }
        private void buttonLaserOn_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) serialCommunicator.stop();
        }

        private void информацияОСостоянииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (serialCommunicator.isConnected())
                messageBox(serialCommunicator.version());
            else
                messageBox("Подключите гравер для выполнения этой операции.");
        }

        private void panelContainer_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            float coefX = ((float)e.X) / ((float)centerScalablePanel.Width);
            float coefY = ((float)e.Y) / ((float)centerScalablePanel.Height);
            float coordX = resolution.X * coefX;
            float coordY = resolution.Y * coefY;
            //messageBox("doubleblick " + coordX + " x " + coordY);
            if (serialCommunicator.isConnected())
            {
                status("Перемещение на координаты " + coordX + " " + coordY + "...");
                serialCommunicator.goTo((int)coordX, (int)coordY);
                serialCommunicator.pos();
            }
        }

        private void обесточитьМоторыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (serialCommunicator.isConnected())
            {
                serialCommunicator.releare();
                status("Моторы обесточены");
            }
            else
                messageBox("Подключите гравер для выполнения этой операции.");
        }

        private void безОптимизацииМаршрутаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            безОптимизацииМаршрутаToolStripMenuItem.Checked = true;
            гравировкаПоЗигзагуToolStripMenuItem.Checked = false;
            гравировкаПоКратчайшемуМаршрутуToolStripMenuItem.Checked = false;
            гравировкаВСлучайномПорядкеToolStripMenuItem.Checked = false;
        }

        private void гравировкаПоЗигзагуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            безОптимизацииМаршрутаToolStripMenuItem.Checked = false;
            гравировкаПоЗигзагуToolStripMenuItem.Checked = true;
            гравировкаПоКратчайшемуМаршрутуToolStripMenuItem.Checked = false;
            гравировкаВСлучайномПорядкеToolStripMenuItem.Checked = false;
        }

        private void гравировкаПоКратчайшемуМаршрутуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            безОптимизацииМаршрутаToolStripMenuItem.Checked = false;
            гравировкаПоЗигзагуToolStripMenuItem.Checked = false;
            гравировкаПоКратчайшемуМаршрутуToolStripMenuItem.Checked = true;
            гравировкаВСлучайномПорядкеToolStripMenuItem.Checked = false;
        }

        private void гравировкаВСлучайномПорядкеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            безОптимизацииМаршрутаToolStripMenuItem.Checked = false;
            гравировкаПоЗигзагуToolStripMenuItem.Checked = false;
            гравировкаПоКратчайшемуМаршрутуToolStripMenuItem.Checked = false;
            гравировкаВСлучайномПорядкеToolStripMenuItem.Checked = true;
        }

        private void использоватьПолутонаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            использоватьПолутонаToolStripMenuItem.Checked = !использоватьПолутонаToolStripMenuItem.Checked;
        }

        Keys lastDown = Keys.KeyCode;
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != lastDown)
            {
                lastDown = e.KeyCode;
                log("DOWN " + e.KeyCode);
                if (engravingThread != null)
                    return;

                if (e.KeyCode == Keys.Control)
                    buttonLaserOn.Focus();
                if (e.KeyCode == Keys.Right)
                    serialCommunicator.rightfast();
                if (e.KeyCode == Keys.Left)
                    serialCommunicator.leftfast();
                if (e.KeyCode == Keys.Up)
                    serialCommunicator.upfast();
                if (e.KeyCode == Keys.Down)
                    serialCommunicator.downfast();
                if (e.KeyCode == Keys.Space)
                    serialCommunicator.laseron();
            }
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (lastDown == e.KeyCode)
                lastDown = Keys.KeyCode;
            log("UP " + e.KeyCode);
            if (engravingThread == null && 
                (e.KeyCode == Keys.Left || e.KeyCode == Keys.Up || e.KeyCode == Keys.Right 
                || e.KeyCode == Keys.Down || e.KeyCode == Keys.Space) )
            {
                serialCommunicator.stop();
            }
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            //TODO запустить поток воспроизведения процесса гравировки
            if (previewThread == null)
            {
                buttonPreview.Text = "Остановить";
                buttonPreview.Image = new Bitmap(LaserDrawerApp.Properties.Resources.stop);
                buttonPreviewSpeed1x.Visible = true;
                buttonPreviewSpeed2x.Visible = true;
                buttonPreviewSpeed5x.Visible = true;
                buttonPreviewSpeed10x.Visible = true;
                buttonPreviewSpeed50x.Visible = true;
                buttonPreviewSpeed100x.Visible = true;
                buttonPreviewSpeed200x.Visible = true;

                previewThread = new Thread(previewAsync);
                previewThread.Start();
            }
            else
            {
                previewThread = null;
                buttonPreview.Text = "Предпросмотр";
                buttonPreview.Image = new Bitmap(LaserDrawerApp.Properties.Resources.play);
                buttonPreviewSpeed1x.Visible = false;
                buttonPreviewSpeed2x.Visible = false;
                buttonPreviewSpeed5x.Visible = false;
                buttonPreviewSpeed10x.Visible = false;
                buttonPreviewSpeed50x.Visible = false;
                buttonPreviewSpeed100x.Visible = false;
                buttonPreviewSpeed200x.Visible = false;
            }
        }

        private void previewAsync() //to be launched in separate thread
        {
            for(int i=0; i<burnMarks.Count && previewThread != null; i+=previewSpeed)
            {
                progress(100 * i / burnMarks.Count);
                List<BurnMark> selected = burnMarks.GetRange(0, i);
                drawPreview(renderedImage, selected);
                status("Реконструкция " + i + " инструкций ("+ previewSpeed + "x)");
                showRendered(renderedImage);
            }

            previewThread = null;
            Invoke((MethodInvoker)delegate {
                buttonPreview.Text = "Предпросмотр";
                buttonPreview.Image = new Bitmap(LaserDrawerApp.Properties.Resources.play);
                buttonPreviewSpeed1x.Visible = false;
                buttonPreviewSpeed2x.Visible = false;
                buttonPreviewSpeed5x.Visible = false;
                buttonPreviewSpeed10x.Visible = false;
                buttonPreviewSpeed50x.Visible = false;
                buttonPreviewSpeed100x.Visible = false;
                buttonPreviewSpeed200x.Visible = false;
            });

            drawPreview(renderedImage, burnMarks);
            showRendered(renderedImage);
            progress(100);
            status("Готово.");
        }

        private void endToSlider_Scroll(object sender, EventArgs e)
        {

        }

        private void buttonPreviewSpeed1x_Click(object sender, EventArgs e)
        {
            previewSpeed = 1;
        }

        private void buttonPreviewSpeed2x_Click(object sender, EventArgs e)
        {
            previewSpeed = 2;
        }

        private void buttonPreviewSpeed5x_Click(object sender, EventArgs e)
        {
            previewSpeed = 5;
        }

        private void buttonPreviewSpeed10x_Click(object sender, EventArgs e)
        {
            previewSpeed = 10;
        }

        private void buttonPreviewSpeed50x_Click(object sender, EventArgs e)
        {
            previewSpeed = 50;
        }

        private void buttonPreviewSpeed100x_Click(object sender, EventArgs e)
        {
            previewSpeed = 100;
        }

        private void buttonPreviewSpeed200x_Click(object sender, EventArgs e)
        {
            previewSpeed = 200;
        }
    }


    public class BurnMark
    {
        public int burnTimeMs = 0;
        public int Xfrom = 0;
        public int Xto = 0;
        public int Yfrom = 0;
        public int Yto = 0;

        public BurnMark(String toParse)
        {
            String[] splitted = toParse.Split('_');
            burnTimeMs = Int32.Parse(splitted[0]);
            Xfrom = Int32.Parse(splitted[1]);
            Xto = Int32.Parse(splitted[2]);
            Yfrom = Int32.Parse(splitted[3]);
            Yto = Int32.Parse(splitted[4]);
        }
        public BurnMark(int _burnTimeMs, int _Xfrom, int _Xto, int _Yfrom, int _Yto)
        {
            burnTimeMs = _burnTimeMs;
            Xfrom = _Xfrom;
            Xto = _Xto;
            Yfrom = _Yfrom;
            Yto = _Yto;
        }
        public long predictedTime()
        {
            return burnTimeMs + Math.Abs(Xfrom - Xto);
        }
        public override String ToString()
        {
            return burnTimeMs + "_" + Xfrom + "_" + Xto + "_" + Yfrom + "_" + Yto;
        }
        
    }
}
