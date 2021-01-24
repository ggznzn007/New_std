using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _19_Timer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblTime.Text = "";
            Timer1.Interval = 1000; // 1초
            Timer1.Tick += Timer1_Tick;
            Timer1.Start(); // Timer.Enabled = true;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Location = new Point(ClientSize.Width / 2 - lblTime.Width / 2,
                ClientSize.Height / 2 - lblTime.Height / 2);
            lblTime.Font = new Font("맑은 고딕", 30, FontStyle.Bold);
            lblTime.Text = DateTime.Now.ToString();
        }
    }
}
