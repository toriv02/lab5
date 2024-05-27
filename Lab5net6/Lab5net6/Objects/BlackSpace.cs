using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5net6.Objects
{
    internal class BlackSpace:BaseObjects
    {
        public float vX;//скорость области 
        public float Width, Height;//размер области
        public BlackSpace(float x, float y, float angle) : base(x, y, angle) { }
        public override void Render(Graphics canvas,bool flag=true)
        {
            canvas.FillRectangle(new SolidBrush(Color.Black), -Width, -Height, Width, Height);
           
        }
        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-Width, -Height, Width, Height);
            return path;
            
        }
    }
}
