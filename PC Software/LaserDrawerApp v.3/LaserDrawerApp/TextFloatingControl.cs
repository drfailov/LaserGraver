using System;
using System.Drawing;
using System.Windows.Forms;

namespace LaserDrawerApp
{
    class TextFloatingControl : FloatingControl
    {
        string text = null;
        Font font = new Font("Arial", 14);

        public TextFloatingControl(string i)
        {
            text = i;
            contextMenuStrip.Items.Add("Edit text").MouseDown += selectText;
            contextMenuStrip.Items.Add("Select font").MouseDown += selectFont;
        }
        private void selectFont(object sender, MouseEventArgs e)
        {
            contextMenuStrip.Hide();
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = font;
            try
            {
                if (fontDialog.ShowDialog() == DialogResult.OK)
                {
                    font = fontDialog.Font;
                    Invalidate();
                }
            }
            catch (Exception) { }
        }
        private void selectText(object sender, MouseEventArgs e)
        {
            contextMenuStrip.Hide();
            TextInputWindow textInputWindow = new TextInputWindow();
            textInputWindow.setText(text);
            textInputWindow.ShowDialog();
            if (textInputWindow.DialogResult == DialogResult.OK)
            {
                text = textInputWindow.text;
                Invalidate();
            }
        }
        protected override void drawContent(Graphics g, float width, float height)
        {
            if (text != null)
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                font = new Font(font.FontFamily, Math.Max(Height, 3));
                while (g.MeasureString(text, font).Width > width && font.SizeInPoints > 1)
                    font = new Font(font.FontFamily, font.SizeInPoints - 1);
                SizeF futureSize = g.MeasureString(text, font);
                Brush brush = new SolidBrush(Color.Black);
                PointF position = new PointF((width - futureSize.Width) / 2, (height - futureSize.Height) / 2);
                g.DrawString(text, font, brush, position);
            }
        }
    }
}
