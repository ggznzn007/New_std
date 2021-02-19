using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _48_Omok
{
    public partial class Form1 : Form
    {
        int margin = 40;
        int 눈Size = 30; // 눈금 크기
        int 돌Size = 28; // 바둑돌 크기
        int 화점Size = 10;// 화점 크기

        Graphics g;
        Pen pen;
        Brush wBrush, bBrush;

        enum STONE { none, black, white };
        STONE[,] 바둑판 = new STONE[19, 19];
        bool flag = false; // false = 흑돌, true = 백돌
        bool imageFlag = false;

        public Form1()
        {
            InitializeComponent();

            this.BackColor = Color.Orange;

            pen = new Pen(Color.Black);
            bBrush = new SolidBrush(Color.Black);
            wBrush = new SolidBrush(Color.White);

            this.ClientSize = new Size(2 * margin + 18 * 눈Size,
                2 * margin + 18 * 눈Size + menuStrip1.Height);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            DrawBoard();
            base.OnPaint(e);
            DrawStones();
        }

        private void DrawStones()
        {
            for (int x = 0; x < 19; x++)
                for (int y = 0; y < 19; y++)
                    if (바둑판[x, y] == STONE.white)
                        if (imageFlag == false)
                            g.FillEllipse(wBrush, margin + x * 눈Size - 돌Size / 2,
                                margin + y * 눈Size - 돌Size / 2, 돌Size, 돌Size);
                        else
                        {
                            Bitmap bmp = new Bitmap("../../Images/white.png");
                            g.DrawImage(bmp, margin + x * 눈Size - 돌Size / 2,
                                margin + y * 눈Size - 돌Size / 2, 돌Size, 돌Size);
                        }

                    else if (바둑판[x, y] == STONE.black)
                        if (imageFlag == false)
                            g.FillEllipse(bBrush, margin + x * 눈Size - 돌Size / 2,
                                margin + y * 눈Size - 돌Size / 2, 돌Size, 돌Size);
                        else
                        {
                            Bitmap bmp = new Bitmap("../../Images/black.png");
                            g.DrawImage(bmp, margin + x * 눈Size - 돌Size / 2,
                                margin + y * 눈Size - 돌Size / 2, 돌Size, 돌Size);
                        }
        }


        private void DrawBoard()
        {
            g = panel1.CreateGraphics();

            for (int i = 0; i < 19; i++)
            {
                g.DrawLine(pen, new Point(margin + i * 눈Size, margin),
                    new Point(margin + i * 눈Size, margin + 18 * 눈Size));// 세로선
                g.DrawLine(pen, new Point(margin, margin + i * 눈Size),
                    new Point(margin + 18 * 눈Size, margin + i * 눈Size));// 가로선
            }

            // 화점그리기
            for (int x = 3; x <= 15; x += 6)
                for (int y = 3; y <= 15; y += 6)
                {
                    g.FillEllipse(bBrush,
                        margin + 눈Size * x - 화점Size / 2,
                        margin + 눈Size * y - 화점Size / 2,
                        화점Size, 화점Size);
                }
        }


        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            // e.X는 픽셀 단위, x는 바둑판 좌표
            int x = (e.X - margin + 눈Size / 2) / 눈Size;
            int y = (e.Y - margin + 눈Size / 2) / 눈Size;

            if (바둑판[x, y] != STONE.none) return;

            // 바둑판[x,y]에 돌을 그리기 위한 Rectangle
            Rectangle r = new Rectangle(margin + 눈Size * x - 돌Size / 2,
                margin + 눈Size * y - 돌Size / 2, 돌Size, 돌Size);

            // 흑돌 차례
            if (flag == false)
            {
                if (imageFlag == false)
                    g.FillEllipse(bBrush, r);
                else
                {
                    Bitmap bmp = new Bitmap("../../Images/black.png");
                    g.DrawImage(bmp, r);
                }
                flag = true;
                바둑판[x, y] = STONE.black;
            }
            else
            {
                if (imageFlag == false)
                    g.FillEllipse(wBrush, r);
                else
                {
                    Bitmap bmp = new Bitmap("../../Images/white.png");
                    g.DrawImage(bmp, r);
                }
                flag = false;
                바둑판[x, y] = STONE.white;
            }
        }
        private void 그리기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imageFlag = false;
        }

        private void 이미지ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imageFlag = true;
        }




    }
}
