using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _13_ListBox
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listBox2.Items.Add("미국, 뉴욕");
            listBox2.Items.Add("러시아, 모스크바");
            listBox2.Items.Add("중국, 베이징");
            listBox2.Items.Add("영국, 런던");
            listBox2.Items.Add("독일, 베를린");
            listBox2.Items.Add("프랑스, 파리");
            listBox2.Items.Add("일본, 도쿄");
            listBox2.Items.Add("이스라엘, 예루살렘");
            listBox2.Items.Add("사우디아라비아, 리야드");
            listBox2.Items.Add("UAE, 아부다비");
            listBox2.Items.Add("한국, 서울");
            listBox2.Items.Add("태국, 방콕");
            listBox2.Items.Add("필리핀, 마닐라");
            listBox2.Items.Add("방글라데시, 다카");
            listBox2.Items.Add("캐나다, 오타와");
            listBox2.Items.Add("뉴질랜드, 웰링턴");
           

            List<string> lstGDP = new List<string> { "미국","러시아","중국",
                "영국","독일","프랑스","일본","이스라엘","사우디아라비아","UAE",
            "한국","태국","필리핀","인도","방글라데시","캐나다","뉴질랜드"};
            listBox3.DataSource = lstGDP;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lst = sender as ListBox;
            txtSIndex1.Text = lst.SelectedIndex.ToString();
            txtSItem1.Text = lst.SelectedItem.ToString();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lst = sender as ListBox;
            txtSIndex2.Text = lst.SelectedIndex.ToString();
            txtSItem2.Text = lst.SelectedItem.ToString();
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lst = sender as ListBox;
            txtSIndex3.Text = lst.SelectedIndex.ToString();
            txtSItem3.Text = lst.SelectedItem.ToString();
        }
    }
}
