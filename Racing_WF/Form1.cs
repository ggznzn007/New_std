using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Racing_WF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int carSpeed = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            moveLine(carSpeed);
            blockCar(3);
        }

        Random r = new Random();
        int x;

        internal void blockCar(int speed)
        {
            if(pictureBox_Block.Top>=600)
            {
                x = r.Next(0, 400);
                pictureBox_Block.Location = new Point(x,0);
            }
            else
            {
                pictureBox_Block.Top += speed;
            }
        }

        internal void moveLine(int speed)
        {
            if (pictureBox1.Top >= 500)
            {
                pictureBox1.Top = 0;
            }
            else
            {
                pictureBox1.Top += speed;
            }
            if (pictureBox2.Top >= 500)
            {
                pictureBox2.Top = 0;
            }
            else
            {
                pictureBox2.Top += speed;
            }
            if (pictureBox3.Top >= 500)
            {
                pictureBox3.Top = 0;
            }
            else
            {
                pictureBox3.Top += speed;
            }
            if (pictureBox4.Top >= 500)
            {
                pictureBox4.Top = 0;
            }
            else
            {
                pictureBox4.Top += speed;
            }

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                if (pictureBox_Car.Left > 19)
                    pictureBox_Car.Left += -15;
            }
            if (e.KeyCode == Keys.Right)
            {
                if (pictureBox_Car.Right < 398 - pictureBox_Car.Width)
                    pictureBox_Car.Left += 15;
            }

            if(e.KeyCode==Keys.Up)
            {
                if(carSpeed<15)
                { carSpeed++; }
            }
            if(e.KeyCode==Keys.Down)
            {
                if(carSpeed>0)
                { carSpeed--; }
            }
        }
    }
}
