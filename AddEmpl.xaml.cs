using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Data.SQLite;
using uchet.Connection;
using System.Data;

namespace uchet
{
    /// <summary>
    /// Логика взаимодействия для AddEmpl.xaml
    /// </summary>
    public partial class AddEmpl : MetroWindow
    {
        DataTable dt1 = new DataTable("Position");
        DataTable dt2 = new DataTable("Stat");

        public AddEmpl()
        {
            InitializeComponent();
            CbFill();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
        public void CbFill()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {
                try
                {
                    connection.Open();
                    string query1 = $@"SELECT * FROM Position";
                    string query2 = $@"SELECT * FROM Stat";
                    SQLiteCommand cmd1 = new SQLiteCommand(query1, connection);
                    SQLiteCommand cmd2 = new SQLiteCommand(query2, connection);
                    SQLiteDataAdapter SDA1 = new SQLiteDataAdapter(cmd1);
                    SQLiteDataAdapter SDA2 = new SQLiteDataAdapter(cmd2);
                    SDA1.Fill(dt1);
                    SDA2.Fill(dt2);
                    CbPost.ItemsSource = dt1.DefaultView;
                    CbPost.DisplayMemberPath = "Name";
                    CbPost.SelectedValuePath = "id";
                    CbStat.ItemsSource = dt2.DefaultView;
                    CbStat.DisplayMemberPath = "Status";
                    CbStat.SelectedValuePath = "id";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
     }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {
                int IdStatus, IdPost;
                bool NameStat = int.TryParse(CbStat.SelectedValue.ToString(), out IdStatus);
                bool NamePost= int.TryParse(CbPost.SelectedValue.ToString(), out IdPost);
                connection.Open();
                string query = $@"INSERT INTO Employee('FirstName', 'SecondName', 'MiddleName', 'Dateofbirth', 'Phone', 'idPost', 'idStatus') values (@SN, @FN, @MN, @Date, @Phone, '{IdPost}', '{IdStatus}')";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                try
                {
                    cmd.Parameters.AddWithValue("@SN", TbSN.Text);
                    cmd.Parameters.AddWithValue("@FN", TbFN.Text);
                    cmd.Parameters.AddWithValue("@MN", TbMN.Text);
                    cmd.Parameters.AddWithValue("@Date", DpB.Text);
                    cmd.Parameters.AddWithValue("@Phone", TbPhone.Text);
                    /*cmd.Parameters.AddWithValue("@Post", CbPost.SelectedItem);
                    cmd.Parameters.AddWithValue("@Stat", CbStat.SelectedItem);*/
                    cmd.ExecuteNonQuery();
                    this.Close();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
    }
}

