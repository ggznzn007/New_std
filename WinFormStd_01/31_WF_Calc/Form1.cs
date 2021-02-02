using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _31_WF_Calc
{
    public partial class Form1 : Form
    {
        private double saved; // txtResult에 있는 값 저장
        private double memory; // 메모리에 저장된 값
        private char op = '\0'; // 현재 계산할 Operator
        private bool opFlag = false; // 연산자를 누른 후인지 체크하는 flag
        private bool memFlag; // 메모리 버튼을 누른 후인지 체크
        private bool percentFlag; // %처리를 위한 flag
        public Form1()
        {
            InitializeComponent();

            btnMC.Enabled = false;
            btnMR.Enabled = false;
        }

        // 소수점 처리
        private void btnDot_Click(object sender, EventArgs e)
        {
            if (txtResult.Text.Contains("."))
                return;
            else
                txtResult.Text += ".";
        }


    }
}
