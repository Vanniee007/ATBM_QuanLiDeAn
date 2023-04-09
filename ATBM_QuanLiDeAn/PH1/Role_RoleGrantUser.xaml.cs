using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for Role_RoleGrantUser.xaml
    /// </summary>
    public partial class Role_RoleGrantUser : Window
    {
        public string RoleName;
        public Role_RoleGrantUser(string nameRole_)
        {
            InitializeComponent();
            RoleName = nameRole_;
            lb_role_name.Content = "Role: " + RoleName;
        }
        public void datagrid_HienTHi()
        {
            try
            {
                DataTable table_User;
                string sql = "select USERNAME from DBA_USERS where USERNAME NOT IN(select GRANTEE AS USERNAME FROM DBA_ROLE_PRIVS WHERE GRANTED_ROLE = '"+RoleName+"') and EXpirY_date is not NULL ";
                table_User = Class.DB_Config.GetDataToTable(sql); //Đọc dữ liệu từ bảng
                User_Datagrid.ItemsSource = null;
                User_Datagrid.ItemsSource = table_User.DefaultView; //Nguồn dữ liệu    
            }
            catch { }
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

        private void User_Datagrid_DataGridRow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
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

        private void User_Datagrid_Loaded(object sender, RoutedEventArgs e)
        {
            datagrid_HienTHi();
        }

      
        private void role_rgu_bt_return_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void role_rgu_bt_grant_click(object sender, RoutedEventArgs e)
        {
            var list = GetSelectedIndexes(User_Datagrid, 1);
            int count = 0;
            foreach (var username in list)
            {
                if (ND_GanQuyen(username, RoleName))
                    count++;
            }
            User_Label_error.Content = "Gán " + count + " thành công, " + (list.Count - count) + " thất bại";
            datagrid_HienTHi();
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
                sql = "grant " + role_name + " to  USER " + "\"" + username + "\"";
            }
            if (User_CheckBox_AdminOption.IsChecked == true)
            {
                sql += " With ADMIN option";
            }
            Class.DB_Config.RunSqlDel("ALTER SESSION SET \"_ORACLE_SCRIPT\" = TRUE");
            return Class.DB_Config.RunSQL(sql);
        }


    }
}
