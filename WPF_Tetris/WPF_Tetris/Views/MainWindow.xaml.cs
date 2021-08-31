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
using WPF_Tetris.ViewModels;

namespace WPF_Tetris.Views
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainWindowViewModel();
            InitializeComponent();
            
        }

        //<MediaElement Source="C:/Users/ggznz/reposit/Tetris-master/WPF_Tetris/WPF_Tetris/BGM/Tetris02.mp3"/>
        /*public void PlayBGM()
        {
            MediaElement bgm = new MediaElement();
            bgm.Source = new Uri(@"C:/Users/ggznz/reposit/Tetris-master/WPF_Tetris/WPF_Tetris/BGM/Tetris02.mp3", UriKind.Absolute);
            bgm.LoadedBehavior = MediaState.Manual;
            //bgm.Position = TimeSpan.Zero;
            bgm.Play();            
            bgm.Stop();
        }*/

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)DataContext).EnterGame();
            
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)DataContext).StopTimer();
        
        }



        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            MainWindowViewModel mv = (MainWindowViewModel)DataContext;
            if (!mv.Is_gaming) return;
            switch (e.Key)
            {
                case Key.Left:
                    mv.BlockMoveLeft();
                    break;
                case Key.Right:
                    mv.BlockMoveRight();
                    break;
                case Key.Space:
                    mv.Block_drop();
                    break;
                case Key.Down:
                    mv.Block_down();
                    break;
                case Key.Up:
                    mv.BlockRotate();
                    break;
            }
        }
    }
}
