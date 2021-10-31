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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
            CbStatusFill();
            CbPostFill();
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
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
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
                        MessageBoxResult result = MessageBox.Show("Внести ещё одного сотрудника? ", "Пользователь добавлен", MessageBoxButton.YesNo, MessageBoxImage.Question);
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
            bool? result = addPost.ShowDialog();
            switch (result)
            {
                default:
                    CbPostFill();
                    break;
            }
        }
        public void Delete()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {
                if (CbPost.SelectedIndex == -1)
                {
                    MessageBox.Show("Выберите должность, которую хотите удалить", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
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
        }

        private void BtnDelPost_Click(object sender, RoutedEventArgs e)
        {
            Delete();
            CbPostFill();
        }
        private void PhoneMask(string Phone)
        {
            var newVal = Phone;
            Phone = string.Empty;
            switch (newVal.Length)
            {
                case 1:
                    Phone = Regex.Replace(newVal, @"(\d{1})", "+7(___)___-__-__");
                    break;
                case 2:
                    Phone = Regex.Replace(newVal, @"(\d{1})(\d{0,3})", "+7($2__)___-__-__");
                    break;
                case 3:
                    Phone = Regex.Replace(newVal, @"(\d{1})(\d{0,3})", "+7($2_)___-__-__");
                    break;
                case 4:
                    Phone = Regex.Replace(newVal, @"(\d{1})(\d{0,3})", "+7($2)___-__-__");
                    break;
                case 5:
                    Phone = Regex.Replace(newVal, @"(\d{1})(\d{3})(\d{0,3})", "+7($2)$3__-__-__");
                    break;
                case 6:
                    Phone = Regex.Replace(newVal, @"(\d{1})(\d{3})(\d{0,3})", "+7($2)$3_-__-__");
                    break;
                case 7:
                    Phone = Regex.Replace(newVal, @"(\d{1})(\d{3})(\d{0,3})", "+7($2)$3-__-__");
                    break;
                case 8:
                    Phone = Regex.Replace(newVal, @"(\d{1})(\d{3})(\d{0,3})(\d{0,2})", "+7($2)$3-$4_-__");
                    break;
                case 9:
                    Phone = Regex.Replace(newVal, @"(\d{1})(\d{3})(\d{0,3})(\d{0,2})", "+7($2)$3-$4-__");
                    break;
                case 10:
                    Phone = Regex.Replace(newVal, @"(\d{1})(\d{3})(\d{0,3})(\d{0,2})(\d{0,2})", "+7($2)$3-$4-$5_");
                    break;
                case 11:
                    Phone = Regex.Replace(newVal, @"(\d{1})(\d{3})(\d{0,3})(\d{0,2})(\d{0,2})", "+7($2)$3-$4-$5");
                    break;
            }
            TbPhone.Text = Phone;
        }
        private string replacenumber()
        {
            string num = Regex.Replace(TbPhone.Text, @"[^0-9]","");
            return num;
        }
        private void changeCaretIndex(string Phone)
        {
            if (Phone.Length<=11)
            {
                PhoneMask(Phone);
            }
            if (Phone.Length <= 4)
            {
                TbPhone.CaretIndex = Phone.Length + 2;
            }
            else if (Phone.Length <= 7)
            {
                TbPhone.CaretIndex = Phone.Length + 3;
            }
            else if (Phone.Length <= 9)
            {
                TbPhone.CaretIndex = Phone.Length + 4;
            }
            else if (Phone.Length <= 11)
            {
                TbPhone.CaretIndex = Phone.Length + 5;
            }
        }
        private void TbPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            changeCaretIndex(replacenumber());
        }

        private void TbPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            changeCaretIndex(replacenumber() + e.Text);
            e.Handled = true;
        }

        private void TbPhone_GotFocus(object sender, RoutedEventArgs e)
        {
            changeCaretIndex(replacenumber());
        }
    }
}

