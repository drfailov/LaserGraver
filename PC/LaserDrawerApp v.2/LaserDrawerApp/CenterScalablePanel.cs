using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace LaserDrawerApp
{
    public class CenterScalablePanel : Panel
    {
        // ReSharper disable All
        PointF lastSize = new PointF(-1, -1);
        public PointF markPoint = new PointF(-1, -1);
        Hashtable positions = new Hashtable();

        public CenterScalablePanel()
        {
            BackColor = Color.White;
        }
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            Parent.Resize += Parent_Resize;
        }
        void Parent_Resize(object sender, EventArgs e)
        {
            OnSizeChanged(e);
        }
        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            Control c = e.Control;
            positions.Add(c, new RectangleF(c.Location.X, c.Location.Y, c.Width, c.Height));
            c.Resize += c_Resize;
            c.Move += c_Move;
        }
        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            Control c = e.Control;
            positions.Remove(c);
            c.Resize -= c_Resize;
            c.Move -= c_Move;
        }
        void c_Move(object sender, EventArgs e)
        {
            Control c = (Control)sender;
            if (positions.Contains(c))
                positions.Remove(c);
            positions.Add(c, new RectangleF(c.Location.X, c.Location.Y, c.Width, c.Height));
        }
        void c_Resize(object sender, EventArgs e)
        {
            Control c = (Control)sender;
            if (positions.Contains(c))
                positions.Remove(c);
            positions.Add(c, new RectangleF(c.Location.X, c.Location.Y, c.Width, c.Height));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (markPoint.X != -1)
            {
                float pointX = Width * markPoint.X;
                float pointY = Height * markPoint.Y;
                e.Graphics.FillEllipse(new SolidBrush(Color.DarkRed), pointX - 2, pointY-2, 5, 5);
            }
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            float width = Width;
            float height = Height;
            //center
            int nx = (Parent.Width - Width) / 2;
            int ny = (Parent.Height - Height) / 2;
            if (nx < 0)
                nx = 0;
            if (ny < 0)
                ny = 0;
            Location = new Point(nx, ny);

            //inside
            foreach (Control control in Controls)
            {
                if (positions.Contains(control))
                {
                    PointF coef = new PointF(width / lastSize.X, height / lastSize.Y);

                    RectangleF oldLocation = (RectangleF)positions[control];
                    RectangleF newLocation = new RectangleF(oldLocation.X * coef.X, oldLocation.Y * coef.Y, oldLocation.Width * coef.X, oldLocation.Height * coef.Y);

                    control.Location = new Point((int)newLocation.X, (int)newLocation.Y);
                    control.Size = new Size((int)newLocation.Width, (int)newLocation.Height);
                    control.Invalidate();

                    positions.Remove(control);
                    positions.Add(control, newLocation);

                }
                else{
                    if (positions.Contains(control))
                        positions.Remove(control);
                    positions.Add(control, new RectangleF(control.Location.X, control.Location.Y, control.Width, control.Height));
                }
            }

            //last
            lastSize.X = width;
            lastSize.Y = height;
        }
        public Bitmap generateImage(int width, int height)
        {
            Size old = Size;
            Size = new Size(width, height);

            Bitmap bitmap = new Bitmap(width, height);
            Graphics graphics =  Graphics.FromImage(bitmap);
            graphics.FillRectangle(new SolidBrush(Color.White), 0, 0, width, height);
            foreach (FloatingControl control in Controls)
            {
                Bitmap controlImage = new Bitmap(control.Size.Width, control.Size.Height);
                Graphics controlGraphics = Graphics.FromImage(controlImage);
                control.drawData(controlGraphics, control.Size.Width, control.Size.Height);
                graphics.DrawImage(controlImage, control.Location);
            }


            Size = old;
            return bitmap;
        }
    }
}
