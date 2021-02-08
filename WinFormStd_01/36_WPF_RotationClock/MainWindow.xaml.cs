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

namespace _36_WPF_RotationClock
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        // 시침, 분침, 초침의 각도 (12시 방향 기준, 시계 방향)
        private double hDeg;
        private double sDeg;
        private double mDeg;
        
        public MainWindow()
        {
            InitializeComponent();

            DrawFace(); // 시계판을 그린다 (눈금)
            MakeClockHands(); // 시계바늘을 만든다

            // 타이머 세팅
            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = new TimeSpan(0, 0, 0, 0, 10); // 10ms
            dt.Tick += Dt_Tick;
            dt.Start();
        }


        // 시계판 그리기
        private void DrawFace()
        {
            // 눈금 60개를 Line 배열로 만든다
            Line[] marking = new Line[60];
            int W = 600; // 시계 넗이

            for(int i = 0; i<60;i++)
            {
                marking[i] = new Line();
                marking[i].Stroke = Brushes.HotPink;
                marking[i].X1 = W / 2;
                marking[i].Y1 = 2;
                marking[i].X2 = W / 2;
                if(i%5==0) // 매 다섯번째 눈금은 큰 눈금으로 한다
                {
                    marking[i].StrokeThickness = 5;
                    marking[i].Y2 = 40;
                }
                else
                {
                    marking[i].StrokeThickness = 2;
                    marking[i].Y2 = 20;
                }

                // 눈금 하나 당 중심점을 기준으로 6도씩 회전(RotationTransform)
                RotateTransform rt = new RotateTransform(6 * i);
                rt.CenterX = 300; // 회전 중심점
                rt.CenterY = 300; // 회전 중심점
                marking[i].RenderTransform = rt;
                aClock.Children.Add(marking[i]);
            }
        }

        private void MakeClockHands()
        {
            int W = 600; // 시계판 넓이
            int H = 600; // 시계판 높이

            sHand.X1 = W / 2;
            sHand.Y1 = H / 2;
            sHand.X2 = W / 2;
            sHand.Y2 = 45;

            mHand.X1 = W / 2;
            mHand.Y1 = H / 2;
            mHand.X2 = W / 2;
            mHand.Y2 = 65;

            hHand.X1 = W / 2;
            hHand.Y1 = H / 2;
            hHand.X2 = W / 2;
            hHand.Y2 = 95;
        }
        private void Dt_Tick(object sender, EventArgs e)
        {
            DateTime currentTime = DateTime.Now;

            int h = currentTime.Hour;
            int m = currentTime.Minute;
            int s = currentTime.Second;
            hDeg = h % 12 * 30 + m * 0.5;
            mDeg = m * 6;
            sDeg = s * 6;

            // 시계바늘을 Remove & Add
            aClock.Children.Remove(hHand);
            RotateTransform hRt = new RotateTransform(hDeg);
            hRt.CenterX = hHand.X1;
            hRt.CenterY = hHand.Y1;
            hHand.RenderTransform = hRt;
            aClock.Children.Add(hHand);

            aClock.Children.Remove(mHand);
            RotateTransform mRt = new RotateTransform(mDeg);
            mRt.CenterX = mHand.X1;
            mRt.CenterY = mHand.Y1;
            mHand.RenderTransform = mRt;
            aClock.Children.Add(mHand);

            aClock.Children.Remove(sHand);
            RotateTransform sRt = new RotateTransform(sDeg);
            sRt.CenterX = sHand.X1;
            sRt.CenterY = sHand.Y1;
            sHand.RenderTransform = sRt;
            aClock.Children.Add(sHand);

            // 배꼽
            aClock.Children.Remove(center);
            aClock.Children.Add(center);
        }
    }
}
