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

namespace _28_WPF_BMI
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            lblH.Foreground = Brushes.White;
            lblW.Foreground = Brushes.White;
            lblResult.Foreground = Brushes.White;

        }


        private void btnBMI_Click(object sender, RoutedEventArgs e)
        {
            if (txtHeight.Text == "" || txtWeight.Text == "")
            {
                lblResult.Content = "키와 체중을 입력하세요";
                return;
            }

            double h = Convert.ToDouble(txtHeight.Text) / 100.0;
            double w = Double.Parse(txtWeight.Text);
            double bmi = w / (h * h);
            string comment = null;
            if (bmi < 18.5)
                comment = "\n저체중입니다. \n많이 드세요.";
            else if (bmi < 23)
                comment = "\n정상체중입니다. \n계속 유지하세요.";
            else if (bmi < 25)
                comment = "\n경도비만입니다. \n적당한 운동이 필요해요.";
            else if (bmi < 30)
                comment = "\n비만입니다. \n운동을 열심히 하세요!!!";
            else
                comment = "\n고도비만입니다. \n그만 좀 쳐먹어!!! 운동 좀 해라!!!";

            // Form에서는 Label.Text 인데, WPF에서는 label.Content
            lblResult.Content = string.Format("당신의 비만도는 {0:F2}\n{1}", bmi, comment);

        }
        private void txtWeight_KeyDown(object sender, KeyEventArgs e)
        { // 엔터키를 눌렀을 때 버튼을 눌렀을 때와 같은 효과 나타내기 코드
            if (e.Key == Key.Enter)
            {
                this.btnBMI_Click(sender, e);
            }
        }
        private void btnClr_Click(object sender, RoutedEventArgs e)
        {
            if (txtHeight.Text == "" || txtWeight.Text == "")
            {
                lblResult.Content = "지울 내용이 없습니다";
                return;
            }
            else if (txtHeight.Text != "" || txtWeight.Text != "")
            {
                txtHeight.Text = "";
                txtWeight.Text = "";
                lblResult.Content = "";
            }
        }

        private void txtHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtHeight.Foreground = Brushes.Red;
        }

        private void txtWeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtWeight.Foreground = Brushes.Blue;
        }

    }
}
