using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace LaserDrawerApp
{
    class FloatingControl : Panel
    {
        // ReSharper disable All
        Pen borderPen = null;
        SolidBrush ellipseBrush = null;
        protected ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
        int circleRadius = 4;
        bool pressed = false;
        int lx = 0, ly = 0;
        private bool visible = true;
        private Color borderColor = Color.LightGray;
        private int borderSize = 1;

        public FloatingControl()
        {
            BackColor = Color.Transparent;
            contextMenuStrip.Items.Add("Расширить").MouseDown += fill;
            contextMenuStrip.Items.Add("Удалить").MouseDown += remove;
            contextMenuStrip.Items.Add("Показать\\Скрыть").MouseDown += showHide;
        }

        public void setBorderColor(Color color)
        {
            borderColor = color;
            ellipseBrush = null;
            borderPen = null;
        }

        public void setBorderSize(int _borderSize)
        {
            borderSize = _borderSize;
            //1 ... 4      2 .... 6        3 ... 8
            circleRadius = 2 + _borderSize * 2;
            borderPen = null;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (borderPen == null)
            {
                borderPen = new Pen(borderColor, borderSize);
                borderPen.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                ellipseBrush = new SolidBrush(borderColor);
            }
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            //data
            drawContent(e.Graphics, Width, Height);

            if (!visible)
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(200, Color.White)), 0, 0, Width - 1, Height - 1);

            //frame
            e.Graphics.DrawRectangle(borderPen, 0, 0, Width-1, Height-1);

            //mark
            e.Graphics.FillRectangle(ellipseBrush, resizeBottomRight());
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (resizeBottomRight().Contains(e.X, e.Y))
                Cursor.Current = Cursors.SizeNWSE;
            else
                Cursor.Current = Cursors.SizeAll;

            int x = Location.X + e.X, y = Location.Y + e.Y;
            if (pressed)
            {
                int dx = x - lx, dy = y - ly;
                if (resizeBottomRight().Contains(lx - Location.X, ly - Location.Y))
                {
                    int nsx = Size.Width + dx;
                    int nsy = Size.Height + dy;
                    if (checkSize(nsx, nsy))
                    {
                        Width = nsx;
                        Height = nsy;
                        Invalidate();
                    }
                }
                else
                {
                    int nlx = Location.X + dx;
                    int nly = Location.Y + dy;
                    if(checkLocation(nlx, nly))
                        Location = new Point(nlx, nly);
                }
            }
            lx = x;
            ly = y;
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if(e.Button == MouseButtons.Left)
                pressed = true;
            if (e.Button == MouseButtons.Right)
                showMenu();
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left)
            pressed = false;
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            Cursor.Current = Cursors.Arrow;
        }
        public virtual void drawData(Graphics g, float width, float height)
        {
            if(visible)
                drawContent(g, width, height);
        }
        protected virtual void drawContent(Graphics g, float width, float height)
        {
            //g.DrawLine(borderPen, 0, 0, width, height);
            //g.DrawLine(borderPen, 0, height, width, 0);
        }

        private Rectangle resizeBottomRight()
        {
            return new Rectangle(Width - borderSize  - circleRadius*2, Height - borderSize - circleRadius * 2, circleRadius * 2, circleRadius * 2);
        }
        private bool checkLocation(int x, int y)
        {
            return x >= 0
                && y >= 0
                && (x + Width) < Parent.Width
                && (y + Height) < Parent.Height;
        }
        private bool checkSize(int x, int y)
        {
            return x > circleRadius * 2
                && y > circleRadius * 2
                && (Location.X + x) < Parent.Width
                && (Location.Y + y) < Parent.Height;
        }
        private void showMenu(){
            contextMenuStrip.Show(Cursor.Position);
        }
        protected void remove(Object sender, MouseEventArgs e)
        {
            Parent.Controls.Remove(this);
        }
        private void fill(Object sender, MouseEventArgs e)
        {
            Location = new Point(0, 0);
            Size = new Size(Parent.Size.Width, Parent.Size.Height);
            Invalidate();
        }
        private void showHide(Object sender, MouseEventArgs e)
        {
            visible = !visible;
            Invalidate();
        }
    }
}
