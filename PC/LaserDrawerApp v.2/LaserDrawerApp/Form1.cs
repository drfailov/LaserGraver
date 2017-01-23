using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LaserDrawerApp
{
    public partial class Form1 : Form
    {
        SerialPort port = null;
        int maxX = 0;
        int maxY = 0;
        Bitmap bitmap = null;
        bool printing = false;


        public Form1()
        {
            InitializeComponent();
            AllocConsole();
        }

        private void button1_Click(object sender, EventArgs e)//load COMs
        {
            string[] COMs =  SerialPort.GetPortNames();
            foreach (string com in COMs)
            {
                comboBoxCOMs.Items.Add(com);
            }
            comboBoxCOMs.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)//connect
        {
            port = new SerialPort((String)comboBoxCOMs.SelectedItem);
            port.Open();

            //Thread thread = new Thread(waitText);
            //thread.Start();
        }

        private void waitText()
        {
            while(true){
                String data =  port.ReadLine();
                lineReceived(data);
            }
        }

        String lastString = "";
        private void lineReceived(String line)
        {
            writeReceived(line);
        }
        private void button3_Click(object sender, EventArgs e)//send
        {
            sendLine(textBoxToSend.Text);
        }


        private void writeReceived(String text)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { writeReceived(text); });
                return;
            }
            // this code will run on main (UI) thread 
            labelReceived.Text = text;
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
        } 

        private String getLine()
        {
            if (port != null)
            {
                String text = port.ReadLine();
                Console.WriteLine("<=== " + text);
                return text;
            }
            return "";
        }
        private void sendLine(String text)
        {
            Console.WriteLine("===> " + text);
            if (port != null)
            {
                port.WriteLine(text);
            }
        }

        private void reload()
        {
            writeJob("Restarting...");
            writeReceived("Sending command...");
            sendLine("reload");
            while (true)
            {
                String got = getLine();
                writeReceived(got);
                if (got.Contains("Done."))
                {
                    writeJob("Complete.");
                    break;
                }
            }
        }
        private void GetResolution()
        {
            writeJob("Geting resolution...");
            writeReceived("Sending command...");
            sendLine("resolution");
            String got = getLine();
            writeReceived(got);
            String[] parts = got.Split(';');
            if (parts.Length == 2)
            {
                maxX = Int32.Parse(parts[0]);
                maxY = Int32.Parse(parts[1]);
            }
            got = getLine();
            writeReceived(got);
            if (got.Contains("Done."))
                writeJob("Complete.");
            pictureBox1.Width = maxX;
            pictureBox1.Height = maxY;
        }
        private void print()
        {
            writeJob("Printing...");
            writeReceived("Sending command...");
            printing = true;
            sendLine("print");
            String got;
            while (printing) { 
                got = getLine();
                writeReceived(got);
                String[] parts = got.Split(';');
                if (parts.Length == 3)
                {
                    if (parts[0].Equals("GET"))
                    {
                        int x = Int32.Parse(parts[1]);
                        int y = Int32.Parse(parts[2]);

                        int r = bitmap.GetPixel(x, y).R;
                        int g = bitmap.GetPixel(x, y).G;
                        int b = bitmap.GetPixel(x, y).B;
                        int px = (r + g + b) / 3;
                        if (px < 128)
                            sendLine("1");
                        else
                            sendLine("0");

                        markDot(x, y);

                        if (x >= maxX - 1 && y >= maxY - 1)
                        {
                            break;
                        }
                    }
                }
                Application.DoEvents();
            }

            do{
                got = getLine();
                writeReceived(got);
            }while(!got.Contains("Done."));
            writeJob("Complete.");
        }

        private void buttonReload_Click(object sender, EventArgs e)
        {
            reload();
        }

        private void buttonResolution_Click(object sender, EventArgs e)
        {
            GetResolution();
        }

        private void buttonBMP_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "Image Files|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    Bitmap bit = new Bitmap(open.FileName);
                    bitmap = new Bitmap(resizeImage(bit, new Size(maxX, maxY)));
                    pictureBox1.Image = bitmap;
                }
            }
            catch (Exception)
            {
                throw new ApplicationException("Failed loading image");
            }
        }
        public Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }

        private void buttonPrint_Click(object sender, EventArgs e)
        {
            print();
        }


        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        private void buttonStop_Click(object sender, EventArgs e)
        {
            printing = false;
            sendLine("stop");
        }
        Graphics graphics = null;
        private void markDot(int x, int y)
        {
            //if (graphics == null)
            //{
            //    graphics = Graphics.FromHwnd(pictureBox1.Handle);
            //}
            bitmap.SetPixel(x, y, Color.Green);
            pictureBox1.Image = bitmap;
            Application.DoEvents();
            //graphics.Draw(new Pen(Color.FromArgb(100, 0, 255, 0)), x, y, x, y);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int cnt = 100;
            while (cnt-- > 0)
            {
                markDot(cnt, 20);
                Application.DoEvents();
            }
        }
    }
}
