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

namespace _41_WF_GraphWithChart
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Text = "Graph using Chart";
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            chart1.ChartAreas[0].BackColor = Color.Black;

            // ChartArea X축과 Y축을 설정
            chart1.ChartAreas[0].AxisX.Minimum = -20;
            chart1.ChartAreas[0].AxisX.Maximum = 20;
            chart1.ChartAreas[0].AxisX.Interval = 2;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Pink;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = 
                ChartDashStyle.Dash;

            chart1.ChartAreas[0].AxisY.Minimum = -2;
            chart1.ChartAreas[0].AxisY.Maximum = 2;
            chart1.ChartAreas[0].AxisY.Interval = 0.5;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.HotPink;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineDashStyle =
                ChartDashStyle.Dash;

            // Series 설정(Sin)
            chart1.Series[0].ChartType = SeriesChartType.Line;
            chart1.Series[0].Color = Color.LightBlue;
            chart1.Series[0].BorderWidth = 2;
            chart1.Series[0].LegendText = "sin(x)/x";

            // Series 추가, 설정(Cos)
            if(chart1.Series.Count==1)
            {
                chart1.Series.Add("Cos");
                chart1.Series["Cos"].ChartType = SeriesChartType.Line;
            }
        }
    }
}
