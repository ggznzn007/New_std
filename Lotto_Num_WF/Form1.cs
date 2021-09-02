using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lotto_Num_WF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 텍스트 박스 초기화
            textBox1.Text = "";
            textBox1.Text = Create_LottoNum();
        }

        private string Create_LottoNum()
        {
            string strTmp = "";
            string strNum = "";
            string strBonusNum = "";
            int[] tmpNum = new int[7];

            for (int i = 0; i < 7; i++)
            {
                Random rnd = new Random();
                // 1부터 45 숫자 중 선택
                tmpNum[i] = rnd.Next(1, 45);
                for (int j = 0; j < i; j++)
                {
                    if (tmpNum[i] == tmpNum[j])
                    {
                        i = i - 1;
                    }
                }
            }
            strTmp = string.Join(", ", tmpNum);
            // 로또 번호
            strNum = strTmp.Substring(0, strTmp.LastIndexOf(',') - 1);
            // 끝에 있는 보너스 번호
            strBonusNum = strTmp.Substring(strTmp.LastIndexOf(',') + 1, 2).Trim();
            return "로또 번호 : " + strNum + ", 보너스번호 : " + strBonusNum;
        }
    }
}
