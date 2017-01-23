using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace LaserDrawerApp
{
    class ImageFloatingControl : FloatingControl
    {
        Image image = null;

        public ImageFloatingControl(Image i)
        {
            image = i;
        }
        protected override void drawContent(Graphics g, float width, float height)
        {
            if(image != null){
                float coef = Math.Min((float)width/(float)image.Width, (float)height/(float)image.Height);
                float niw = (float)image.Width*coef;
                float nih = (float)image.Height*coef;

                Rectangle drawingRect = new Rectangle(
                    (int)(width - niw)/2,
                    (int)(height - nih) / 2,
                    (int)niw,
                    (int)nih
                );
                g.DrawImage(image, drawingRect);
            }
        }
    }
}
