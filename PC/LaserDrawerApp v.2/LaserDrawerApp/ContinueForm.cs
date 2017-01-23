using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace LaserDrawerApp
{
    public partial class ContinueForm : Form
    {
        // ReSharper disable All
        private LaserDrawer2Window laserDrawer;
        private int steps = 0;
        private Bitmap image = null;
        private int burnTime = 0;
        private int beginFrom = 0;
        private bool cont = false;
        private bool verticalOptimisation = false;

        public ContinueForm(LaserDrawer2Window centerScalable)
        {
            InitializeComponent();
            laserDrawer = centerScalable;
        }
        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                int divider = steps = Int32.Parse(textBoxDivider.Text);
                int resX = laserDrawer.resolution.X / divider;
                int resY = laserDrawer.resolution.Y / divider;
                laserDrawer.log("Resolution: " + resX + "x" + resY);
                labelResolution.Text = resX + "x" + resY;
                textBoxBeginFromLine.Text = "0";
                labelProgress.Text = "0%";
                labelTimeRemain.Text = "Осталось: неизвестно";
                Application.DoEvents();
                Bitmap toPrint = laserDrawer.centerScalablePanel.generateImage(resX, resY);
                pictureBoxPreview.Image = image = toPrint;
                pictureBoxPreview.Location = new Point(0, 0);
                pictureBoxPreview.Size = toPrint.Size;
                buttonPrint.Enabled = true;
            }
            catch (Exception) { }
        }
        private void buttonPrint_Click(object sender, EventArgs e)
        {
            if (image != null)
            {
                laserDrawer.log("Preparing for printimg...");
                cont = true;
                verticalOptimisation = checkBoxVerticalOptimisation.Checked;
                burnTime = Int32.Parse(textBoxBurnTime.Text);
                beginFrom = Int32.Parse(textBoxBeginFromLine.Text);
                laserDrawer.log("Burn Time = " + burnTime);
                laserDrawer.log("begin From = " + beginFrom);
                buttonPrint.Enabled = false;
                buttonGenerate.Enabled = false;
                buttonMakeBW.Enabled = false;
                buttonStop.Enabled = true;
                laserDrawer.showModal();

                new Thread(print).Start();
            }
        }
        private void printvertical()
        {
            //анализ целесообразности печати некоторых строчек вертикально
            int avg = 0;
            int min = 255;
            int max = 0;
            int[] avgs = new int[image.Width];
            {   //вычислить средний
                long tsum = 0;
                long tcnt = 0;
                for (int x = 0; x < image.Width; x++)
                {
                    long sum = 0;
                    long cnt = 0;
                    for (int y = 0; y < image.Height; y++)
                    {
                        Color color = image.GetPixel(x, y);
                        int intensity = (color.R + color.G + color.B)/3; //0...255
                        sum += intensity;
                        cnt++;
                    }
                    avgs[x] = (int)(sum / cnt);
                    tsum += avgs[x];
                    tcnt ++;
                    if (avgs[x] > max)
                        max = avgs[x];
                    if (avgs[x] < min)
                        min = avgs[x];
                }
                avg = (int)(tsum / tcnt);
            }
            avg = (min + max)/2;
            int threshold = avg - ((avg - min) / 2);

            for (int x = 0; x < image.Width; x++)
            {
                if (avgs[x] < threshold)
                {
                    printVerticalLine(x);
                }
                inv(0);
            }
            //finish();
        }
        private void print()
        {
            try
            {
                if(verticalOptimisation)
                    printvertical();
                for (int y = beginFrom; y < image.Height && cont; y++)
                {
                    printHorizontalLine(y);
                    //обновить экран
                    calulateTime(y);
                    inv(y);
                }
                laserDrawer.executeBuffer();
                laserDrawer.sendCommand("release");
                laserDrawer.log("Печать завершена!");
                finish();
            }
            catch (Exception exc)
            {
                laserDrawer.log("Error while printing: " + exc.ToString());
            }
        }
        private bool horizontalFirst = true;
        private void printHorizontalLine(int y)
        {
            bool posY = false;
            int begin = -1;
            int end = -1;
            int lastBurnTime = -1;
            for (int x = 0; x < image.Width && cont; x++)
            {
                Color color = image.GetPixel(x, y);
                int intensity = 255 - (color.R + color.G + color.B) / 3; //0...255
                int showIntensity = 255;//Math.Max(0, 255 - intensity - 30);
                image.SetPixel(x, y, Color.FromArgb(255, showIntensity, showIntensity, showIntensity));

                if (intensity > 30)
                {
                    int burningTime = (burnTime * intensity) / 255;
                    int Xcoord = x * steps;
                    int Ycoord = y * steps;
                    //сделать 20 шагов к цели, чтобы компенчсировать смещение
                    if (horizontalFirst)
                    {
                        horizontalFirst = false;
                        for (int i = Ycoord - 30 * steps; i < Ycoord; i += steps)
                        {
                            Thread.Sleep(500);
                            laserDrawer.sendCommand("gy" + i);
                        }
                    }
                    //переместиться на У, если в этом ряду ещё не было команд
                    if (!posY)
                    {
                        posY = true;
                        laserDrawer.sendCommandBuffered("xhold");
                        laserDrawer.sendCommandBuffered("gy" + Ycoord);
                        laserDrawer.sendCommandBuffered("yhold");
                    }
                    //дополнить строку, если она такая же как и предыдущая
                    if (lastBurnTime == burningTime)
                        end = Xcoord;
                    else
                    //выполнить линию, если интенсивность изменилась
                    {
                        if (lastBurnTime != -1)
                            laserDrawer.sendCommandBuffered("bx" + lastBurnTime + "." + begin + (end == begin ? "" : ("." + end)));
                        begin = Xcoord;
                        end = Xcoord;
                        lastBurnTime = burningTime;
                    }
                }
                else
                //выполнить команду, если картинка в этом ряду закончилась
                {
                    if (lastBurnTime != -1)
                        laserDrawer.sendCommandBuffered("bx" + lastBurnTime + "." + begin + (end == begin ? "" : ("." + end)));
                    lastBurnTime = -1;
                }
            }
            //выполнить очередь в этом ряду, пока не перешли на следующий
            if (lastBurnTime != -1)
                laserDrawer.sendCommandBuffered("bx" + lastBurnTime + "." + begin + (end == begin ? "" : ("." + end)));
        }
        private bool verticalFirst = true;
        private void printVerticalLine(int x)
        {
            bool posX = false;
            int begin = -1;
            int end = -1;
            int lastBurnTime = -1;
            for (int y = 0; y < image.Height && cont; y++)
            {
                Color color = image.GetPixel(x, y);
                int intensity = 255 - (color.R + color.G + color.B) / 3; //0...255
                int showIntensity = 255;
                image.SetPixel(x, y, Color.FromArgb(255, showIntensity, showIntensity, showIntensity));

                if (intensity > 30)
                {
                    int burningTime = (burnTime * intensity) / 255;
                    int Xcoord = x * steps;
                    int Ycoord = y * steps;
                    //сделать 20 шагов к цели, чтобы компенчсировать смещение
                    if (verticalFirst)
                    {
                        verticalFirst = false;
                        for (int i = Xcoord - 30 * steps; i < Xcoord; i += steps)
                        {
                            Thread.Sleep(500);
                            laserDrawer.sendCommand("gx" + i);
                        }
                    }
                    //переместиться на X, если в этом ряду ещё не было команд
                    if (!posX)
                    {
                        posX = true;
                        laserDrawer.sendCommandBuffered("yhold");
                        laserDrawer.sendCommandBuffered("gx" + Xcoord);
                        laserDrawer.sendCommandBuffered("xhold");
                    }
                    //дополнить строку, если она такая же как и предыдущая
                    if (lastBurnTime == burningTime)
                        end = Ycoord;
                    else
                    //выполнить линию, если интенсивность изменилась
                    {
                        if (lastBurnTime != -1)
                            laserDrawer.sendCommandBuffered("by" + lastBurnTime + "." + begin + (end == begin ? "" : ("." + end)));
                        begin = Ycoord;
                        end = Ycoord;
                        lastBurnTime = burningTime;
                    }
                }
                else
                //выполнить команду, если картинка в этом ряду закончилась
                {
                    if (lastBurnTime != -1)
                        laserDrawer.sendCommandBuffered("by" + lastBurnTime + "." + begin + (end == begin ? "" : ("." + end)));
                    lastBurnTime = -1;
                }
            }
            //выполнить очередь в этом ряду, пока не перешли на следующий
            if (lastBurnTime != -1)
                laserDrawer.sendCommandBuffered("by" + lastBurnTime + "." + begin + (end == begin ? "" : ("." + end)));
        }
        private void finish()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { finish(); });
                return;
            }

            buttonPrint.Enabled = true;
            buttonGenerate.Enabled = true;
            buttonMakeBW.Enabled = true;
            buttonStop.Enabled = false;
            laserDrawer.hideModal();
        }
        private void inv(int line)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { inv(line); });
                return;
            }
            // this code will run on main (UI) thread 
            textBoxBeginFromLine.Text = line.ToString();
            pictureBoxPreview.Invalidate();
            Application.DoEvents();
        }
        private void buttonStop_Click(object sender, EventArgs e)
        {
            //laserDrawer.sendString("stop");
            buttonPrint.Enabled = true;
            buttonGenerate.Enabled = true;
            buttonMakeBW.Enabled = true;
            buttonStop.Enabled = false;
            laserDrawer.hideModal();
            cont = false;
        }
        private void buttonMakeBW_Click(object sender, EventArgs e)
        {
            laserDrawer.showModal();
            int min = 255;
            int max = 0;
            for (int y = 0; y < image.Height; y++)
                for (int x = 0; x < image.Width; x++)
                {
                    Color color = image.GetPixel(x, y);
                    int intensity = (color.R + color.G + color.B) / 3; //0...255
                    if (intensity < min)
                        min = intensity;
                    if (intensity > max)
                        max = intensity;
                }
            long avg = (min+max)/2;
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color color = image.GetPixel(x, y);
                    int intensity = (color.R + color.G + color.B) / 3; //0...255
                    if (intensity > avg)
                        image.SetPixel(x, y, Color.FromArgb(255, 255, 255, 255));
                    else
                        image.SetPixel(x, y, Color.FromArgb(255, 0, 0, 0));
                }
                if(y%10 == 0)
                    inv(0);
            }
            inv(0);
            laserDrawer.hideModal();
        }

        private bool firstTimeCalculating = true;
        private long totalPointsSize = 0;
        private long printBeginTime = 0;
        private long msPerPoint = 30;
        private void calulateTime(int fy)
        {
            long nowTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long nowPoints = calculatePointsFromLine(fy);
            if (firstTimeCalculating)
            {
                firstTimeCalculating = false;
                totalPointsSize = nowPoints;
                printBeginTime = nowTime;
                setProgress("0%");
            }
            else
            {
                long percent = (nowPoints*100)/totalPointsSize;
                percent = 100 - percent;
                setProgress(percent + "%");

                if (percent != 0)
                {
                    long dtime = nowTime - printBeginTime;
                    long percentTime = dtime/percent;
                    long msRemain = percentTime * (100 - percent);
                    long minutesRemain = msRemain / 60000;
                    setTimeRemaining("Осталось: " + minutesRemain + " минут.");
                }
            }
        }
        private void setProgress(String text)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { setProgress(text); });
                return;
            }
            // this code will run on main (UI) thread 
            labelProgress.Text = text;
            Application.DoEvents();
        }
        private void setTimeRemaining(String text)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { setTimeRemaining(text); });
                return;
            }
            // this code will run on main (UI) thread 
            labelTimeRemain.Text = text;
            Application.DoEvents();
        }
        private long[] pointCalculationCacle = null;
        private long calculatePointsFromLine(int fy)
        {
            if (pointCalculationCacle == null)
            {
                pointCalculationCacle = new long[image.Height];
                for (int y = 0; y < image.Height; y++)
                    pointCalculationCacle[y] = -1;
            }

            long cnt = 0;
            for (int y = fy; y < image.Height; y++)
            {
                long lineCnt = 0;
                if (pointCalculationCacle[y] == -1)
                {
                    int first = -1;
                    int last = -1;
                    for (int x = 0; x < image.Width; x++)
                    {
                        Color color = image.GetPixel(x, y);
                        int intensity = (color.R + color.G + color.B)/3; //0...255
                        if (intensity < 20)
                        {
                            if (first == -1)
                                first = x;
                            last = x;
                        }
                    }
                    pointCalculationCacle[y] = lineCnt = last - first;
                }
                else
                {
                    lineCnt = pointCalculationCacle[y];
                }
                cnt += lineCnt;
            }
            return cnt;
        }
    }
}
