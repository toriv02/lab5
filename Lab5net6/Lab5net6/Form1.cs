using Lab5net6.Objects;
using System.Xml.Serialization;

namespace Lab5net6
{
    public partial class MainForm : Form
    {
        private int targetCount =2;//���������� ���������� �� ���������� ����� �� ������
        Random rnd=new Random();//���������� ������������ ��� ��������� ��������� �����
        List<BaseObjects> objects = new();//������ �������� �� ����
        BlackSpace space;//������ �������
        Player player;//�����
        Marker marker;//������
        Target target;//����
        private int score = 0;//����
        public MainForm()//����� ��� ������������� �����������
        {
            InitializeComponent();
            space = new BlackSpace(-PictureBox.Width / 4, PictureBox.Height, 0);//�������������� � ��������� � ������ �������� ������ �������
            space.Width = PictureBox.Width / 4; space.Height = PictureBox.Height;//����� ������ ������ �������
            objects.Add(space);


            player = new Player(PictureBox.Width / 2, PictureBox.Height / 2, 0);//�������������� � ��������� � ������ �������� ������
            // �������� ������� �� ����������� ������ � ��������
            player.OnOverlap += (p, obj) =>
            {
                Logs.Text = $"[{DateTime.Now:HH:mm:ss:ff}] ����� ��������� � {obj}\n" + Logs.Text;
            };
            // ������� ������� �� ����������� � ��������
            player.OnMarkerOverlap += (m) =>
            {
                // ���� ������, �� ������ ������ �� ������������� objects
                objects.Remove(m);
                marker = null; // � ������� ������
            };
            // ������� ������� �� ����������� � �����
            player.OnTargetOverlap += (t) =>
            {
                ScoreLabel.Text="����: "+ ++score;//���������� ������� �����
                objects.Remove(t);//������ ����
                //���������� ���� �� ����� ��������� �������.
                t = null;
                t = new Target(rnd.Next() % PictureBox.Width, rnd.Next() % PictureBox.Height, 0);
                objects.Add(t);
            };
            objects.Add(player);


            for (int i = 0; i < targetCount; i++)//������������ ��������� ���������� ����� ������������� ����������� �� �����
            {
               target = new Target(rnd.Next() % PictureBox.Width, rnd.Next() % PictureBox.Height, 0);
               objects.Add(target);
            }

            //������������� ������
            marker = new Marker(PictureBox.Width / 2 + 150, PictureBox.Height / 2, 0);
            objects.Add(marker);
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            var canvas = e.Graphics;
            canvas.Clear(Color.White);//������ ����� �� ����������� �������
            updateSpace();//�������� ��������� ���������
            updatePlayer();
            //�������� ������� ������
            // ����� ��� objects �� objects.ToList()
            // ��� ����� ��������� ����� ������
            // � �������� �������������� ������������ objects ����� �� ����� foreach
            foreach (var curr in objects.ToList())
            {
                // �������� ���� �� ����������� � �������
                if (curr != player && player.Overlaps(curr, canvas))
                {
                    player.Overlap(curr); // �� ���� ����� ��������� � ��������
                    curr.Overlap(player); // � ������ ��������� � �������
                }
                if (curr is Target){//�������� �������� �� ������� ������ �����
                    if (curr.Exists()) { curr.ChangeR(); }//���� ���� ���������� (� R ������ 0), �� �������� ������
                    else//���� ���� �� ����������
                    {
                        objects.Remove(curr);//������ ������� ���� �� ������
                        //������ ����� ���� �� ����� �������
                        target = null;
                        target = new Target(rnd.Next() % PictureBox.Width, rnd.Next() % PictureBox.Height, 0);
                        objects.Add(target);
                    }
                }
            }
            foreach (var curr in objects.ToList())
            {
                canvas.Transform = curr.GetTransform();// ������������� ����� �������
                if (curr != space && space.Overlaps(curr, canvas)) curr.Render(canvas,false); // ��������
                else curr.Render(canvas); // ��������
            }
        }

        private void timer1_Tick(object sender, EventArgs e)//������� ������������� ������ ��� �������
        {

            PictureBox.Invalidate();//����������� ����������� �� ������� ����
        }

        private void PictureBox_MouseClick(object sender, MouseEventArgs e)//������� ������� �� �����
        {
            // ��� ������� �������� ������� �� ����� ���� �� ��� �� ������
            if (marker == null)
            {
                marker = new Marker(0, 0, 0);
                objects.Add(marker); // � ������� �� ������ ��������� � objects
            }
            //������� ������� ������� � ���������� ����������� ������� ��� �������
            marker.X = e.X;
            marker.Y = e.Y;
        }
        private void updatePlayer()//����� ������������� �� ���������� ��������� ������
        {
            // ��� ��������� �������� �� marker �� �������
            if (marker != null)
            {
                //���������� ������ ����� ������� � ���������
                float dx = marker.X - player.X;
                float dy = marker.Y - player.Y;
                //������� ��� ������
                float length = (float)Math.Sqrt(dx * dx + dy * dy);
                dx /= length;//����������� ����������
                dy /= length;
                //������������� ���������� ������
                // �� ���� �� ������ ���������� ������ dx, dy
                // ��� ������ ���������, ������ ���� ������ ����������
                // ������� ����������� ������ � �������
                // 0.5 ������ ����������� ������� �������� �� ����
                // � ������� ���� ������������ �������� ��������
                player.vX += dx * 0.5f;
                player.vY += dy * 0.5f;

                // ����������� ���� �������� ������ 
                player.Angle = 90 - MathF.Atan2(player.vX, player.vY) * 180 / MathF.PI;
            }
            // ���������� ������,
            // ����� �����, ����� ����� ��������� ������� ��������� ����������� ����������
            player.vX += -player.vX * 0.1f;
            player.vY += -player.vY * 0.1f;

            // �������� ������� ������ � ������� ������� ��������
            player.X += player.vX;
            player.Y += player.vY;
        }
        private void updateSpace()//����� ������������� �� ���������� ��������� ���������
        {
            //�������� ���������� �� � ���� ��� ����� �� ������� ���� �� ��������� �� ������ ������� ���� � ��������� �������
            if (space.X > (4+3+7+18)+PictureBox.Width+ PictureBox.Width / 4) space.X = -PictureBox.Width / 4;
            space.vX = 3f;//����� �������� �������� ���������
            space.X += space.vX;

        }

    }
}