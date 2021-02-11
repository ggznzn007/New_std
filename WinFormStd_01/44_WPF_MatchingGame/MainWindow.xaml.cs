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
        Button Second;
        DispatcherTimer myTimer = new DispatcherTimer();
        int matched = 0;
        int[] rnd = new int[16]; // 랜덤숫자가 중복되는지 체크용
        public MainWindow()
        {
            InitializeComponent();

        }

        private void BoardSet() // 16버튼 초기화
        {
            for(int i =0;i<16;i++)
            {
                Button c = new Button();
                c.Background = Brushes.White;
                c.Margin = new Thickness(10);
            }
        }
    }
}
