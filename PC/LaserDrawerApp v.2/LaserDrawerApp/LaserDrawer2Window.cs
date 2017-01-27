using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LaserDrawerApp
{
    public partial class LaserDrawer2Window : Form
    {
        // ReSharper disable All
        //Ctrl+M, O
        public Point resolution = new Point(-1, -1);
        public CenterScalablePanel centerScalablePanel = null;
        private WaitingWindow waitingWindow = null;
        private SerialPort port = null;
        private bool paused = false;

        public LaserDrawer2Window()
        {
            InitializeComponent();
        }
        private void LaserDrawer2Window_Load(object sender, EventArgs e)
        {
            refreshCOMs();
        }
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
            if (comboBoxComPorts.Items.Count > 0)
                comboBoxComPorts.SelectedIndex = sel;
            writeLog("");
            writeLog("COM port list loaded.");
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
            if (textBoxLog.Text.Length > 20000)
                textBoxLog.Text = "";
            textBoxLog.AppendText(text + "\n");//Text += (text + "\n");
            //if (textBoxLog.Text.Length > 4000)
            //    textBoxLog.Text = textBoxLog.Text.Substring(0, 3000);
            textBoxLog.ScrollToCaret();
            Application.DoEvents();
        }
        public String log(String text)
        {
            writeLog(text);
            return text;
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
        private void initImage()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { initImage(); });
                return;
            }
            log("Инициализация изображения...");
            if (centerScalablePanel == null)
            {
                float maxCSize = Math.Min(panelContainer.Width, panelContainer.Height);
                float maxISize = Math.Max(resolution.X, resolution.Y);
                float coef = maxCSize/maxISize;

                centerScalablePanel = new CenterScalablePanel();
                panelContainer.Controls.Clear();
                panelContainer.Controls.Add(centerScalablePanel);
                centerScalablePanel.Size = new Size((int) (resolution.X*coef), (int) (resolution.Y*coef));

            }
            Application.DoEvents();
        }
        public void showModal()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { showModal(); });
                return;
            }
            hideModal();
            foreach (Control control in panelConnector.Controls)
                control.Enabled = false;
            foreach (Control control in panelFunctions.Controls)
                control.Enabled = false;
            //panelConnector.Visible = false;
            //panelFunctions.Visible = false;

            waitingWindow = new WaitingWindow();
            waitingWindow.StartPosition = FormStartPosition.CenterParent;
            waitingWindow.Show();
            waitingWindow.TopMost = true;
            Point p = new Point(this.Location.X + (this.Width / 2 - waitingWindow.Width / 2), this.Location.Y + (this.Height / 2 - waitingWindow.Height / 2));
            waitingWindow.Location = p;
            Application.DoEvents();
        }
        public void hideModal()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { hideModal(); });
                return;
            }
            foreach (Control control in panelConnector.Controls)
                control.Enabled = true;
            foreach (Control control in panelFunctions.Controls)
                control.Enabled = true;
//            panelConnector.Visible = true;
//            panelFunctions.Visible = true;
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
        void setScale(float coef)
        {
            if (coef > 0)
                centerScalablePanel.Size = new Size((int)((float)resolution.X * coef), (int)((float)resolution.Y * coef));
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
        public void showMark(float xcoef, float ycoef)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { showMark(xcoef, ycoef); });
                return;
            }
            centerScalablePanel.markPoint.X = xcoef;
            centerScalablePanel.markPoint.Y = ycoef;
            centerScalablePanel.Invalidate();
            Application.DoEvents();
        }


        private void connect()
        {
            if (port != null)
            {
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
                sendString("connect");
                waitFor(doneMark());
                Thread.Sleep(1000);
                getResolution();
                writeJob("Подключено.");
                makeJobGreen();
            }
            catch (Exception e)
            {
                log("Ошибка: " + e.ToString());
                port = null;
                writeJob("Ошибка подключения");
                makeJobRed();
            }
            hideModal();
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
                    makeJobRed();
                }
            }
        }
        public String receiveString()
        {
            //todo ДОБАВИТЬ КЭШ С ВОЗМОЖНОСТЬЮ УЧЕТА РАЗМЕРА ОЧЕРЕДИ
            if (port != null)
            {
                String text = port.ReadLine();
                writeLog("|#| --> " + text);
                return text;
            }
            writeLog("|X| -x- ...");
            return "";
        }
        public void sendString(String text)
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
        public void waitFor(String text)
        {
            if (port == null)
                return;
            String received;
            do
            {
                received = receiveString();
                Application.DoEvents();
            } while (!received.Contains(text));
            clearBuffers();
        }
        private String doneMark()
        {
            return "---------------";
        }
        private void clearBuffers()
        {
            port.DiscardInBuffer();
            port.DiscardOutBuffer();
        }
        public void sendCommand(String text)
        {
            //if (waitingWindow == null)
            {
                //showModal();
                //writeJob("Выполнение " + text + " ...");
                sendString(text);
                waitFor(doneMark());
                //writeJob("Готово.");
                //hideModal();
            }
        }
        public void executeCommand(String text)
        {
            if (waitingWindow == null)
            {
                showModal();
                log("Выполнение " + text + " ...");
                sendString(text);
                waitFor(doneMark());
                log("Готово.");
                hideModal();
            }
        }
        private String commandsBuffer = "";
        public void sendCommandBuffered(String text)
        {
            if (commandsBuffer.Length + text.Length < 100)//240
                commandsBuffer += text + "|";
            else
            {
                sendCommand(commandsBuffer + "xhold");
                commandsBuffer = text + "|";
            }
        }
        public void executeBuffer()
        {
            sendCommand(commandsBuffer + "xhold");
            commandsBuffer = "";
        }
        private void getResolution()
        {
            showModal();
            log("Получение разрешения...");
            sendString("getxresolution");
            String input;
            do
            {
                input = receiveString();
                if (input.Contains("X resolution = "))
                    log("Новое значение разрешения Х: " + (resolution.X = Int32.Parse(input.Replace("X resolution = ", ""))));
            } while (!input.Contains(doneMark()));

            sendString("getyresolution");
            do
            {
                input = receiveString();
                if (input.Contains("Y resolution = "))
                    log("Новое значение разрешения Х: " + (resolution.Y = Int32.Parse(input.Replace("Y resolution = ", ""))));
            } while (!input.Contains(doneMark()));

            writeResolution(resolution.X + " x " + resolution.Y);
            hideModal();
            initImage();
        }
        private void getCurrentPoint()
        {
            showModal();
            log("Получение координаты...");
            sendString("getxy");
            String input;
            float x = 0;
            float y = 0;
            do
            {
                input = receiveString();
                if (input.Contains("X = "))
                    log("Положение Х: " + (x = Int32.Parse(input.Replace("X = ", ""))));
                if (input.Contains("Y = "))
                    log("Положение Y: " + (y = Int32.Parse(input.Replace("Y = ", ""))));
            } while (!input.Contains(doneMark()));

            float xcoef = x / (float)resolution.X;
            float ycoef = y / (float)resolution.Y;

            showMark(xcoef, ycoef);
            hideModal();
            initImage();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            refreshCOMs();
        }
        private void buttonConnect_Click(object sender, EventArgs e)
        {
            new Thread(connect).Start();
        }
        private void buttonAddImage_Click(object sender, EventArgs e)
        {
            if (centerScalablePanel != null)
            {

                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "Изображения(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    Bitmap file = new Bitmap(open.FileName);

                    ImageFloatingControl sizeable = new ImageFloatingControl(file);
                    sizeable.Location = new Point(10, 10);
                    sizeable.Size = new System.Drawing.Size(100, 80);
                    centerScalablePanel.Controls.Add(sizeable);
                    sizeable.BringToFront();
                }
            }
        }
        private void buttonAddText_Click(object sender, EventArgs e)
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
        private void buttonPlus_Click(object sender, EventArgs e)
        {
            setScale(getScale() + 0.003f);
        }
        private void buttonMinus_Click(object sender, EventArgs e)
        {
            float scale = getScale() - 0.003f;
            if(scale > 0)
                setScale(scale);
        }
        private void buttonAddEmpty_Click(object sender, EventArgs e)
        {
            if (centerScalablePanel != null)
            {
                FloatingControl sizeable = new FloatingControl();
                sizeable.Location = new Point(10, 10);
                sizeable.Size = new System.Drawing.Size(100, 80);
                centerScalablePanel.Controls.Add(sizeable);
                sizeable.BringToFront();
            }
        }
        private void LaserDrawer2Window_Resize(object sender, EventArgs e)
        {
            if (centerScalablePanel != null)
            {
                float fit = fitScale();
                if (getScale() < fit*2)
                    setScale(fit);
            }
        }
        private void buttonFit_Click(object sender, EventArgs e)
        {
            setScale(fitScale());
        }
        private void buttonContinue_Click(object sender, EventArgs e)
        {
            ContinueForm continueForm = new ContinueForm(this);
            continueForm.Show();
        }
        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            new Thread(disconnect).Start();
        }
        private void buttonStop_Click(object sender, EventArgs e)
        {
            new Thread(() => sendString("stop")).Start();
        }
        private void buttonHome_Click(object sender, EventArgs e)
        {
            new Thread(() => executeCommand("home")).Start();
        }
        private void buttonXmin_Click(object sender, EventArgs e)
        {
            new Thread(() => executeCommand("xmin")).Start();
        }
        private void buttonXtest_Click(object sender, EventArgs e)
        {
            new Thread(() => executeCommand("xtest")).Start();
        }
        private void buttonYtest_Click(object sender, EventArgs e)
        {
            new Thread(() => executeCommand("ytest")).Start();
        }
        private void buttonCenter_Click(object sender, EventArgs e)
        {
            new Thread(() => executeCommand("center|xhold|yhold")).Start();
        }
        private void buttonYmin_Click(object sender, EventArgs e)
        {
            new Thread(() => executeCommand("ymin")).Start();
        }
        private void buttonXmax_Click(object sender, EventArgs e)
        {
            new Thread(() => executeCommand("xmax")).Start();
        }
        private void buttonYMinus_Click(object sender, EventArgs e)
        {
            new Thread(() => executeCommand("yshift -10|yhold")).Start();
        }
        private void buttonXminus_Click(object sender, EventArgs e)
        {
            new Thread(() => executeCommand("xshift -10|xhold")).Start();
        }
        private void buttonXplus_Click(object sender, EventArgs e)
        {
            new Thread(() => executeCommand("xshift 10|xhold")).Start();
        }
        private void buttonYplus_Click(object sender, EventArgs e)
        {
            new Thread(() => executeCommand("yshift 10|yhold")).Start();
        }
        private void buttonYmax_Click(object sender, EventArgs e)
        {
            new Thread(() => executeCommand("ymax")).Start();
        }
        private void buttonXYtest_Click(object sender, EventArgs e)
        {
            new Thread(() => executeCommand("xytest")).Start();
        }
        private void buttonXYhold_Click(object sender, EventArgs e)
        {
            new Thread(() => executeCommand("xhold|yhold")).Start();
        }
        private void buttonXYrelease_Click(object sender, EventArgs e)
        {
            new Thread(() => executeCommand("release")).Start();
        }
        private void buttonLaserOn_Click(object sender, EventArgs e)
        {
            new Thread(() => executeCommand("laseron")).Start();
        }
        private void buttonLaserOff_Click(object sender, EventArgs e)
        {
            new Thread(() => executeCommand("laseroff")).Start();
        }
        private void buttonLaserTest_Click(object sender, EventArgs e)
        {
            new Thread(() => executeCommand("lasertest")).Start();
        }
        private void buttonGetXY_Click(object sender, EventArgs e)
        {
            new Thread(getCurrentPoint).Start();
        }
        private void textBoxCommand_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Return))
            {
                buttonSendCommandFromFiels_Click(null, null);
            }
        }

        private void buttonSendCommandFromFiels_Click(object sender, EventArgs e)
        {
            if (!textBoxCommand.Text.Equals(""))
            {
                executeCommand(textBoxCommand.Text);
                textBoxCommand.Text = "";
                textBoxCommand.Focus();
            }
        }
    }
}
