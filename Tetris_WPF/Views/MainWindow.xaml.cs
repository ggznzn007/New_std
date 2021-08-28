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
using Tetris_WPF.ViewModels;

namespace Tetris_WPF.Views
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainWindowViewModels();
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModels)DataContext).EnterGame();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            MainWindowViewModels mv = (MainWindowViewModels)DataContext;
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
