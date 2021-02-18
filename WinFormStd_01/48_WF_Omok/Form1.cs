using System;
using System.Drawing;
using System.Windows.Forms;

namespace _48_WF_Omok
{
    public partial class Form1 : Form
    {
        int margin = 40;
        int squareSize = 30; // 눈금 크기
        int stoneSize = 28; // 바둑돌 크기
        int pointSize = 10; // 화점 크기

        Graphics g;
        Pen pen;
        Brush wBrush, bBrush;

        enum STONE { none, black, white};
        STONE[,] badookpan = new STONE[19, 19];
        bool flag = false; // false == 검은돌, true == 흰돌
        bool imageFlag = false;
        public Form1()
        {
            InitializeComponent();

            this.BackColor = Color.Orange;

            pen = new Pen(Color.Black);
            bBrush = new SolidBrush(Color.Black);
            wBrush = new SolidBrush(Color.White);

            this.ClientSize = new Size(2 * margin + 18 * squareSize,
                2 * margin + 18 * squareSize + menuStrip1.Height);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawBoard();
        }


        private void DrawBoard()
        {
            g = panel1.CreateGraphics();

            for(int i =0; i<19;i++)
            {
                g.DrawLine(pen, new Point(margin + i * squareSize, margin),// 세로선
                    new Point(margin + i * squareSize, margin + 18 * squareSize));
                g.DrawLine(pen, new Point(margin, margin + i * squareSize),// 가로선
                    new Point(margin + 18 * squareSize, margin + i * squareSize));
            }

            // 화점 그리기
            for(int x = 3; x<=15;x+=6)
                for(int y = 3; y<=15;y+=6)
                {
                    g.FillEllipse(bBrush,
                        margin + squareSize * x - pointSize / 2,
                        margin + squareSize * y - pointSize / 2,
                        pointSize, pointSize);
                }
        }


        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            // e.X는 픽셀 단위, x는 바둑판 좌표
            int x = (e.X - margin + squareSize / 2) / squareSize;
            int y = (e.Y - margin + squareSize / 2) / squareSize;

            if (badookpan[x, y] != STONE.none) return;

            // 바둑판에 돌을 그리기 위한 Rectangle
            Rectangle r = new Rectangle(margin + squareSize * x - stoneSize / 2,
                margin + squareSize * y - stoneSize / 2, stoneSize, stoneSize);

            // 검은돌 차례
            if(flag==false)
            {
                if(imageFlag==false)
                g.FillEllipse(bBrush, r);
                else
                {
                    Bitmap bmp = new Bitmap("../../Images/black.png");
                    g.DrawImage(bmp, r);
                }
                flag = true;
                badookpan[x, y] = STONE.black;
            }
            else
            {
                if(imageFlag==false)
                g.FillEllipse(wBrush, r);
                else
                {
                    Bitmap bmp = new Bitmap("../../Images/white.png");
                    g.DrawImage(bmp, r);
                }
                flag = false;
                badookpan[x, y] = STONE.white;
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
