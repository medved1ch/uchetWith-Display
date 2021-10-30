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
    /// Логика взаимодействия для AddPost.xaml
    /// </summary>
    public partial class AddPost : MetroWindow
    {
        public AddPost()
        {
            InitializeComponent();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {

                if (String.IsNullOrEmpty(TbPost.Text))
                {
                    MessageBox.Show("Заполните поле", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    connection.Open();
                    string query = $@"INSERT INTO Position('Name') values (@Post)";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    try
                    {
                        cmd.Parameters.AddWithValue("@Post", TbPost.Text);
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
}
