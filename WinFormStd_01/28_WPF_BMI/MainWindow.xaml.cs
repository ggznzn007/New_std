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
                comment = "저체중입니다. 많이 드세요.";
            else if (bmi < 23)
                comment = "정상체중입니다. 계속 유지하세요.";
            else if (bmi < 25)
                comment = "경도비만입니다. 적당한 운동이 필요해요.";
            else if (bmi < 30)
                comment = "비만입니다. 운동을 열심히 하세요!!!";
            else
                comment = "고도비만입니다. 그만 좀 쳐먹어!!! 운동 좀 해라!!!";

            // Form에서는 Label.Text 인데, WPF에서는 label.Content
            lblResult.Content = string.Format("당신의 비만도는 {0:F2},\n{1}", bmi, comment);

        }
    }
}
