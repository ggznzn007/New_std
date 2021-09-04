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

            for(int i =0; i<missile_quantity;i++)
            {
                ballPoint[i].X = rs.Next(0, pictureBox1.Width);
                ballPoint[i].Y = rs.Next(0, pictureBox1.Height);
            }
        }
    }
}
