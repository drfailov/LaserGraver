using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Text;

namespace LaserDrawerApp
{
    class ImageFloatingControl : FloatingControl
    {
        Bitmap image = null;
        MainWindow mainWindow = null;

        public ImageFloatingControl(Bitmap i, MainWindow _mainWindow)
        {
            mainWindow = _mainWindow;
            image = i;
            contextMenuStrip.Items.Add("Масштабировать 1:1").MouseDown += scale100;
        }
        protected override void drawContent(Graphics g, float width, float height)
        {
            if (image != null) {
                float coef = Math.Min((float)width / (float)image.Width, (float)height / (float)image.Height);
                float niw = (float)image.Width * coef;
                float nih = (float)image.Height * coef;

                Rectangle drawingRect = new Rectangle(
                    (int)(width - niw) / 2,
                    (int)(height - nih) / 2,
                    (int)niw,
                    (int)nih
                );
                g.DrawImage(image, drawingRect);
            }
        }


        private void scale100(Object sender, MouseEventArgs e)
        {
            //image file size: image.Width, image.Height
            //canvas size: new Size(Parent.Size.Width, Parent.Size.Height);
            //priject size: mainWindow.resolution.X, mainWindow.resolution.Y 

            if (mainWindow == null)
                return;
            if (mainWindow.resolution == null)
                return;
            //какой процент проекта занимает файл изображения
            float coefX = (float)image.Width / (float)mainWindow.resolution.X;
            float coefY = (float)image.Height / (float)mainWindow.resolution.Y;
            float newSizeX = (float)Parent.Size.Width * coefX;
            float newSizeY = (float)Parent.Size.Height * coefY;
            //MessageBox.Show("s: " + newSizeX + "x" + newSizeY);
            Size = new Size((int)newSizeX, (int)newSizeY);
            Invalidate();
        }
        private void remove(Object sender, MouseEventArgs e) {
            base.remove(sender, e);
            image.Dispose();
        }

    }
}
