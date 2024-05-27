using Lab5net6.Objects;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5net6
{
    internal class Target : BaseObjects
    {
        private float R=70;//радиус первоночального круга
        public Target(float x, float y, float angle) : base(x, y, angle) { }//конструктор объекта
        public override void Render(Graphics canvas,bool flag=true)//отрисовка элемента
        {
            if (flag)//обычная отрисовка
            {
                canvas.FillEllipse(new SolidBrush(Color.LightGreen), -1*(R/2), -1 * (R / 2), R, R);
                canvas.DrawEllipse(new Pen(Color.Black, 2), -1 * (R / 2), -1 * (R / 2), R, R);
            }
            else//отрисовка обесцвеченного элемента
            {
                canvas.FillEllipse(new SolidBrush(Color.White), -1 * (R / 2), -1 * (R / 2), R, R);
                canvas.DrawEllipse(new Pen(Color.White, 2), -1 * (R / 2), -1 * (R / 2), R, R);
            }
        }
        public override GraphicsPath GetGraphicsPath()//добавляем рисунку GraphicPath для взаимодействия с другими объектами
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-1 * (R / 2), -1 * (R / 2), R, R);
            return path;
        }
        public override bool Exists() { return (R >-1) ? true : false; }//проверка существования
        public override void ChangeR() { R-=0.6f; }//скорость изменения размера кружка
    }
}
