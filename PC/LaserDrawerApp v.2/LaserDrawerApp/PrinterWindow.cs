using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LaserDrawerApp
{
    public partial class PrinterWindow : Form
    {
        public Point resolution = new Point(-1, -1);
        public Bitmap image = null;

        private WaitingWindow waitingWindow = null;
        private SerialPort port = null;
        private bool paused = false;


        private void refreshCOMs()
        {
            comboBoxComPorts.Items.Clear();
            string[] COMs = SerialPort.GetPortNames();
            int sel = 0;
            for (int i = 0; i < COMs.Length; i++)
            {
                String com = COMs[i];
                if (!com.Equals("COM1"))
                    sel = i;
                comboBoxComPorts.Items.Add(com);
            }
            comboBoxComPorts.SelectedIndex = sel;
            writeLog("");
            writeLog("COM port list loaded.");
        }
        private void writeStatus(String text)
        {
            writeLog(text);
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { writeStatus(text); });
                return;
            }
            // this code will run on main (UI) thread 
            labelStatus.Text = text;
            Application.DoEvents();
        }
        private void writeJob(String text)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { writeJob(text); });
                return;
            }
            // this code will run on main (UI) thread 
            labelJob.Text = text;
            Application.DoEvents();
        }
        private void writeLog(String text)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { writeLog(text); });
                return;
            }
            // this code will run on main (UI) thread 
            textBoxLog.AppendText(text + "\n");//Text += (text + "\n");
            //if (textBoxLog.Text.Length > 4000)
            //    textBoxLog.Text = textBoxLog.Text.Substring(0, 3000);
            textBoxLog.ScrollToCaret();
            Application.DoEvents();
        }
        private void writeResolution(String text)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { writeResolution(text); });
                return;
            }
            // this code will run on main (UI) thread 
            labelResolution.Text = text;
            Application.DoEvents();
        }
        private void makeJobGreen()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { makeJobGreen(); });
                return;
            }
            // this code will run on main (UI) thread 
            labelJob.BackColor = Color.Honeydew;
            Application.DoEvents();
        }
        private void makeJobRed()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { makeJobRed(); });
                return;
            }
            // this code will run on main (UI) thread 
            labelJob.BackColor = Color.LavenderBlush;
            Application.DoEvents();
        }
        private void makeJobYellow()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { makeJobYellow(); });
                return;
            }
            // this code will run on main (UI) thread 
            labelJob.BackColor = Color.LemonChiffon;
            Application.DoEvents();
        }
        private void showModal()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { showModal(); });
                return;
            }
            hideModal();
            panelConnector.Visible = false;
            panelFunctions.Visible = false;

            waitingWindow = new WaitingWindow();
            waitingWindow.StartPosition = FormStartPosition.CenterParent;
            waitingWindow.Show();
            waitingWindow.TopMost = true;
            Point p = new Point(this.Location.X + (this.Width / 2 - waitingWindow.Width / 2), this.Location.Y + (this.Height / 2 - waitingWindow.Height / 2));
            waitingWindow.Location = p;
            Application.DoEvents();
        }
        private void hideModal()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { hideModal(); });
                return;
            }
            panelConnector.Visible = true;
            panelFunctions.Visible = true;
            if (waitingWindow != null)
            {
                waitingWindow.Close();
                waitingWindow = null;
            }
            Application.DoEvents();
        }
        private String getSelectedCom()
        {
            String text = null;
            this.Invoke((MethodInvoker)delegate()
            {
                text = comboBoxComPorts.Text;
            });
            while (text == null) ;
            return text;
        }
        private void selectFile()
        {
            if (resolution.X == -1)
            {
                log("Для начала подключитесь к принтеру.");
                return;
            }
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "Изображения(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    Bitmap file = new Bitmap(open.FileName);
                    Bitmap canvas = new Bitmap(resolution.X, resolution.Y);
                    Graphics graphics = Graphics.FromImage(canvas);
                    graphics.FillRectangle(new SolidBrush(Color.White), 0, 0, resolution.X, resolution.Y);
                    int height = resolution.Y;
                    int width = (height * file.Width) / file.Height;
                    if (width > resolution.X)
                    {
                        width = resolution.X;
                        height = (width * file.Height) / file.Width;
                    }
                    graphics.DrawImage(file, 0, 0, width, height);
                    image = canvas;
                    showImage(image);
                }
            }
            catch (Exception e)
            {
                log("Ошибка открытия изображения: " + e.ToString());
            }
        }
        private Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }
        private void markDot(int x, int y)
        {
            Color pixel = image.GetPixel(x, y);
            int intensivity = 200;

            int r = pixel.R - intensivity;
            if (r > 255) r = 255;
            if (r < 0) r = 0;

            int g = pixel.G - (intensivity/2);
            if (g > 255) g = 255;
            if (g < 0) g = 0;

            int b = pixel.B - intensivity;
            if (b > 255) b = 255;
            if (b < 0) b = 0;

            image.SetPixel(x, y, Color.FromArgb(r,g,b));
        }
        private int getPixel(int x, int y)
        {
            // 0...9
            int r = image.GetPixel(x, y).R;
            int g = image.GetPixel(x, y).G;
            int b = image.GetPixel(x, y).B;
            int px = (r + g + b) / 3;
            if (px > 240)
                px = 0;
            else
                px = 255 - px;
            px = (px * 9) / 255;
            if (px > 9) px = 9;
            return px;
        }
        private bool haveRemaining(int fx)
        {
            for (int x = fx; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                    if (getPixel(x, y) > 0)
                        return true;
            }
            return false;
        }
        public void showImage(Bitmap bitmap)
        {
            int c = 3;
            Size size = new Size(bitmap.Width * c, bitmap.Height * c);
            Bitmap newBitmap = new Bitmap(size.Width, size.Height);
            Graphics graphics = Graphics.FromImage(newBitmap);
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            graphics.DrawImage(bitmap, 0, 0, size.Width, size.Height);
            pictureBoxImage.Image = newBitmap;
        }
        public String log(String text)
        {
            writeLog(text);
            return text;
        }

        private void connect()
        {
            if (port != null)
            {
                writeStatus("Уже подключено.");
                writeJob("Подключено.");
                return;
            }
            showModal();
            makeJobYellow();
            writeJob("Подключение...");
            try
            {
                port = new SerialPort(getSelectedCom(), 38400);
                port.Open();
                restart();
                getResolution();
                writeJob("Подключено.");
                makeJobGreen();
            }
            catch (Exception e)
            {
                port = null;
                writeJob("Ошибка подключения");
                writeStatus("Ошибка подключения: " + e.ToString());
                makeJobRed();
            }
        }
        private void disconnect()
        {
            if (port != null)
            {
                makeJobYellow();
                writeJob("Отключение...");
                try
                {
                    port.Close();
                    port = null;
                    writeJob("Отключено.");
                    makeJobRed();
                }
                catch (Exception e)
                {
                    writeJob("Ошибка отключения");
                    writeStatus("Ошибка отключения: " + e.ToString());
                    makeJobRed();
                }
            }
        }
        private String receiveString()
        {
            //todo ДОБАВИТЬ КЭШ С ВОЗМОЖНОСТЬЮ УЧЕТА РАЗМЕРА ОЧЕРЕДИ
            if (port != null)
            {
                String text = port.ReadLine();
                writeLog("|#| --> " + text);
                return text;
            }
            return "";
        }
        private void sendString(String text)
        {
            if (port != null)
            {
                writeLog("|#| <-- " + text);
                port.WriteLine(text);
            }
            else
            {
                writeLog("|X| <-- " + text);
            }
        }
        private void waitDone()
        {
            if (port == null)
                return;
            String received;
            do
            {
                received = receiveString();
                Application.DoEvents();
            } while (!received.Contains("Done."));
            clearBuffers();
        }
        private void clearBuffers()
        {
            port.DiscardInBuffer();
            port.DiscardOutBuffer();
        }
        private void restart()
        {
            sendString("reload");
            waitDone();
        }
        private void sendCommand(String text)
        {
            if (waitingWindow == null)
            {
                showModal();
                writeJob("Выполнение " + text + " ...");
                sendString(text);
                waitDone();
                writeJob("Готово.");
                hideModal();
            }
        }
        private void getResolution()
        {
            showModal();
            writeJob("Получение разрешения...");
            sendString("resolution");
            String input = receiveString();
            String[] parts = input.Split(';');
            if (parts.Length == 2)
            {
                int maxX = Int32.Parse(parts[0]);
                int maxY = Int32.Parse(parts[1]);
                resolution = new Point(maxX, maxY);
                log("Разрешение получено: " + maxX + "x" + maxY);
            }
            else
            {
                log("Получены некорректные данные. ");
            }
            waitDone();
            writeResolution(resolution.X + " x " + resolution.Y);
            image = new Bitmap(resolution.X, resolution.Y);
            Graphics.FromImage(image).FillRectangle(new SolidBrush(Color.White), 0, 0, image.Width - 1, image.Height - 1);
            showImage(image);
            hideModal();
        }
        private void print()
        {
            if (resolution.X == -1)
            {
                log("Для начала подключитесь к принтеру.");
                return;
            }
            if (image == null)
            {
                log("Для начала выберите рисунок.");
                return;
            }
            showModal();
            writeJob("Печать...");
            sendString("print");
            while (true)
            {
                String received = receiveString();
                if (received.Contains("Done."))
                {
                    clearBuffers();
                    writeJob("Ура! Печать завершена");
                    break;
                }
                if (received.Contains("GET"))
                {
                    String[] parts = received.Split(';');
                    if (parts.Length == 2)
                    {
                        if (parts[0].Equals("GET"))
                        {
                            int x = Int32.Parse(parts[1]);
                            if (!haveRemaining(x))
                            {
                                sendString("stop");
                            }
                            else if (paused)
                            {
                                sendString("pause");
                                while (paused) ;
                            }
                            else
                            {
                                String answer = "";
                                for (int y = 0; y < image.Height; y++)
                                {
                                    answer += (getPixel(x, y).ToString()) + ";";
                                    markDot(x, y);
                                }
                                showImage(image);
                                Application.DoEvents();
                                answer += "0";
                                sendString(answer);
                            }
                        }
                    }
                    Application.DoEvents();
                }
            }
            hideModal();
        }

        public PrinterWindow()
        {
            InitializeComponent();

        }
        private void PrinterWindow_Load(object sender, EventArgs e)
        {
            refreshCOMs();
        }
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            refreshCOMs();
        }
        private void buttonConnect_Click(object sender, EventArgs e)
        {
            new Thread(connect).Start();
        }
        private void buttonAnalogs_Click(object sender, EventArgs e)
        {
            new Thread(() => sendCommand("analog")).Start();
        }
        private void buttonPlatformForward_Click(object sender, EventArgs e)
        {
            new Thread(() => sendCommand("pl fw")).Start();
        }
        private void buttonPlatformBackward_Click(object sender, EventArgs e)
        {
            new Thread(() => sendCommand("pl bw")).Start();
        }
        private void buttonLaserForward_Click(object sender, EventArgs e)
        {
            new Thread(() => sendCommand("ls fw")).Start();
        }
        private void buttonLaserBackward_Click(object sender, EventArgs e)
        {
            new Thread(() => sendCommand("ls bw")).Start();
        }
        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            new Thread(disconnect).Start();
        }
        private void buttonStop_Click(object sender, EventArgs e)
        {
            new Thread(() => sendString("stop")).Start();
        }
        private void buttonLaserMotorTest_Click(object sender, EventArgs e)
        {
            new Thread(() => sendCommand("ls test")).Start();
        }
        private void buttonTestPlatform_Click(object sender, EventArgs e)
        {
            new Thread(() => sendCommand("pl test")).Start();
        }
        private void buttonTestMulti_Click(object sender, EventArgs e)
        {
            new Thread(() => sendCommand("multi test")).Start();
        }
        private void buttonGetLaserTime_Click(object sender, EventArgs e)
        {
            new Thread(() => sendCommand("get laser time")).Start();
        }
        private void buttonSetBurningTime_Click(object sender, EventArgs e)
        {
            String text = textBoxBurningTime.Text;
            new Thread(() => sendCommand("set laser time " + text)).Start();
        }
        private void buttonResetMotors_Click(object sender, EventArgs e)
        {
            new Thread(() => sendCommand("reset motors")).Start();
        }
        private void buttonGoto_Click(object sender, EventArgs e)
        {
            String textX = textBoxGotoX.Text;
            String textY = textBoxGotoY.Text;
            new Thread(() => sendCommand("goto " + textX + " " + textY)).Start();
        }
        private void buttonSelectFile_Click(object sender, EventArgs e)
        {
            selectFile();
        }
        private void buttonPrint_Click(object sender, EventArgs e)
        {
            new Thread(print).Start();
        }
        private void buttonReadString_Click(object sender, EventArgs e)
        {
            new Thread(clearBuffers).Start();
        }
        private void buttonPause_Click(object sender, EventArgs e)
        {
            if (paused)
            {
                paused = false;
                buttonPause.Text = "Приостановить";
                log("Если печать включена - она возобновлена.");
            }
            else
            {
                paused = true;
                buttonPause.Text = "Возобновить";
                log("Если печать включена - она приостановлена.");
            }
        }
        private void buttonTestLaser_Click(object sender, EventArgs e)
        {
            new Thread(() => sendCommand("lsr test")).Start();
        }
        private void buttonWriteText_Click(object sender, EventArgs e)
        {
            if (resolution.X == -1)
            {
                log("Для начала подключитесь к принтеру.");
                return;
            }
            new WriteText(this).Show();
        }
    }
}
