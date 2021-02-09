using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace _40_WF_ChartControl2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Text = "진주고등학교 중간고사 성적";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chart1.Titles.Add("중간고사 성적");
            chart1.Series.Add("Series2");
            chart1.Series["Series1"].LegendText = "한국사";
            chart1.Series["Series2"].LegendText = "외국어";

            chart1.ChartAreas.Add("ChartArea2");
            chart1.Series["Series2"].ChartArea = "ChartArea2";

            Random r = new Random();
            for(int i = 0;i<100;i++)
            {
                chart1.Series["Series1"].Points.AddXY(i, r.Next(100));
                chart1.Series["Series2"].Points.AddXY(i, r.Next(100));
            }
        }

        private void btnOneChartArea_Click(object sender, EventArgs e)
        {
            chart1.ChartAreas.RemoveAt(chart1.ChartAreas.IndexOf("ChartArea2"));
            chart1.Series["Series2"].ChartArea = "ChartArea1";
        }

        private void btnTwoChartArea_Click(object sender, EventArgs e)
        {
            chart1.ChartAreas.Add("ChartArea2");
            chart1.Series["Series2"].ChartArea = "ChartArea2";
        }
    }
}
