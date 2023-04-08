using System;
using System.Collections.Generic;
using System.Data;
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

namespace ATBM_QuanLiDeAn.PH1
{
    /// <summary>
    /// Interaction logic for Admin_GanQuyenUser.xaml
    /// </summary>
    public partial class Admin_GanQuyenUser : Window
    {
        string username;
        public Admin_GanQuyenUser(string username_)
        {
            InitializeComponent();
            username = username_;
            Lb_username.Content = "user: " + username;
            
        }

        private void VT_GetList_Role()
        {
            DataTable table_User;
            string sql = "SELECT GRANTED_ROLE as ROLE FROM USER_ROLE_PRIVS WHERE GRANTED_ROLE != 'RESOURCE'";
            //if (cbOnly.IsChecked == false)
            //{
            //    sql = sql + " AND USERNAME LIKE 'U%'";
            ////}
            table_User = Class.DB_Config.GetDataToTable(sql); //Đọc dữ liệu từ bảng
            VT_Datagrid.ItemsSource = null;
            VT_Datagrid.ItemsSource = table_User.DefaultView; //Nguồn dữ liệu       
            
        }
        private void VT_Datagrid_Loaded(object sender, RoutedEventArgs e)
        {
            VT_GetList_Role();
        }

        private void Q_GetList_Privilige()
        {
            DataTable table_User;
            string sql = "SELECT * FROM session_privs";
            //if (cbOnly.IsChecked == false)
            //{
            //    sql = sql + " AND USERNAME LIKE 'U%'";
            //}
            table_User = Class.DB_Config.GetDataToTable(sql); //Đọc dữ liệu từ bảng
            Q_Datagrid.ItemsSource = null;
            Q_Datagrid.ItemsSource = table_User.DefaultView; //Nguồn dữ liệu
        }
        private void Q_Datagrid_Loaded(object sender, RoutedEventArgs e)
        {
            Q_GetList_Privilige();
        }

        private void B_GetList_Table()
        {
            DataTable table_User;
            string sql = "Select * From User_Objects where Object_type = 'TABLE'";
            //if (cbOnly.IsChecked == false)
            //{
            //    sql = sql + " AND USERNAME LIKE 'U%'";
            //}
            table_User = Class.DB_Config.GetDataToTable(sql); //Đọc dữ liệu từ bảng
            B_Datagrid.ItemsSource = null;
            B_Datagrid.ItemsSource = table_User.DefaultView; //Nguồn dữ liệu
        }
        private void B_Datagrid_Loaded(object sender, RoutedEventArgs e)
        {
            B_GetList_Table();
        }


        private void B_GetList_Column()
        {
            if (B_Datagrid.SelectedIndex >= 0)
            {
                DataRowView curRow = B_Datagrid.SelectedItem as DataRowView;
                string Table_ = curRow.Row.ItemArray[0].ToString();
                DataTable table_User;
                string sql = "SELECT column_name FROM user_tab_cols WHERE table_name = '"+ Table_+"'";
                table_User = Class.DB_Config.GetDataToTable(sql); //Đọc dữ liệu từ bảng
                C_Datagrid.ItemsSource = null;
                C_Datagrid.ItemsSource = table_User.DefaultView; //Nguồn dữ liệu
            }
        }

        private void B_Datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            B_GetList_Column();
            C_Datagrid.IsEnabled = false;
        }
    }
}
