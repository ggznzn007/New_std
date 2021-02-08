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



namespace _37_WPF_MonteCarloPI
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        int iCnt = 0;
        int oCnt = 0;

        DispatcherTimer timer = new DispatcherTimer();
        Random r = new Random();

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = new TimeSpan(10000); // 1ms
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            /*Rectangle rect = new Rectangle();
            rect.Width = 5;
            rect.Height = 5;*/
            Ellipse elps = new Ellipse();
            elps.Width = 4;
            elps.Height = 4;


            int x = r.Next(0, 400);
            int y = r.Next(0, 400);

            if ((x - 200) * (x - 200) + (y - 200) * (y - 200) <= 40000)
            {
                elps.Stroke = Brushes.Red;
                elps.Fill = Brushes.Red;
                iCnt++;
            }
            else
            {
                elps.Stroke = Brushes.Blue;
                elps.Fill = Brushes.Blue;
                oCnt++;
            }
            int count = iCnt + oCnt;
            double pi = (double)iCnt / count * 4;
            txtStatus.Text = "n = " + count + ", In: " + iCnt + "," +
                "Out:  " + oCnt + ", PI = " + pi;
            Canvas.SetLeft(elps, x);
            Canvas.SetTop(elps, y);
            canvas1.Children.Add(elps);
        }
    }
}
