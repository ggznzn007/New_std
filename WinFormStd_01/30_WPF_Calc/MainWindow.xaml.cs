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
            txtResult.Foreground = Brushes.White;
            
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
            Button btn = sender as Button;

            savedValue = double.Parse(txtResult.Text);
            myOperator = btn.Content.ToString()[0];
            newButton = true;
        }

        // 소수점 처리
        private void Dot_Click(object sender, RoutedEventArgs e)
        {
            if (txtResult.Text.Contains(".") == false)
                txtResult.Text += ".";
        }

        // = 버튼으로 계산 처리
        private void Equal_Click(object sender, RoutedEventArgs e)
        {
            if (myOperator == '+')
                txtResult.Text = (savedValue +
                    double.Parse(txtResult.Text)).ToString();
            else if (myOperator == '-')
                txtResult.Text = (savedValue -
                    double.Parse(txtResult.Text)).ToString();
            else if (myOperator == '×')
                txtResult.Text = (savedValue *
                    double.Parse(txtResult.Text)).ToString();
            else if (myOperator == '÷')
                txtResult.Text = (savedValue /
                    double.Parse(txtResult.Text)).ToString();
            else if (myOperator == '^')
                txtResult.Text = ((savedValue *
                    double.Parse(txtResult.Text) * savedValue * double.Parse(txtResult.Text))).ToString();
            else if (myOperator == '%')
                txtResult.Text = (savedValue %
                    double.Parse(txtResult.Text)).ToString();
        }

        private void Equal_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
            {
                Equal_Click(sender, e);
            }
        }
        private void Power_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            savedValue = double.Parse(txtResult.Text);
            myOperator = btn.Content.ToString()[0];
            newButton = true;
        }

        private void btnCE_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text = "0";
        }

        private void btnC_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text = "0";
        }

        private void btnRes_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            savedValue = double.Parse(txtResult.Text);
            myOperator = btn.Content.ToString()[0];
            newButton = true;
        }

       

        
    }
}
