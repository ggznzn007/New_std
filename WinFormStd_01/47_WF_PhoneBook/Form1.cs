using System;
using System.Windows.Forms;
using System.Data.OleDb;

namespace _47_WF_PhoneBook
{
    public partial class Form1 : Form
    {
        OleDbConnection conn = null;
        OleDbCommand comm = null;
        OleDbDataReader reader = null;

        string connStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\ggznz\reposit\New\WinFormStd_01\47_WF_PhoneBook\Student.accdb";
        public Form1()
        {
            InitializeComponent();
            DisplayStudents();
        }

        private void ConnectionOpen()
        {
            if(conn==null)
            {
                conn = new OleDbConnection(connStr);
                conn.Open();
            }
        }

        private void ReadAndAddToListBox()
        {
            reader = comm.ExecuteReader();
            while(reader.Read())
            {
                string x = "";
                x += reader["ID"] + "\t";
                x += reader["SID"] + "\t";
                x += reader["SName"] + "\t";
                x += reader["Phone"];

                listBox1.Items.Add(x);
            }
            reader.Close();
        }

        private void ConnectionClose()
        {
            conn.Close();
            conn = null;
        }

        private void DisplayStudents()
        {
            ConnectionOpen();


                       string sql = "SELECT * FROM StudentTable";
            comm = new OleDbCommand(sql, conn);

            ReadAndAddToListBox();
            ConnectionClose();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lb = sender as ListBox;

            if (lb.SelectedItem == null)
                return;

            string[] s = lb.SelectedItem.ToString().Split('\t');
            txtID.Text = s[0];
            txtSId.Text = s[1];
            txtSName.Text = s[2];
            txtPhone.Text = s[3];
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (txtSName.Text == "" || txtPhone.Text == "" || txtSId.Text == "")
                return;

            ConnectionOpen();

            string sql = string.Format("insert into " +
                "StudentTable(SId, SName, Phone) VALUES({0}, '{1}','{2}')",
                txtSId.Text, txtSName.Text, txtPhone.Text);

            comm = new OleDbCommand(sql, conn);
            if (comm.ExecuteNonQuery() == 1)
                MessageBox.Show("삽입성공!");

            ConnectionClose();
            listBox1.Items.Clear();
            DisplayStudents();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSName.Text == "" && txtPhone.Text == "" && txtSId.Text == "")
                return;

            ConnectionOpen();

            string sql = "";
            if (txtSId.Text != "")
                sql = string.Format("SELECT * FROM StudentTable WHERE SID={0}",
                    txtSId.Text);
            else if (txtSName.Text != "")
                sql = string.Format(
                    "SELECT * FROM StudentTable WHERE Phone='{0}'", txtSName.Text);
            else if (txtPhone.Text != "")
                sql = string.Format(
                    "SELECT * FROM StudentTable WHERE Phone='{0}'", txtPhone.Text);

            listBox1.Items.Clear();
            comm = new OleDbCommand(sql, conn);
            ReadAndAddToListBox();
            ConnectionClose();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {

        }

        private void btnViewAll_Click(object sender, EventArgs e)
        {

        }
    }
}
