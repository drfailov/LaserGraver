using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaserDrawerApp
{
    public partial class WriteText : Form
    {
        PrinterWindow printerWindow = null;
        Bitmap bitmap = null;
        Graphics graphics = null;

        int x = 0;
        int y = 0;


        public WriteText(PrinterWindow p)
        {
            InitializeComponent();
            printerWindow = p;
        }

        private void WriteText_Load(object sender, EventArgs e)
        {
            fillFonts();
            fillSizes();
            redraw();
        }
        private void fillFonts()
        {
            comboBoxFont.Items.Clear();
            foreach (FontFamily font in System.Drawing.FontFamily.Families)
                comboBoxFont.Items.Add(font.Name);
            comboBoxFont.SelectedIndex = 0;
            createNontsPreview();
        }

        private void createNontsPreview()
        {
            panelFonts.Controls.Clear();
            int previewHeight = 60;
            int previewWidth = panelFonts.Width - 40;
            int previewGap = 5;
            int x = 10;
            int y = 10;

            foreach (String name in comboBoxFont.Items)
            {
                PictureBox pictureBox = new PictureBox();
                pictureBox.Location = new System.Drawing.Point(x, y);
                pictureBox.Name = name;
                pictureBox.Size = new System.Drawing.Size(previewWidth, previewHeight);

                Bitmap fontPreview = new Bitmap(previewWidth, previewHeight);
                Graphics graph = Graphics.FromImage(fontPreview);
                graph.FillRectangle(new SolidBrush(Color.White), 0, 0, previewWidth, previewHeight);

                int fontSize = 10;
                String text = textBoxText.Text;
                FontFamily fontFamily = new FontFamily(name);
                FontStyle fontStyle = FontStyle.Regular;
                if (!fontFamily.IsStyleAvailable(fontStyle))
                    fontStyle = FontStyle.Italic;
                if (!fontFamily.IsStyleAvailable(fontStyle))
                    fontStyle = FontStyle.Bold;
                Font font = new Font(name, fontSize, fontStyle);
                graph.DrawString("Test String\nПример шрифта\n" + name, font, new SolidBrush(Color.Black), 3, 3);
                pictureBox.Image = fontPreview;
                pictureBox.Click += new System.EventHandler(this.previewClick);
                panelFonts.Controls.Add(pictureBox);

                y += previewHeight + previewGap;
            }
        }
        private void fillSizes()
        {
            comboBoxSize.Items.Clear();
            for (int i = 6; i < 50; i++)
                comboBoxSize.Items.Add(i);
            comboBoxSize.SelectedIndex = 0;
        }

        private void previewClick(object sender, EventArgs e)
        {
            comboBoxFont.SelectedItem = ((PictureBox)sender).Name;
        }

        private void redraw()
        {
            if (bitmap == null)
            {
                bitmap = new Bitmap(printerWindow.resolution.X, printerWindow.resolution.Y);
                log("Init bitmap...");
            }
            if (graphics == null)
            {
                graphics = Graphics.FromImage(bitmap);
                log("Init graphics...");
            }
            if (checkBoxAntialiasing.Checked)
                graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            else
                graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
            int fontSize = Int32.Parse(comboBoxSize.SelectedItem.ToString());
            String fontName = comboBoxFont.SelectedItem.ToString();
            String text = textBoxText.Text;
            FontFamily fontFamily = new FontFamily(fontName);
            FontStyle fontStyle = FontStyle.Regular;
            if (!fontFamily.IsStyleAvailable(fontStyle))
                fontStyle = FontStyle.Italic;
            if (!fontFamily.IsStyleAvailable(fontStyle))
                fontStyle = FontStyle.Bold;
            Font font = new Font(fontName, fontSize, fontStyle);

            graphics.FillRectangle(new SolidBrush(Color.White), 0, 0, printerWindow.resolution.X, printerWindow.resolution.Y);
            graphics.DrawString(text, font, new SolidBrush(Color.Black), x, y);
            printerWindow.showImage(bitmap);
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            printerWindow.image = bitmap;
            redraw();
        }
        private void log(String text)
        {
            printerWindow.log(text);
        }
    }
}
