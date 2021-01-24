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
                pictureBox1.Image = Bitmap.FromFile("../../Images/King_George_1.jpg");
                txtMemo.Text = "영국 하노버 왕가의 시조로 재위기간은 1714년 8월 1일부터" +
                    " 1727년 6월 11일까지다. 신성 로마 제국의 제후국 중 하나인" +
                    " 하노버 선제후국의 선제후이자 브라운슈바이크뤼네부르크 " +
                    "공작 에른스트 아우구스트와 팔츠의 조피 사이에서 태어났으며, " +
                    "영국 왕 제임스 1세의 외외증손자(딸의 외손자)가 된다.";
            }
            else if (e.Node.Text == "조지 2세(1727~1760)")
            {
                pictureBox1.Image = Bitmap.FromFile("../../Images/King_George_2.jpg");
                txtMemo.Text = "조지 2세(영어: George II of Great Britain, " +
                    "독일어: Georg II, 1683년 11월 10일 ~ 1760년 10월 25일)는 " +
                    "영국과 하노버 선제후국의 선제후(재위: 1727년 ~ 1760년)이다." +
                    " 조지 1세와 조피 도로테아의 아들이며 프로이센 왕비 하노버의" +
                    " 조피 도로테아의 오빠이다.";
            }
            else if (e.Node.Text == "조지 3세(1760~1820)")
            {
                pictureBox1.Image = Bitmap.FromFile("../../Images/King_George_3.jpg");
                txtMemo.Text = "조지 3세(George William Frederick, 1738년 6월 4일 – " +
                    "1820년 1월 29일)는 1760년 10월 25일부터 그레이트브리튼 왕국과 아일랜드의 국왕이었고," +
                    " 1800년 연합법이 제정된 이후 1801년 1월 1일부터 1820년 사망할 때까지" +
                    " 그레이트브리튼 아일랜드 연합 왕국의 왕이 되었다. 영국의 왕으로 재임하는 동안" +
                    " 그는 신성 로마 제국에 속한 브라운슈바이크뤼네부르크 선제후국의 선제후이자 공작이었으며," +
                    " 1814년 10월 12일 하노버 왕국이 수립되면서 하노버의 국왕을 겸임했다." +
                    " 조지 3세는 하노버가의 3번째 영국 군주였지만, 이전의 국왕과는 달리 그는" +
                    " 제1언어로 영어를 썼으며, 영국에서 태어난 군주였고,[1] " +
                    "하노버를 한번도 방문하지 않았다.[2]";
            }
            else if (e.Node.Text == "조지 4세(1820~1830)")
            {
                pictureBox1.Image = Bitmap.FromFile("../../Images/King_George_4.jpg");
                txtMemo.Text = "조지 4세는 영국 국왕 조지 3세와 샬럿 왕비의 장남으로 태어났다." +
                    " 조지 4세는 섭정시대 유행에 일조한 사치스러운 생활을 하였다." +
                    " 조지 4세는 새로운 여가 방식, 스타일, 취향을 후원하기도 했다." +
                    " 조지 4세는 존 나쉬에게 브라이튼의 로열 파빌리언 건축과 버킹엄 궁전 개보수를," +
                    " 제프리 와이엇빌 경에게는 윈저 성의 개보수를 의뢰하였다. " +
                    "조지 4세의 매력과 교양은 그로 하여금 잉글랜드의 첫번째 신사라는 " +
                    "칭호를 얻게끔 했으나, 그의 방종한 생활 방식, " +
                    "부모와 아내 브라운슈바이크의 캐럴라인과의 나쁜 관계는 사람들의 경멸을" +
                    " 샀으며 왕실의 품격을 떨어뜨렸다.";
            }
            else if (e.Node.Text == "윌리엄 4세(1830~1837)")
            {
                pictureBox1.Image = Bitmap.FromFile("../../Images/King_William_4.jpg");
                txtMemo.Text = "윌리엄 4세(William IV, 1765년 8월 21일 - 1837년 6월 20일)는" +
                    " 영국의 왕(재위 1830년 6월 26일 - 1837년 6월 20일)으로," +
                    " 하노버 왕가가 배출한 다섯 번째 군주이다." +
                    " 본명은 윌리엄 헨리(William Henry)이며 즉위 전에는 클래런스와 " +
                    "세인트앤드루스 공작 윌리엄(Prince William, Duke of Clarence" +
                    " and St Andrews)으로 불렸다. 조지 3세와 샬럿 소피아의 세 번째 아들이다.";
            }
            else if (e.Node.Text == "빅토리아(1837~1901)")
            {
                pictureBox1.Image = Bitmap.FromFile("../../Images/Queen_Victoria.jpg");
                txtMemo.Text = "빅토리아(Queen Victoria, 1819년 5월 24일 ~ 1901년 1월 22일)" +
                    "는 그레이트브리튼 아일랜드 연합 왕국의 왕" +
                    " (재위 : 1837년 6월 20일 ~ 1901년 1월 22일)이다." +
                    " 현 영국 국왕인 엘리자베스 2세의 고조모이다.";
            }
            else if (e.Node.Text == "에드워드 7세(1901~1910)")
            {
                pictureBox1.Image = Bitmap.FromFile("../../Images/Edward_7.jpg");
                txtMemo.Text = "에드워드 7세(Edward VII, 1841년 11월 9일 ~ 1910년 5월 6일)는" +
                    " 영국의 왕(재위 : 1901년 1월 22일 ~ 1910년 5월 6일)이자" +
                    " 인도 제국의 황제이다. 본명은 앨버트 에드워드 웨틴(Albert Edward Wettin)이다." +
                    " 작센코부르크고타 왕가의 첫번째 영국 군주이다.";
            }
            else if (e.Node.Text == "조지 5세(1910~1936)")
            {
                pictureBox1.Image = Bitmap.FromFile("../../Images/King_George_5.jpg");
                txtMemo.Text = "조지 5세(영어: George V, 1865년 6월 3일 ~ 1936년 1월 20일)는" +
                    " 그레이트브리튼 아일랜드 연합 왕국의 왕이자 그레이트브리튼 및 북아일랜드" +
                    " 연합 왕국 및 인도 제국의 황제이다. (재위 : 1910년 5월 6일 - 1936년 1월 20일)." +
                    "현재 영국 국왕 엘리자베스 2세의 할아버지이다.";
            }
            else if (e.Node.Text == "에드워드 8세(1936~1936)")
            {
                pictureBox1.Image = Bitmap.FromFile("../../Images/Edward_8.jpg");
                txtMemo.Text = "에드워드 8세(Edward VIII, 1894년 6월 23일 ~ 1972년 5월 28일)는" +
                    " 1936년 1월 20일부터 1936년 12월 11일까지 그레이트브리튼 및 북아일랜드" +
                    " 연합왕국의 국왕 겸 인도 제국의 황제(영어: King of the United Kingdom" +
                    " of Great Britain and Northern Ireland and Emperor of India)였다." +
                    " 본명은 에드워드 앨버트 크리스천 조지 앤드루 패트릭 데이비드 윈저" +
                    "(영어: Edward Albert Christian George Andrew Patrick David Windsor)" +
                    "이다. 미국 출신의 이혼여성인 월리스 워필드 심슨(Wallis Warfield Simpson)과" +
                    " 결혼하고자 했으나 많은 사람들이 반대하자 왕위를 남동생인 조지 6세에게 물려주고" +
                    " 에드워드 8세에서 윈저 공으로 신분이 바뀐 사건으로 유명하다. " +
                    "정치적으로는 제2차 세계 대전 당시 적성국가인 나치 독일을 지지하여 " +
                    "영국 왕실에서 에드워드 8세 부부를 프랑스에서 영국으로 소환하였다.";
            }
            else if (e.Node.Text == "조지 6세(1936~1952)")
            {
                pictureBox1.Image = Bitmap.FromFile("../../Images/King_George_6.jpg");
                txtMemo.Text = "조지 6세(George VI, 1895년 12월 14일 ~ 1952년 2월 6일)는" +
                    " 1936년 12월 11일에서 1952년 2월 6일까지 그레이트브리튼 및 아일랜드의 왕" +
                    " 겸 인도 제국의 황제(King of the United Kingdom of Great Britain" +
                    " and Ireland and Emperor of India)였다." +
                    " 본명은 앨버트 프레데릭 아서 조지 윈저(Albert Frederick Arthur" +
                    " George Windsor)이다.";
            }
            else if (e.Node.Text == "엘리자베스 2세(1952~현재)")
            {
                pictureBox1.Image = Bitmap.FromFile("../../Images/Queen_Elizabeth_2.jpg");
                txtMemo.Text = "엘리자베스 2세(영어: Elizabeth II, 1926년 4월 21일 ~ )는 " +
                    "영국을 포함한 16개국(영국 연방 왕국)과 기타 국외 영토와 보호령의 왕이다." +
                    " 본명은 엘리자베스 알렉산드라 메리(영어: Elizabeth Alexandra Mary)이다." +
                    " 호칭은 ‘영국 연방의 엘리자베스 2세 여왕 폐하(영어: Her Majesty Queen" +
                    " Elizabeth II of the United Kingdom)’이다. " +
                    "그는 1952년 2월 서거한 부왕 조지 6세의 뒤를 이어 왕위에 올랐다." +
                    "또한, 영국 외에도 (실질적으로는 연방 총독이 대표하고 있지만) 캐나다," +
                    " 오스트레일리아, 뉴질랜드, 자메이카, 바베이도스, 바하마, 그레나다," +
                    " 파푸아뉴기니, 솔로몬 제도, 투발루, 세인트루시아, 세인트 빈센트 그레나딘," +
                    " 벨리즈, 앤티가 바부다, 세인트키츠 네비스 등의 왕이기도 하다." +
                    " 왕이 군림하는 영국 연방 16개국의 총인구는 보호령까지" +
                    " 포함해서 1억 2900만 명이 넘는다. 따라서 이론적으로 " +
                    "그는 막강한 권세를 가진 셈이지만 정치적 문제에 개입하지는 않는다."+
                    "엘리자베스 2세는 그 밖에도 영국 성공회의 최고 치리자[1], 노르망디 공작," +
                    " 랭커스터 공작, 맨 섬의 영주, 피지의 최고 추장, 함대 사령장관 등의" +
                    " 다양한 직함을 보유하고 있다."+
                    "현재 전 세계 군주들 가운데서 최고령이며 최장기간 재위한 군주이다." +
                    " 2012년 6월에 재위 60주년을 맞은 엘리자베스 2세는 64년 동안 재위했던" +
                    " 빅토리아 여왕에 이어 영국 역사상 두 번째로 다이아몬드 주빌리를 맞이하는" +
                    " 군주가 되었다.군주가 96세가 되는 해인 2022년 2월 6일에는 군주의 재위" +
                    " 70주년을 기념하는 플래티넘 주빌리를 맞게 되는데, 건강 상태가 매우 좋기 때문에" +
                    " 이것도 가능할 것으로 예상한다.[2] 즉위 60주년을 맞아 실시된 영국의 " +
                    "가장 위대한 국왕이 누구인지 묻는 설문조사에서는 빅토리아 여왕, " +
                    "엘리자베스 1세를 제치고 1위에 오르기도 했다." +
                    " [3] 또한, 엘리자베스 2세 여왕은 2015년 9월 9일 오후 5시 30분을 기점으로 " +
                    "빅토리아 여왕의 통치 기간을 넘어서 영국의 최장수 통치자가 되었다.";
            }
        }
    }
}
