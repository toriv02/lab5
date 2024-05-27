using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5net6
{
    internal class Player : BaseObjects
    {
        public Action<Marker> OnMarkerOverlap;//событие при пересечнеии с маркером
        public Action<Target> OnTargetOverlap;//событие при пересечнеии с целью
        public float vX, vY;//скорость игрока

        public Player(float x,float y, float angle) : base(x,y,angle) { }
        public override void Render(Graphics canvas, bool flag = true)
        {
            if(flag) { 
                canvas.FillEllipse(new SolidBrush(Color.Gray),-15,-15,30,30);
                canvas.DrawEllipse(new Pen(Color.Black,2), -15,-15,30,30);
                canvas.DrawLine(new Pen(Color.Black,2),0,0,25,0);
            }
            else
            {
                canvas.FillEllipse(new SolidBrush(Color.White), -15, -15, 30, 30);
                canvas.DrawEllipse(new Pen(Color.White, 2), -15, -15, 30, 30);
                canvas.DrawLine(new Pen(Color.Black, 2), 0, 0, 25, 0);
            }
        } 
        public override GraphicsPath GetGraphicsPath()//добавляем рисунку колиженбокс для взаимодействия с другими объектами
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-15,-15,30,30);
            return path;
        }
        public override void Overlap(BaseObjects obj)//проверка пересечений
        {
            base.Overlap(obj);//вызывем метод базового класса
            //проверяем с каким объектом пересеклись и вызываем соответствующую реакцию
            if (obj is Marker) OnMarkerOverlap(obj as Marker);
            if (obj is Target) OnTargetOverlap(obj as Target);
        }
        
    }
}
