﻿using System;
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
using Excel = Microsoft.Office.Interop.Excel;

namespace uchet
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow

    {
        DataTable dt1 = new DataTable("Position");
        DataTable dt2 = new DataTable("Stat");
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
            tracker.ShowDialog();
        }
        public void DisplayData()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {
                try
                {
                    connection.Open();
                    string query = $@"SELECT Employee.id, Employee.FirstName AS FN, Employee.SecondName AS SN, Employee.MiddleName AS MN, Employee.Dateofbirth AS DoB, Employee.Phone AS Phone, Position.Name AS Post, Stat.Status FROM Employee INNER JOIN Position on Employee.idPost = Position.id INNER JOIN Stat on Employee.idStatus = Stat.id";
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

        private void BtnExp_Click(object sender, RoutedEventArgs e)
        {
            Excel.Application excel = new Excel.Application();
            excel.Visible = true;
            Excel.Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            Excel.Worksheet sheet1 = (Excel.Worksheet)workbook.Sheets[1];
            for (int j = 0; j < DGAllEmp.Columns.Count; j++)
            {
                Excel.Range myRange = (Excel.Range)sheet1.Cells[1, j + 1];
                sheet1.Cells[1, j + 1].Font.Bold = true;
                sheet1.Columns[j + 1].ColumnWidth = 15;
                myRange.Value2 = DGAllEmp.Columns[j].Header;
            }
            for (int i = 0; i < DGAllEmp.Columns.Count; i++)
            {
                for (int j = 0; j < DGAllEmp.Items.Count; j++)
                {
                    TextBlock b = DGAllEmp.Columns[i].GetCellContent(DGAllEmp.Items[j]) as TextBlock;
                    Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 2, i + 1];
                    myRange.Value2 = b.Text;
                }
            }
        }
    }
}