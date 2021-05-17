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

namespace Calc_WPF
{
    
    public partial class MainWindow : Window
    {
        private bool newButton;
        private double savedValue;
        private char myOperator;
        public MainWindow()
        {
            InitializeComponent();
        }

        // 숫자 버튼의 처리
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

        //4개의 연산처리
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

        // "=" 버튼 처리
        private void Equal_Click(object sender, RoutedEventArgs e)
        {
            if (myOperator == '+')
            {
                txtResult.Text = (savedValue + double.Parse(txtResult.Text)).ToString();
            }
            else if(myOperator=='-')
            {
                txtResult.Text = (savedValue - double.Parse(txtResult.Text)).ToString();
            }
            else if (myOperator == '×')
            {
                txtResult.Text = (savedValue * double.Parse(txtResult.Text)).ToString();
            }
            else if (myOperator == '÷')
            {
                txtResult.Text = (savedValue / double.Parse(txtResult.Text)).ToString();
            }
        }
    }
}
