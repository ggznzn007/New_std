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

namespace _44_WPF_MatchingGame
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        Button first;
        Button second;
        DispatcherTimer myTimer = new DispatcherTimer();
        int matched = 0;
        int[] rnd = new int[16]; // 랜덤숫자가 중복되는지 체크용
        public MainWindow()
        {
            InitializeComponent();
            BoardSet();
            myTimer.Interval = new TimeSpan(0, 0, 0, 0, 750); // 0.75초
            myTimer.Tick += MyTimer_Tick;
        }


        private void BoardSet() // 16버튼 초기화
        {
            for (int i = 0; i < 16; i++)
            {
                Button c = new Button();
                c.Background = Brushes.White;
                c.Margin = new Thickness(10);
                c.Content = MakeImage("../../Images/check.png");
                c.Tag = TagSet(); // 그림의 인덱스 세팅
                c.Click += C_Click;
                board.Children.Add(c);
            }
        }


        private Image MakeImage(string v) // v는 이미지 파일의 경로
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(v, UriKind.Relative);
            bi.EndInit();
            Image myImage = new Image();
            myImage.Margin = new Thickness(10);
            myImage.Stretch = Stretch.Fill;
            myImage.Source = bi;
            return myImage;
        }

        private int TagSet() // 중복되지 않는 i = 0 ~ 15 생성, i % 8 리턴
        {
            int i;
            Random r = new Random();
            while (true)
            {
                i = r.Next(16); // 0 ~ 15까지
                if (rnd[i] == 0)
                {
                    rnd[i] = 1;
                    break;
                }
            }
            return i % 8; // 태그는 0~7까지, 8개의 그림을 표시
        }
        private void C_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            string[] icon = {"딸기","레몬","배","블루베리","사과",
            "수박","파인애플", "포도"};
            btn.Content = MakeImage("../../Images/" + icon[(int)btn.Tag] + 
                ".png");

            // 두 개의 버튼을 수선대로 눌렀을 때 처리
            if (first == null) // 지금 누른 버튼이 첫 버튼이면
            {
                first = btn;
                return;
            }
            second = btn;

            if ((int)first.Tag == (int)second.Tag) // 매치가 되었을 때
            {
                first = null;
                second = null;
                matched += 2;
                if (matched >= 16)
                {
                    MessageBoxResult res = MessageBox.Show(
                        "성공! 다시하시겠습니까? " + MessageBoxButton.YesNo);
                    if (res == MessageBoxResult.Yes)
                        NewGame();
                    else
                        Close();
                }
                else // 매치가 되지 않았을 때 다시 덮어주기
                {
                    myTimer.Start();
                }
            }
        }
        private void MyTimer_Tick(object sender, EventArgs e)
        {
            myTimer.Stop();
            first.Content = MakeImage("../../Images/check.png");
            second.Content = MakeImage("../../Images/check.png");
            first = null;
            second = null;
        }

        private void NewGame()
        {
            for (int i = 0; i < 16; i++)
                rnd[i] = 0;
            board.Children.Clear();
            BoardSet();
            matched = 0;
        }
    }
}
