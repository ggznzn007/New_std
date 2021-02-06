using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace _35_WPF_Analog_Clock
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point center;
        private double radius;
        private int hHand;
        private int mHand;
        private int sHand;
        public MainWindow()
        {
            InitializeComponent();

            aClock_Setting();
            TimerSetting();
        }

        private void aClock_Setting()
        {
            center = new Point(canvas1.Width / 2, canvas1.Height / 2);
            radius = canvas1.Width / 2;
            hHand = (int)(radius * 0.45);
            mHand = (int)(radius * 0.55);
            sHand = (int)(radius * 0.65);
        }

        private void TimerSetting()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);// 0.01초에 한번씩
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTime c = DateTime.Now;

            canvas1.Children.Clear(); // 현재 화면 지우기
            DrawClockFace(); // 시계판 그리기
            double radHr = (c.Hour % 12 + c.Minute / 60.0) * 30 * Math.PI / 180;
            double radMin = (c.Minute + c.Second / 60.0) * 6 * Math.PI / 180;
            double radSec = (c.Second + c.Millisecond / 1000.0) * 6 * Math.PI / 180;
            DrawHands(radHr, radMin, radSec); // 바늘 그리기
            
        }

        // 시계판 그리기
        private void DrawClockFace()
        {
            aClock.Stroke = Brushes.HotPink;
            aClock.StrokeThickness = 40;
            canvas1.Children.Add(aClock);
        }

        // 시계바늘 그리기
        private void DrawHands(double radHr,double radMin, double radSec)
        {
            // 시침
            DrawLine(hHand * Math.Sin(radHr), -hHand * Math.Cos(radHr),
                0, 0, Brushes.Black, 9, new Thickness(center.X, center.Y, 0, 0));
            // 분침
            DrawLine(mHand * Math.Sin(radMin), -mHand * Math.Cos(radMin),
                0, 0, Brushes.DarkGray, 7, new Thickness(center.X, center.Y, 0, 0));
            // 초침
            DrawLine(sHand * Math.Sin(radSec), -sHand * Math.Cos(radSec),
                0, 0, Brushes.Pink, 4, new Thickness(center.X, center.Y, 0, 0));

            Ellipse core = new Ellipse();
            core.Margin = new Thickness(canvas1.Width / 2 - 10,
                canvas1.Height / 2 - 10, 0, 0);
            core.Stroke = Brushes.SteelBlue;
            core.Fill = Brushes.HotPink;
            core.Width = 30;
            core.Height = 30;
            canvas1.Children.Add(core);
        }

        private void DrawLine(double x1, double y1, int x2, int y2,
            SolidColorBrush color, int thick, Thickness margin)
        {
            Line line = new Line();
            line.X1 = x1; line.Y1 = y1; line.X2 = x2; line.Y2 = y2;
            line.Stroke = color;
            line.StrokeThickness = thick;
            line.Margin = margin;
            line.StrokeStartLineCap = PenLineCap.Round;
            canvas1.Children.Add(line);
        }
    }
}
