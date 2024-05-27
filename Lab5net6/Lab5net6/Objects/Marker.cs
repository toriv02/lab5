using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5net6
{
    internal class Marker : BaseObjects
    {
        public Marker(float x, float y,float angle): base(x,y,angle) { }
        public override void Render(Graphics canvas, bool flag = true)
        {
            if (flag)
            {
                canvas.FillEllipse(new SolidBrush(Color.Red), -3, -3, 6, 6);
                canvas.DrawEllipse(new Pen(Color.Red, 2), -6, -6, 12, 12);
                canvas.DrawEllipse(new Pen(Color.Red, 2), -12, -12, 24, 24);
            }
            else
            {
                canvas.FillEllipse(new SolidBrush(Color.White), -3, -3, 6, 6);
                canvas.DrawEllipse(new Pen(Color.White, 2), -6, -6, 12, 12);
                canvas.DrawEllipse(new Pen(Color.White, 2), -12, -12, 24, 24);
            }
        }
        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-3, -3, 6, 6);
            return path;
        }
    }
}
