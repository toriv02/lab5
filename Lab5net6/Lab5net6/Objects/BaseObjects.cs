using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5net6
{
    internal class BaseObjects
    {
        //координаты в пространстве для объекта
        public float X;
        public float Y;
        public float Angle;
        // добавил поле делегат, к которому можно будет привязать реакцию на собыития
        public Action<BaseObjects, BaseObjects> OnOverlap;
        public BaseObjects(float x, float y, float angle)
        {
            X = x;
            Y = y;
            this.Angle = angle;
        }
        public Matrix GetTransform()
        {
            // вытаскиваем матрицу преобразования Graphics
            var matrix = new Matrix();
            matrix.Translate(X, Y);// смещаем ее в пространстве в соответствии с координатами
            matrix.Rotate(Angle);
            return matrix;
        }
        // добавил виртуальный метод для отрисовки
        public virtual void Render(Graphics canvas,bool flag=true){}
        public virtual GraphicsPath GetGraphicsPath() { 
            return new GraphicsPath();
        }
        // так как пересечение учитывает толщину линий и матрицы трансформацией
        // то для того чтобы определить пересечение объекта с другим объектом
        // надо передать туда объект Graphics, это не очень удобно 
        // но в учебных целях реализуем так
        public virtual bool Overlaps( BaseObjects obj, Graphics canvas)
        {
            // берем информацию о форме
            var path1=this.GetGraphicsPath();
            var path2=obj.GetGraphicsPath();
            // применяем к объектам матрицы трансформации
            path1.Transform(this.GetTransform());
            path2.Transform(obj.GetTransform());
            // используем класс Region, который позволяет определить 
            // пересечение объектов в данном графическом контексте
            var region = new Region(path1);
            region.Intersect(path2); // пересекаем формы
            return !region.IsEmpty(canvas); // если полученная форма не пуста то значит было пересечение
        }
        public virtual void Overlap(BaseObjects obj)
        {
            if (this.OnOverlap != null)
            { //если к полю есть привязанные функции
                this.OnOverlap(this, obj);//то вызываем их
            }
        }
        public virtual bool Exists() { return true; }
        public virtual void ChangeR() { }
    }
}
