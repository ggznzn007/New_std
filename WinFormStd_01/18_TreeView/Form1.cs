using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _18_TreeView
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            TreeNode root = new TreeNode("영국의 왕");

            TreeNode stuart = new TreeNode("스튜어트 왕가");
            TreeNode hanover = new TreeNode("하노버 왕가");
            TreeNode sachsen = new TreeNode("작센코브르트고타 왕가");
            TreeNode windsor = new TreeNode("윈저 왕가");

            stuart.Nodes.Add("앤(1707~1714)");

            hanover.Nodes.Add("조지 1세(1714~1727)");
            hanover.Nodes.Add("조지 2세(1727~1760)");
            hanover.Nodes.Add("조지 3세(1760~1820)");
            hanover.Nodes.Add("조지 4세(1820~1830)");
            hanover.Nodes.Add("윌리엄 4세(1830~1837)");
            hanover.Nodes.Add("빅토리아(1837~1901)");

            sachsen.Nodes.Add("에드워드 7세(1901~1910)");

            windsor.Nodes.Add("조지 5세(1910~1936)");
            windsor.Nodes.Add("에드워드 8세(1936~1936)");
            windsor.Nodes.Add("조지 6세(1936~1952)");
            windsor.Nodes.Add("엘리자베스 2세(1952~현재)");

            root.Nodes.Add(stuart);
            root.Nodes.Add(hanover);
            root.Nodes.Add(sachsen);
            root.Nodes.Add(windsor);

            treeView1.Nodes.Add(root);
            treeView1.ExpandAll();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if(e.Node.Text == "앤(1707~1714)")
            {
                pictureBox1.Image = Bitmap.FromFile("../../Images/Anne.jpg");
                txtMemo.Text = "앤(Anne, 1665년 2월 6일 - 1714년 8월 1일, " +
                    "재위 1702년 4월 23일 ~ 1714년 8월 1일)은 잉글랜드-스코틀랜드" +
                    " 동군연합의 마지막 여왕이자 최초의 그레이트브리튼 왕국의 여왕 및 아일랜드의" +
                    " 여왕이다. 스튜어트 왕조의 마지막 군주이기도 하다. 브랜디를 좋아하여 " +
                    "‘브랜디 낸(Brandy Nan)’으로도 알려져 있다.";
            }
            else if(e.Node.Text=="조지 1세(1714~1727)")
            {
                pictureBox1.Image = Bitmap.FromFile("../../Images/King_George_I.jpg");
                txtMemo.Text = " 영국 하노버 왕가의 시조로 재위기간은 1714년 8월 1일부터" +
                    " 1727년 6월 11일까지다. 신성 로마 제국의 제후국 중 하나인" +
                    " 하노버 선제후국의 선제후이자 브라운슈바이크뤼네부르크 " +
                    "공작 에른스트 아우구스트와 팔츠의 조피 사이에서 태어났으며, " +
                    "영국 왕 제임스 1세의 외외증손자(딸의 외손자)가 된다.";
            }
        }
    }
}
