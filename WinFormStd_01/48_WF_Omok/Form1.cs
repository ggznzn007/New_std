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
    }
}
