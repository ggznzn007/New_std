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
        }

        // 시계판 그리기
        private void DrawFace()
        {
            // 눈금 60개를 Line 배열로 만든다
            Line[] marking = new Line[60];
            int W = 300; // 시계 넗이

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
                    marking[i].Y2 = 20;
                }
                else
                {
                    marking[i].StrokeThickness = 2;
                    marking[i].Y2 = 10;
                }

                // 눈금 하나 당 중심점을 기준으로 6도씩 회전(RotationTransform)
                RotateTransform rt = new RotateTransform(6 * i);
                rt.CenterX = 150; // 회전 중심점
            }
        }
    }
}
