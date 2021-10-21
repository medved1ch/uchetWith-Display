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
        private void TbPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
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
        private void BtnAdd_Click( object sender, RoutedEventArgs e)
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
                    
                    string query = $@"INSERT INTO Employee('FirstName', 'SecondName', 'MiddleName', 'Dateofbirth', 'Phone', 'idPost', 'idStatus') values (@FN, @SN, @MN, @Date, @Phone, '{IdPost}', '{IdStatus}')";
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
                        MessageBoxResult result = MessageBox.Show("Внести нового сотрудника? ", "Пользователь добавлен", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            TbFN.Clear();
                            TbSN.Clear();
                            TbMN.Clear();
                            TbPhone.Clear();
                            DpB.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
                            TbPhone.Clear();
                        }
                        else
                        {
                            this.Close();
                        }
                        
                    }
                    catch (SQLiteException ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }

                }
            }

        private void BtnAddPost_Click(object sender, RoutedEventArgs e)
        {
            AddPost addPost = new AddPost();
            addPost.Owner = this;
            addPost.ShowDialog();
        }
        public void Delete()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {
                int IdPost;
                bool NamePost = int.TryParse(CbPost.SelectedValue.ToString(), out IdPost);
                try
                {
                        string query1 = $@"DELETE FROM Position WHERE id = '{IdPost}'";
                        connection.Open();
                        SQLiteCommand cmd1 = new SQLiteCommand(query1, connection);
                        DataTable DT = new DataTable("Position");
                        cmd1.ExecuteNonQuery();
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message);
                }


            }
        }

        private void BtnDelPost_Click(object sender, RoutedEventArgs e)
        {
            Delete();
            CbFill();
        }
    }
    }


