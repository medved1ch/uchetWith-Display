using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Data.SQLite;
using uchet.Connection;
using System.Data;

namespace uchet
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow

    {
      

        public MainWindow()
        {
            InitializeComponent();
            DisplayData();
        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddEmpl addEmpl = new AddEmpl();
            addEmpl.Owner = this;
            addEmpl.ShowDialog();


        }

        private void BtnTrack_Click(object sender, RoutedEventArgs e)
        {
            Tracker tracker = new Tracker();
            tracker.Owner = this;
            tracker.Show();
        }
        public void DisplayData()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {
                try
                {
                    connection.Open();
                    string query = $@"SELECT Employee.id, Employee.FirstName, Employee.SecondName, Employee.MiddleName, Employee.Dateofbirth, Employee.Phone, Position.Name AS Post, Stat.Status FROM Employee INNER JOIN Position on Employee.idPost = Position.id INNER JOIN Stat on Employee.idStatus = Stat.id";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    DataTable DT = new DataTable("Employee");
                    SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                    SDA.Fill(DT);
                    DGAllEmp.ItemsSource = DT.DefaultView;
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message);
                }


            }
        }
       
        public void Delete()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {
                try
                {
                    
                    foreach (var item in DGAllEmp.SelectedItems.Cast<DataRowView>())
                    {
                        string query1 = $@"DELETE FROM Employee WHERE id = " + item["ID"];
                        connection.Open();

                        SQLiteCommand cmd1 = new SQLiteCommand(query1, connection);
                        DataTable DT = new DataTable("Employee");
                        cmd1.ExecuteNonQuery();
                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message);
                }


            }
        }
        private void BtnUpd_Click(object sender, RoutedEventArgs e)
        {
            DisplayData();
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            Delete();
            DisplayData();
        }
    }
}

