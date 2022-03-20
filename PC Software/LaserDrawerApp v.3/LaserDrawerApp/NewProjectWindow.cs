using System;
using System.Windows.Forms;

namespace LaserDrawerApp
{
    public partial class NewProjectWindow : Form
    {
        MainWindow mainWindow = null;
        int mx = 0;
        int my = 0;
        public NewProjectWindow(MainWindow mainWindow, int maxX, int maxY)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
            if (maxX != -1 && maxY != -1)
            {
                label1.Text = "The maximum size of the engraver's field: " + maxX + " x " + maxY + ". " +
                    "You can create a project of any size that is smaller than the working area. " +
                    "A smaller field will be processed faster before engraving." +
                    "The project will be engraved starting from zero coordinates.";
                numericUpDownWidth.Maximum = maxX;
                numericUpDownHeight.Maximum = maxY;
                numericUpDownWidth.Value = maxX;
                numericUpDownHeight.Value = maxY;
                mx = maxX;
                my = maxY;
            }
            else
            {
                button10.Visible = false;
                button25.Visible = false;
                button50.Visible = false;
                button75.Visible = false;
                button100.Visible = false;
            }
        }

        private void NewProjectWindow_Load(object sender, EventArgs e)
        {

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            mainWindow.newProject((int)numericUpDownWidth.Value, (int)numericUpDownHeight.Value);
            Close();
        }

        private void button100_Click(object sender, EventArgs e)
        {
            numericUpDownWidth.Value = mx;
            numericUpDownHeight.Value = my;
        }

        private void button75_Click(object sender, EventArgs e)
        {
            numericUpDownWidth.Value = (int)(mx * 0.75);
            numericUpDownHeight.Value = (int)(my * 0.75);
        }

        private void button50_Click(object sender, EventArgs e)
        {
            numericUpDownWidth.Value = (int)(mx * 0.50);
            numericUpDownHeight.Value = (int)(my * 0.50);
        }

        private void button25_Click(object sender, EventArgs e)
        {
            numericUpDownWidth.Value = (int)(mx * 0.25);
            numericUpDownHeight.Value = (int)(my * 0.25);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            numericUpDownWidth.Value = (int)(mx * 0.1);
            numericUpDownHeight.Value = (int)(my * 0.1);
        }
    }
}
