using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dodge_WF
{
    public partial class Form1 : Form
    {
        Random rs = new Random(); // 랜덤 함수 출력
                                  // 볼 갯수 
        const int missile_quantity = 100;


        // 볼 사이즈와 컬러 구조체 선언
        Point[] ballPoint = new Point[missile_quantity];
        int sizePoint_x = 5;
        int sizePoint_y = 5;
        // 비행기컬러 , 비행기 사이즈 구조체 선언
        int ship_x = 0;
        int ship_y = 0;
        int shipsize = 25;

        int ready_x = 0;
        int ready_y = 0;
        int readysize_x = 200;
        int readysize_y = 200;

        int gameover_x = 0;
        int gameover_y = 0;
        int gameover_size_x = 200;
        int gameover_size_y = 200;

        int space_x = 0;
        int space_y = 0;
        int space_size_x = 200;
        int space_size_y = 200;

        int gameState = 0;

        // 볼 속도 선언
        int[] x_speed = new int[100];
        int[] y_speed = new int[100];

        // 위치판단
        Rectangle rt = new Rectangle();

        public Form1()
        {
            InitializeComponent();

            rt.X = ship_x;
            rt.Y = ship_y;
            rt.Width = shipsize;
            rt.Height = shipsize;


            ship_x = (pictureBox1.Width / 2);
            ship_y = (pictureBox1.Height / 2);

            ready_x = (pictureBox1.Width / 2) - (readysize_x / 2);
            ready_y = (pictureBox1.Height * 1 / 3) - (readysize_y / 2);

            gameover_x = (pictureBox1.Width / 2) - (gameover_size_x / 2);
            gameover_y = (pictureBox1.Height / 2) - (gameover_size_x / 2);

            space_x = (pictureBox1.Width / 2) - (space_size_x / 2);
            space_y = (pictureBox1.Height * 2 / 3) - (space_size_y / 2);

            for (int i = 0; i < missile_quantity; i++)
            {
                ballPoint[i].X = rs.Next(0, pictureBox1.Width);
                ballPoint[i].Y = rs.Next(0, pictureBox1.Height);
                x_speed[i] = rs.Next(-2, 2);
                y_speed[i] = rs.Next(-2, 2);
                if (x_speed[i] == 0)
                    x_speed[i] = 1;
                else if (y_speed[i] == 0)
                    y_speed[i] = 1;
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {

            if (gameState == 0)
            {
                Image ready = Bitmap.FromFile("ready.png"); // 이미지로 가져오는 소스
                e.Graphics.DrawImage(ready, ready_x, ready_y, readysize_x, readysize_y);

                Image space = Bitmap.FromFile("space.png"); // 이미지로 가져오는 소스
                e.Graphics.DrawImage(space, space_x, space_y, space_size_x, space_size_y);

            }
            else if (gameState == 3)
            {
                Image gameover = Bitmap.FromFile("gameover.png"); // 이미지로 가져오는 소스
                e.Graphics.DrawImage(gameover, gameover_x, gameover_y, gameover_size_x, gameover_size_y);
            }
            else if (gameState == 1)
            {
                //비행기 그리는 소스
                Image ship = Bitmap.FromFile("ship.png"); // 이미지로 가져오는 소스
                e.Graphics.DrawImage(ship, ship_x, ship_y, shipsize, shipsize);
                //
                // e.Graphics.FillRectangle(Brushes.Orange, rt);
            }

            for (int i = 0; i < missile_quantity; i++)
            {
                if (i % 2 == 0) e.Graphics.FillEllipse(Brushes.Blue, ballPoint[i].X, ballPoint[i].Y, sizePoint_x, sizePoint_y);
                else e.Graphics.FillEllipse(Brushes.Purple, ballPoint[i].X, ballPoint[i].Y, sizePoint_x, sizePoint_y);

            }


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (gameState == 1)
            {
                for (int i = 0; i < missile_quantity; i++)
                {
                    // 볼을 이동시키는 소스
                    ballPoint[i].X += x_speed[i];
                    ballPoint[i].Y += y_speed[i];
                    pictureBox1.Invalidate(); // 화면 갱신

                    // 벽을 만나면 튕기는 소스
                    if (ballPoint[i].X < 0) x_speed[i] *= -1;
                    if (ballPoint[i].X > pictureBox1.Width) x_speed[i] *= -1;
                    if (ballPoint[i].Y < 0) y_speed[i] *= -1;
                    if (ballPoint[i].Y > pictureBox1.Height) y_speed[i] *= -1;
                }

            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        class Watch
        {
            static public int milisec = 0;
            static public int sec = 0;

            static public bool status = false;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            Watch.milisec++;
            if (Watch.milisec > 59)
            {
                Watch.milisec = 0;
                Watch.sec++;
                if (Watch.sec > 59)
                    Watch.sec = 0;
            }
            textBox1.Text = Watch.sec + ":" + Watch.milisec + "초";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //키코드를 뗄 때 입력받는 함수

            if (e.KeyCode == Keys.Right) bright = false;
            if (e.KeyCode == Keys.Left) bleft = false;
            if (e.KeyCode == Keys.Down) bdown = false;
            if (e.KeyCode == Keys.Up) bup = false;

            if (e.KeyCode == Keys.Space) gameState = 1;






            //
            if (Watch.status == false && gameState == 1)
            {
                timer3.Start();
                timer3.Interval = 10;
                Watch.status = true;
            }
            else if (Watch.status == true && gameState == 3)
            {
                timer3.Stop();
                Watch.status = false;
                Watch.milisec = 0;
                Watch.sec = 0;
                gameState = 0;

            }
            else if (gameState == 0)
            {
                Watch.milisec = 0;
                Watch.sec = 0;
                Watch.status = false;
                textBox1.Text = Watch.sec + ":" + Watch.milisec + "초";
                timer3.Stop();
            }

        }
        // 키보드 이동용 bool변수
        bool bleft = false, bright = false, bup = false, bdown = false, bspace = false;

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (gameState == 1)
            {

                // 비행기 움직임
                if (bright) { if (ship_x < pictureBox1.Width - 30) ship_x += 5; }
                if (bleft) { if (ship_x > 0) ship_x -= 5; }
                if (bup) { if (ship_y > 0) ship_y -= 5; }
                if (bdown) { if (ship_y < pictureBox1.Height - 30) ship_y += 5; }

                for (int i = 0; i < missile_quantity; i++)
                {
                    if (rt.Contains(ballPoint[i].X, ballPoint[i].Y) == true)
                    {
                        gameState = 3;
                        textBox1.Text = Watch.sec + ":" + Watch.milisec + "초";
                        timer3.Stop();
                        MessageBox.Show("기록 : " + Watch.sec + ":" + Watch.milisec + "초");
                        break;

                    }
                }
            }
            else if (gameState == 3)
            {
                Watch.status = false;
                bleft = false;
                bright = false;
                bup = false;
                bdown = false;
                if (bspace) gameState = 0;
                for (int i = 0; i < missile_quantity; i++)
                {
                    ballPoint[i].X += x_speed[i];
                    ballPoint[i].Y += y_speed[i];
                    pictureBox1.Invalidate();
                }
            }
            else if (gameState == 0)
            {
                Watch.status = false;

            }
            pictureBox1.Invalidate();

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            // 키코드를 누를 때 입력 받는 함수
            if (e.KeyCode == Keys.Right) bright = true;
            if (e.KeyCode == Keys.Left) bleft = true;
            if (e.KeyCode == Keys.Down) bdown = true;
            if (e.KeyCode == Keys.Up) bup = true;
        }
    }
}
