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
        }

        private void btnBMI_Click(object sender, RoutedEventArgs e)
        {
            if(txtHeight.Text == ""||txtWeight.Text=="")
            {
                lblResult.Content = "키와 체중을 입력하세요";
                return;
            }
            double h = Convert.ToDouble(txtHeight.Text) / 100.0;
            double w = Double.Parse(txtWeight.Text);
            double bmi = w / (h * h);
            // Form에서는 Label.Text 인데, WPF에서는 label.Content
            lblResult.Content = string.Format("당신의 비만도는 {0:F2} 입니다", bmi);
        }
    }
}
