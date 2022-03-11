using System;
using System.Windows.Forms;

namespace LaserDrawerApp
{
    public partial class LogWindow : Form
    {
        MainWindow mainWindow = null;
        public LogWindow(MainWindow _mainWindow)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
        }

        private void LogWindow_Load(object sender, EventArgs e)
        {
            if (mainWindow != null)
            {
                label1.Text = mainWindow.logText;
                mainWindow.logLabel = label1;
            }
        }

        private void LogWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            mainWindow.logLabel = null;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(label1.Text);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(label1.Text);
        }
    }
}
