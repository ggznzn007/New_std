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
        Random rs = new Random();
        const int missile_quantity = 100;
        int randNum = 0;

        // 볼 사이즈와 컬러 구조체 선언
        Point[] ballPoint = new Point[missile_quantity];
        int sizePoint_x = 3;
        int sizePoint_y = 3;
        // 비행기 사이즈와 비행기 컬러 구조체 선언
        int plane_x = 0;
        int plane_y = 0;
        int planeSize = 25;

        int ready_x = 0;
        int ready_y = 0;
        int readySize_x = 200;
        int readySize_y = 100;

        int gameOver_x = 0;
        int gameOver_y = 0;
        int gameOver_size_x = 0;
        int gameOver_size_y = 0;

        int gameState = 0;

        // 볼 스피드
        int[] x_Speed = new int[100];
        int[] y_Speed = new int[100];

        public Form1()
        {
            InitializeComponent();
            plane_x = (pictureBox1.Width / 2);
            plane_y = (pictureBox1.Height / 2);

            ready_x = (pictureBox1.Width / 2) - (readySize_x / 2);
            ready_y = (pictureBox1.Height / 2) - (readySize_y / 2);

            gameOver_x = (pictureBox1.Width / 2) - (gameOver_size_x / 2);
            gameOver_y = (pictureBox1.Height / 2) - (gameOver_size_y / 2);

            for (int i = 0; i < missile_quantity; i++)
            {
                ballPoint[i].X = rs.Next(0, pictureBox1.Width);
                ballPoint[i].Y = rs.Next(0, pictureBox1.Height);
                x_Speed[i] = rs.Next(-3, 3);
                y_Speed[i] = rs.Next(-3, 3);
                if (x_Speed[i] == 0)
                    x_Speed[i] = 1;
                else if (y_Speed[i] == 0)
                    y_Speed[i] = 1;
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (gameState == 0)
            {
                Image ready = Bitmap.FromFile(""); // 이미지 소스 가져오기
                e.Graphics.DrawImage(ready, ready_x, ready_y, readySize_x, readySize_y);
            }
            else if (gameState == 3)
            {
                Image gameover = Bitmap.FromFile("");
                e.Graphics.DrawImage(gameover, gameOver_x, gameOver_y, gameOver_size_x, gameOver_size_y);
            }
            else if (gameState == 1)
            {
                // 비행기 그리는 소스
                Image plane = Bitmap.FromFile("");
                e.Graphics.DrawImage(plane, plane_x, plane_y, planeSize, planeSize);
            }

            for (int i = 0; i < missile_quantity; i++)
            {
                if (i % 2 == 0) { e.Graphics.FillEllipse(Brushes.Red, ballPoint[i].X, ballPoint[i].Y, sizePoint_x, sizePoint_y); }
                else { e.Graphics.FillEllipse(Brushes.DeepPink, ballPoint[i].X, ballPoint[i].Y, sizePoint_x, sizePoint_y); }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (gameState == 1)
            {
                for (int i = 0; i < missile_quantity; i++)
                {
                    // 볼 이동
                    ballPoint[i].X += x_Speed[i];
                    ballPoint[i].Y += y_Speed[i];
                    pictureBox1.Invalidate(); // 화면 갱신

                    // 벽을 만나면 튕기는 소스
                    if (ballPoint[i].X < 0) { x_Speed[i] *= -1; }
                    if (ballPoint[i].X > pictureBox1.Width) { x_Speed[i] *= -1; }
                    if (ballPoint[i].Y < 0) { y_Speed[i] *= -1; }
                    if (ballPoint[i].Y > pictureBox1.Height) { y_Speed[i] *= -1; }
                }
            }
        }
        // 키보드 이동용 변수
        bool bLeft = false, bRight = false, bUp = false, bDown = false;
        class Watch
        {
            static public int milisec = 0;
            static public int sec = 0;

            static public bool status = false;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // 키코드 누를 때 입력 함수
            if (e.KeyCode == Keys.Right) bRight = true;
            if (e.KeyCode == Keys.Left) bLeft = true;
            if (e.KeyCode == Keys.Up) bUp = true;
            if (e.KeyCode == Keys.Down) bDown = true;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            Watch.milisec++;
            if (Watch.milisec > 59)
            {
                Watch.milisec = 0;
                Watch.sec++;
                if (Watch.sec > 59)
                { Watch.sec = 0; }
            }
            textBox1.Text = Watch.sec + ":" + Watch.milisec;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            // 키코드를 뗄 때 입력받는 함수
            if (e.KeyCode == Keys.Right) bRight = false;
            if (e.KeyCode == Keys.Left) bLeft = false;
            if (e.KeyCode == Keys.Up) bUp = false;
            if (e.KeyCode == Keys.Down) bDown = false;

            if(Watch.status == false&&gameState==1)
            {
                timer3.Start();
                timer3.Interval = 10;
                Watch.status = true;
            }
            else if(Watch.status == true && gameState==0 && gameState ==3)
            {
                timer3.Stop();
                Watch.status = false;
            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            if(gameState==1)
            {
                // 비행기 이동
                if(bRight) { if (plane_x < pictureBox1.Width - 30) plane_x += 5; }
                if(bLeft) { if (plane_x > 0) plane_x -= 5; }
                if(bUp) { if (plane_y > 0) plane_y -= 5; }
                if(bDown) { if (plane_y < pictureBox1.Height - 30) plane_y += 5; }
            }
            pictureBox1.Invalidate();
        }

    }
}
