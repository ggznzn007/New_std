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

namespace _30_WPF_Calc
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool newButton;
        private double savedValue;
        private char myOperator;
        public MainWindow()
        {
            InitializeComponent();

        }

            // 숫자 버튼 처리
        private void btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string number = btn.Content.ToString();
            if (txtResult.Text == "0" || newButton == true)
            {
                txtResult.Text = number;
                newButton = false;
            }
            else
                txtResult.Text = txtResult.Text + number;
        }

        // Operator 4개의 처리
        private void btnOp_Click(object sender, RoutedEventArgs e)
        {

        }

        // 소수점 처리
        private void Dot_Click(object sender, RoutedEventArgs e)
        {

        }

        // = 버튼 처리
        private void Equal_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
