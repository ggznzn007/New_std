﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;


namespace _46_WPF_DB_Login
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        string connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ggznz\reposit\New\WinFormStd_01\46_WPF_DB_Login\Login.mdf;Integrated Security=True";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection conn = new SqlConnection(connStr);
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                string sql = string.Format("SELECT COUNT(*) FROM LoginTable" +
                    "WHERE UserName ='{0}' AND Password.Password ='{1}'",
                    txtUserName.Text, txtPassword.Password);
                SqlCommand comm = new SqlCommand(sql, conn);
                int count = Convert.ToInt32(comm.ExecuteScalar());
                if (count == 1)
                {
                    MessageBox.Show("Login 성공");
                }
                else
                {
                    MessageBox.Show("Login 실패");
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
