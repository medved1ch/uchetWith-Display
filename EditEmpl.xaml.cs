﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using uchet.Connection;

namespace uchet
{
    /// <summary>
    /// Логика взаимодействия для EditEmpl.xaml
    /// </summary>
    public partial class EditEmpl : MetroWindow
    {
        DataTable dt1 = new DataTable("Position");
        DataTable dt2 = new DataTable("Stat");
        string id;
        public EditEmpl(DataRowView drv)
        {
            InitializeComponent();
            CbStatusFill();
            CbPostFill();
            TbFN.Text = drv["FN"].ToString();
            TbSN.Text = drv["SN"].ToString();
            TbMN.Text = drv["MN"].ToString();
            TbPhone.Text = drv["Phone"].ToString();
            DpB.Text = drv["DoB"].ToString();
            CbPost.Text = drv["Post"].ToString();
            CbStat.Text = drv["Status"].ToString();
            id = drv["id"].ToString();
            
        }
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
        public void CbStatusFill()
        {
            dt2.Clear();
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {
                try
                {
                    connection.Open();
                    string query2 = $@"SELECT * FROM Stat";
                    SQLiteCommand cmd2 = new SQLiteCommand(query2, connection);
                    SQLiteDataAdapter SDA2 = new SQLiteDataAdapter(cmd2);
                    SDA2.Fill(dt2);
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
        public void CbPostFill()
        {
            dt1.Clear();
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {
                try
                {
                    connection.Open();
                    string query1 = $@"SELECT * FROM Position";
                    SQLiteCommand cmd1 = new SQLiteCommand(query1, connection);
                    SQLiteDataAdapter SDA1 = new SQLiteDataAdapter(cmd1);
                    SDA1.Fill(dt1);
                    CbPost.ItemsSource = dt1.DefaultView;
                    CbPost.DisplayMemberPath = "Name";
                    CbPost.SelectedValuePath = "id";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public void UpdateEmployee()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {
                connection.Open();
                if (String.IsNullOrEmpty(TbSN.Text) || String.IsNullOrEmpty(TbFN.Text) || String.IsNullOrEmpty(DpB.Text) || String.IsNullOrEmpty(TbPhone.Text) || CbStat.SelectedIndex == -1 || CbPost.SelectedIndex == -1)
                {
                    MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    int IdStatus, IdPost;
                    bool NameStat = int.TryParse(CbStat.SelectedValue.ToString(), out IdStatus);
                    bool NamePost = int.TryParse(CbPost.SelectedValue.ToString(), out IdPost);
                    string query = $@"UPDATE Employee SET FirstName=@FN, SecondName=@SN, MiddleName=@MN, Dateofbirth=@Date, Phone=@Phone, idPost='{IdPost}', idStatus='{IdStatus}' WHERE id='{id}' ";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    try
                    {
                        cmd.Parameters.AddWithValue("@SN", TbSN.Text);
                        cmd.Parameters.AddWithValue("@FN", TbFN.Text);
                        cmd.Parameters.AddWithValue("@MN", TbMN.Text);
                        cmd.Parameters.AddWithValue("@Date", DpB.Text);
                        cmd.Parameters.AddWithValue("@Phone", TbPhone.Text);
                        cmd.ExecuteNonQuery();

                    }
                    catch (SQLiteException ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            UpdateEmployee();
            this.Close();
        }
    }
}
        
    

