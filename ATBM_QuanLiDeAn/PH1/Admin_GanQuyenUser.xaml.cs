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
            string sql = "SELECT DISTINCT GRANTED_ROLE as ROLE, (SELECT ADMIN_OPTION FROM DBA_ROLE_PRIVS WHERE GRANTEE = '"+username+ "' and USER_ROLE_PRIVS.GRANTED_ROLE = DBA_ROLE_PRIVS.GRANTED_ROLE)as ADMIN_OPTION, (SELECT 'Granted' FROM DBA_ROLE_PRIVS WHERE GRANTEE = '" + username + "' and USER_ROLE_PRIVS.GRANTED_ROLE = DBA_ROLE_PRIVS.GRANTED_ROLE)as STATUS FROM USER_ROLE_PRIVS WHERE GRANTED_ROLE != 'RESOURCE'";
            table_User = Class.DB_Config.GetDataToTable(sql); //Đọc dữ liệu từ bảng
            VT_Datagrid.ItemsSource = null;
            VT_Datagrid.ItemsSource = table_User.DefaultView; //Nguồn dữ liệu       
            
        }
        private void VT_Datagrid_Loaded(object sender, RoutedEventArgs e)
        {
            VT_GetList_Role();
        }
        private List<string> GetSelectedIndexes(DataGrid dataGrid, int checkboxColumnIndex)
        {
            List<string> selectedValues = new List<string>();

            foreach (var item in dataGrid.Items)
            {
                DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(item);
                if (row != null)
                {
                    // Get the CheckBox control in the specified column of the row
                    var cellContent = dataGrid.Columns[checkboxColumnIndex].GetCellContent(row);
                    var checkBox = cellContent as CheckBox;

                    // If the CheckBox is checked, add the value of the first column to the list
                    if (checkBox != null && checkBox.IsChecked == true)
                    {
                        var firstColumn = dataGrid.Columns[0].GetCellContent(row);
                        selectedValues.Add(((TextBlock)firstColumn).Text);
                    }
                }
            }

            return selectedValues;
        }

        private void VT_Datagrid_DataGridRow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var dataGridRow = sender as DataGridRow;
            var checkBox = FindVisualChild<CheckBox>(dataGridRow);

            if (checkBox != null)
            {
                checkBox.IsChecked = !checkBox.IsChecked;
                e.Handled = true;
            }
        }

        public static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    var result = FindVisualChild<T>(child);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            return null;
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
        private bool ND_GanQuyen(string username, string role_name)
        {
            string sql;
            if (((username[0] >= 'a') && (username[0] <= 'z')) || ((username[0] >= 'A') && (username[0] <= 'Z')))
            {
                sql = "grant " + role_name + " to " + username;
            }
            else
            {
                sql = "grant "+role_name+" to  USER " + "\"" + username + "\"";
            }
            if (VT_CheckBox_GrantOption.IsChecked == true)
            {
                sql += " With ADMIN option";
            }
            Class.DB_Config.RunSqlDel("ALTER SESSION SET \"_ORACLE_SCRIPT\" = TRUE");
            return Class.DB_Config.RunSQL(sql);
        }

        private bool ND_HuyQuyen(string username, string role_name)
        {
            string sql;
            if (((username[0] >= 'a') && (username[0] <= 'z')) || ((username[0] >= 'A') && (username[0] <= 'Z')))
            {
                sql = "revoke " + role_name + " from " + username;
            }
            else
            {
                sql = "revoke " + role_name + " from  USER " + "\"" + username + "\"";
            }
            Class.DB_Config.RunSqlDel("ALTER SESSION SET \"_ORACLE_SCRIPT\" = TRUE");
            return Class.DB_Config.RunSQL(sql);
        }


        private void VT_Button_GanRole_Click(object sender, RoutedEventArgs e)
        {
            var list = GetSelectedIndexes(VT_Datagrid,3);
            int count = 0;
            foreach (var role_name in list)
            {
                if (ND_GanQuyen(username, role_name))
                    count++;
            }
            VT_Label_error.Content = "Gán "+ count + " thành công, "+ (list.Count - count) + " thất bại";
            VT_GetList_Role();
        }

        private void VT_Button_HuyRole_Click(object sender, RoutedEventArgs e)
        {
            var list = GetSelectedIndexes(VT_Datagrid, 3);
            int count = 0;
            foreach (var role_name in list)
            {
                if (ND_HuyQuyen(username, role_name))
                    count++;
            }
            VT_Label_error.Content = "Huỷ "+count + " thành công, " + (list.Count - count) + " thất bại";
            VT_GetList_Role();
        }

        private void Q_Button_GanRole_Click(object sender, RoutedEventArgs e)
        {
            var list = GetSelectedIndexes(Q_Datagrid, 3);
            int count = 0;
            foreach (var role_name in list)
            {
                if (ND_GanQuyen(username, role_name))
                    count++;
            }
            Q_Label_error.Content = "Gán " + count + " thành công, " + (list.Count - count) + " thất bại";
            VT_GetList_Role();
        }

        private void Q_Button_HuyRole_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
