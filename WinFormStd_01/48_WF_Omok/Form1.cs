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
    }
}
