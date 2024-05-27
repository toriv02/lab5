using Lab5net6.Objects;
using System.Xml.Serialization;

namespace Lab5net6
{
    public partial class MainForm : Form
    {
        private int targetCount =2;//переменная отвечающая за количество целей на холсте
        Random rnd=new Random();//переменная используемая для генерации положения целей
        List<BaseObjects> objects = new();//список объектов на поле
        BlackSpace space;//чёрная область
        Player player;//игрок
        Marker marker;//маркер
        Target target;//цель
        private int score = 0;//счёт
        public MainForm()//метод для инициализации компонентов
        {
            InitializeComponent();
            space = new BlackSpace(-PictureBox.Width / 4, PictureBox.Height, 0);//инициализируем и добавляем в список объектов чёрную область
            space.Width = PictureBox.Width / 4; space.Height = PictureBox.Height;//задаю размер чёрной области
            objects.Add(space);


            player = new Player(PictureBox.Width / 2, PictureBox.Height / 2, 0);//инициализируем и добавляем в список объектов игрока
            // добавляю реакцию на пересечение игрока с объектом
            player.OnOverlap += (p, obj) =>
            {
                Logs.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Игрок пересекся с {obj}\n" + Logs.Text;
            };
            // добавил реакцию на пересечение с маркером
            player.OnMarkerOverlap += (m) =>
            {
                // если достиг, то удаляю маркер из оригинального objects
                objects.Remove(m);
                marker = null; // и обнуляю маркер
            };
            // добавил реакцию на пересечение с целью
            player.OnTargetOverlap += (t) =>
            {
                ScoreLabel.Text="Очки: "+ ++score;//увеличиваю счётчик очков
                objects.Remove(t);//удаляю цель
                //пересоздаю цель на новой случайной позиции.
                t = null;
                t = new Target(rnd.Next() % PictureBox.Width, rnd.Next() % PictureBox.Height, 0);
                objects.Add(t);
            };
            objects.Add(player);


            for (int i = 0; i < targetCount; i++)//иницилизирую указанное количество целей единовременно находящихся на форме
            {
               target = new Target(rnd.Next() % PictureBox.Width, rnd.Next() % PictureBox.Height, 0);
               objects.Add(target);
            }

            //инициализирую маркер
            marker = new Marker(PictureBox.Width / 2 + 150, PictureBox.Height / 2, 0);
            objects.Add(marker);
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            var canvas = e.Graphics;
            canvas.Clear(Color.White);//очищаю холст от превидущего рисунка
            updateSpace();//обновляю положение плоскости
            updatePlayer();
            //обновляю позицию игрока
            // меняю тут objects на objects.ToList()
            // это будет создавать копию списка
            // и позволит модифицировать оригинальный objects прямо из цикла foreach
            foreach (var curr in objects.ToList())
            {
                // проверяю было ли пересечение с игроком
                if (curr != player && player.Overlaps(curr, canvas))
                {
                    player.Overlap(curr); // то есть игрок пересекся с объектом
                    curr.Overlap(player); // и объект пересекся с игроком
                }
                if (curr is Target){//проверяю является ли текущий объект целью
                    if (curr.Exists()) { curr.ChangeR(); }//если цель существует (её R больше 0), то уменьшаю размер
                    else//если цель не существует
                    {
                        objects.Remove(curr);//удаляю текущую цель из списка
                        //создаю новую цель на новой позиции
                        target = null;
                        target = new Target(rnd.Next() % PictureBox.Width, rnd.Next() % PictureBox.Height, 0);
                        objects.Add(target);
                    }
                }
            }
            foreach (var curr in objects.ToList())
            {
                canvas.Transform = curr.GetTransform();// устанавливаем новую матрицу
                if (curr != space && space.Overlaps(curr, canvas)) curr.Render(canvas,false); // рендерим
                else curr.Render(canvas); // рендерим
            }
        }

        private void timer1_Tick(object sender, EventArgs e)//событие срабатывающие каждый тик таймера
        {

            PictureBox.Invalidate();//перерисовка изображения на игровом поле
        }

        private void PictureBox_MouseClick(object sender, MouseEventArgs e)//событие нажатие на форму
        {
            // тут добавил создание маркера по клику если он еще не создан
            if (marker == null)
            {
                marker = new Marker(0, 0, 0);
                objects.Add(marker); // и главное не забыть пололжить в objects
            }
            //считыаю позицию курсора и присваиваю координатам маркера эту позицию
            marker.X = e.X;
            marker.Y = e.Y;
        }
        private void updatePlayer()//метод ответственный за обновление положения игрока
        {
            // тут добавляем проверку на marker не нулевой
            if (marker != null)
            {
                //расчитывем вектор между игроком и маркерами
                float dx = marker.X - player.X;
                float dy = marker.Y - player.Y;
                //находим его длинну
                float length = (float)Math.Sqrt(dx * dx + dy * dy);
                dx /= length;//нормализуем координаты
                dy /= length;
                //пересчитываем координаты игрока
                // по сути мы теперь используем вектор dx, dy
                // как вектор ускорения, точнее даже вектор притяжения
                // который притягивает игрока к маркеру
                // 0.5 просто коэффициент который подобрал на глаз
                // и который дает естественное ощущение движения
                player.vX += dx * 0.5f;
                player.vY += dy * 0.5f;

                // расчитываем угол поворота игрока 
                player.Angle = 90 - MathF.Atan2(player.vX, player.vY) * 180 / MathF.PI;
            }
            // тормозящий момент,
            // нужен чтобы, когда игрок достигнет маркера произошло постепенное замедление
            player.vX += -player.vX * 0.1f;
            player.vY += -player.vY * 0.1f;

            // пересчет позиция игрока с помощью вектора скорости
            player.X += player.vX;
            player.Y += player.vY;
        }
        private void updateSpace()//метод ответственный за обновление положение плоскости
        {
            //проверяю координату по х если она вышла за игровое поле то переносим на другую сторону поля в стартовую позицию
            if (space.X > (4+3+7+18)+PictureBox.Width+ PictureBox.Width / 4) space.X = -PictureBox.Width / 4;
            space.vX = 3f;//задаю скорость движения плоскости
            space.X += space.vX;

        }

    }
}