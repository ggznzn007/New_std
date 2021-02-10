using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _42_WF_Notepad
{
    public partial class Form1 : Form
    {
        private bool modifyFlag = false;
        private string fileName = "noname.txt";
        public Form1()
        {
            InitializeComponent();
            this.Text = fileName + " - myNotePad";
        }

        // RichTextBox TextChanged Event 처리 메소드
        private void txtMemo_TextChanged(object sender,EventArgs e)
        {
            modifyFlag = true;
        }

        private void 새로만들기ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
