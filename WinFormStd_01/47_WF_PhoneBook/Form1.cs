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
    }
}
