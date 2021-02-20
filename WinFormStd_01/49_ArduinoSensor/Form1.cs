using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace _49_ArduinoSensor
{
    public partial class Form1 : Form
    {
        SerialPort sPort;
        private double xCount = 200; // 차트에 보여지는 데이터 개수
        List<SensorData> myData = new List<SensorData>(); // 리스트 자료구조
        
        public Form1()
        {
            InitializeComponent();

            // ComboBox
            foreach(var ports in SerialPort.GetPortNames())
            {
                comboBox1.Items.Add(ports);
            }
            comboBox1.Text = "Select Port";

            // 아두이노의 A0에서 받는 값의 범위 표시
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 1023;

            // 차트 모양 세팅
            ChartSetting();

            // 숫자 버튼
            button1.BackColor = Color.Blue;
            button1.ForeColor = Color.White;
            button1.Text = "";
            button1.Font = new Font("맑은 고딕", 16, FontStyle.Bold);

            label1.Text = "Connection Time : ";
            textBox1.TextAlign = HorizontalAlignment.Center;
            btnConnect.Enabled = false;
            btnDisconnect.Enabled = false;
        }

        private void ChartSetting()
        {
            chart1.ChartAreas.Clear();
            chart1.ChartAreas.Add("draw");
            chart1.ChartAreas["draw"].AxisX.Minimum = 0;
            chart1.ChartAreas["draw"].AxisX.Maximum = xCount;
            chart1.ChartAreas["draw"].AxisX.Interval = xCount/4;
            chart1.ChartAreas["draw"].AxisX.MajorGrid.LineColor = Color.White;
            chart1.ChartAreas["draw"].AxisX.MajorGrid.LineDashStyle =
                ChartDashStyle.Dash;

            chart1.ChartAreas["draw"].AxisY.Minimum = 0;
            chart1.ChartAreas["draw"].AxisY.Maximum = 1024;
            chart1.ChartAreas["draw"].AxisY.Interval = 200;
            chart1.ChartAreas["draw"].AxisY.MajorGrid.LineColor = Color.White;
            chart1.ChartAreas["draw"].AxisY.MajorGrid.LineDashStyle =
                ChartDashStyle.Dash;

            chart1.ChartAreas["draw"].BackColor = Color.Blue;
            chart1.ChartAreas["draw"].CursorX.AutoScroll = true;

            chart1.ChartAreas["draw"].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas["draw"].AxisX.ScrollBar.ButtonStyle =
                ScrollBarButtonStyles.SmallScroll;
            chart1.ChartAreas["draw"].AxisX.ScrollBar.ButtonColor =
                Color.LightSteelBlue;

            chart1.Series.Clear();
            chart1.Series.Add("PhotoCell");
            chart1.Series["PhotoCell"].ChartType = SeriesChartType.Line;
            chart1.Series["PhotoCell"].Color = Color.LightGreen;
            chart1.Series["PhotoCell"].BorderWidth = 3;
            if (chart1.Legends.Count > 0)
                chart1.Legends.RemoveAt(0);


        }
    }
}
