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
        }
    }
}
