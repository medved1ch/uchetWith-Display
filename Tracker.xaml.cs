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
using System.Data;
using System.Data.SQLite;
using uchet.Connection;

namespace uchet
{
    /// <summary>
    /// Логика взаимодействия для Tracker.xaml
    /// </summary>
    public partial class Tracker : MetroWindow
    {
        DataTable dt1 = new DataTable("Employee");
       
        public Tracker()
        {
            InitializeComponent();
            CbFill();
            DisplayData();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public void DisplayData()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {
                try
                {
                    
                    connection.Open();
                    string query3 = $@"SELECT Tracking.id, Tracking.TimeEntrance AS TimeEn, Tracking.TimeExit AS TimeEx, Tracking.DateEntrance AS DateEn, Tracking.DateExit AS DateEx, Employee.FirstName, Employee.SecondName FROM Tracking INNER JOIN Employee on Tracking.idEmployee = Employee.id";
                    SQLiteCommand cmd3 = new SQLiteCommand(query3, connection);
                    DataTable dt2 = new DataTable("Tracking");
                    SQLiteDataAdapter SDA3 = new SQLiteDataAdapter(cmd3);
                    SDA3.Fill(dt2);
                    DGTrack.ItemsSource = dt2.DefaultView;
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message);
                }


            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Entrance();
            DisplayData();
        }
        public void CbFill()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {
                try
                {
                    connection.Open();
                    string query2 = $@"SELECT id, SecondName || ' ' || FirstName AS FullName FROM Employee";
                    SQLiteCommand cmd2 = new SQLiteCommand(query2, connection);
                    SQLiteDataAdapter SDA2 = new SQLiteDataAdapter(cmd2);
                    SDA2.Fill(dt1);
                    CbEmp.ItemsSource = dt1.DefaultView;
                    CbEmp.DisplayMemberPath = "FullName";
                    CbEmp.SelectedValuePath = "id";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public void Entrance()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {
                if (CbEmp.SelectedIndex == -1)
                {
                    MessageBox.Show("Выберите сотрудника", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    int IdEmp;
                    bool NameStat = int.TryParse(CbEmp.SelectedValue.ToString(), out IdEmp);
                    connection.Open();
                    string query = $@"INSERT INTO Tracking('idEmployee','TimeEntrance', 'DateEntrance') values ('{IdEmp}', @Time, @Date)";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    try
                    {
                        cmd.Parameters.AddWithValue("@Time", DateTime.Now.TimeOfDay.ToString("hh\\:mm"));
                        cmd.Parameters.AddWithValue("@Date", DateTime.Now.Date.ToString("MM/dd/yyyy"));
                        cmd.ExecuteNonQuery();

                    }
                    catch (SQLiteException ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }
        public void Exit()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {
                if (CbEmp.SelectedIndex == -1)
                {
                    MessageBox.Show("Выберите сотрудника", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    int IdEmp;
                bool NameStat = int.TryParse(CbEmp.SelectedValue.ToString(), out IdEmp);
                connection.Open();
                string query = $@"INSERT INTO Tracking('idEmployee','TimeExit', 'DateExit') values ('{IdEmp}', @Time, @Date)";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                try
                {
                    cmd.Parameters.AddWithValue("@Time", DateTime.Now.TimeOfDay.ToString("hh\\:mm"));
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now.Date.ToString("MM/dd/yyyy"));
                    cmd.ExecuteNonQuery();

                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        }
        private void BtnEx_Click(object sender, RoutedEventArgs e)
        {
            Exit();
            DisplayData();
        }
    }
    }
