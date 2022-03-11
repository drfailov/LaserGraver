using System;
using System.Drawing;
using System.Windows.Forms;

namespace LaserDrawerApp
{
    class StatusPictureBox : PictureBox
    {
        public PointF markPoint = new PointF(-1, -1);
        private Pen linePen = new Pen(Color.FromArgb(100, 255, 0, 0), 1);
        private Pen ellipsePen = new Pen(Color.DarkRed, 1);

        public StatusPictureBox()
        {

        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (Image != null)
            {
                float coefX = (float)Width / Image.Width;
                float coefY = (float)Height / Image.Height;
                float coef = Math.Min(coefX, coefY);
                SizeF imageSize = new SizeF(Image.Width * coef, Image.Height * coef);
                PointF center = new PointF(Width / 2, Height / 2);
                PointF imagePosition = new PointF(center.X - imageSize.Width / 2, center.Y - imageSize.Height / 2);
                float pointX = imagePosition.X + imageSize.Width * markPoint.X;
                float pointY = imagePosition.Y + imageSize.Height * markPoint.Y;

                if (markPoint.X != -1)
                    e.Graphics.DrawLine(linePen, pointX, 0, pointX, Height - 1);
                if (markPoint.Y != -1)
                    e.Graphics.DrawLine(linePen, 0, pointY, Width - 1, pointY);
                if (markPoint.X != -1 && markPoint.Y != -1)
                    e.Graphics.DrawEllipse(ellipsePen, pointX - 10, pointY - 10, 20, 20);
            }
        }
    }
}
