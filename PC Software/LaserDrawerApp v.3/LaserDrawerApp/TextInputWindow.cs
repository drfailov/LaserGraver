using System;
using System.Windows.Forms;

namespace LaserDrawerApp
{
    public partial class TextInputWindow : Form
    {
        public string text = "";

        public TextInputWindow()
        {
            InitializeComponent();
        }

        private void TextInputWindow_Load(object sender, EventArgs e)
        {

        }

        public void setText(string text)
        {
            this.text = text;
            textBox1.Text = text;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            text = textBox1.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                buttonOK_Click(sender, null);
        }
    }
}
