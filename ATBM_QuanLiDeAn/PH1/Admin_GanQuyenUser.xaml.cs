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
        string C_table_name;
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
            Label_error.Content = "";
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
            string sql = "SELECT PRIVILEGE,(select distinct ADMIN_OPTION from dba_sys_privs d where grantee = '"+username+ "' and s.PRIVILEGE = d.PRIVILEGE) as ADMIN_OPTION, (select DISTINCT 'Granted' from dba_sys_privs d where grantee = '"+username+ "' and s.PRIVILEGE = d.PRIVILEGE) as STATUS  FROM session_privs s";

            table_User = Class.DB_Config.GetDataToTable(sql); //Đọc dữ liệu từ bảng
            Q_Datagrid.ItemsSource = null;
            Q_Datagrid.ItemsSource = table_User.DefaultView; //Nguồn dữ liệu
        }
        private void Q_Datagrid_Loaded(object sender, RoutedEventArgs e)
        {
            Label_error.Content = "";
            Q_GetList_Privilige();
        }

        private void B_GetList_Table()
        {
            DataTable table_User;
            string sql = "select distinct table_name TBNAME, (select 'YES' from dba_tab_privs con where grantee = '" + username + "' and GLO.table_name = con.table_name  and PRIVILEGE = 'SELECT')as PRI_SELECT," +
        "(select grantable from dba_tab_privs con where grantee = '" + username + "' and GLO.table_name = con.table_name and PRIVILEGE = 'SELECT') as GA_SELECT," +
        "(select 'YES' from dba_tab_privs con where grantee = '" + username + "' and GLO.table_name = con.table_name  and PRIVILEGE = 'INSERT')as PRI_INSERT, " +
        "(select grantable from dba_tab_privs con where grantee = '" + username + "' and GLO.table_name = con.table_name and PRIVILEGE = 'INSERT') as GA_INSERT," +
        "(select 'YES' from dba_tab_privs con where grantee = '" + username + "' and GLO.table_name = con.table_name  and PRIVILEGE = 'UPDATE')as PRI_UPDATE," +
        "(select grantable from dba_tab_privs con where grantee = '" + username + "' and GLO.table_name = con.table_name and PRIVILEGE = 'UPDATE') as GA_UPDATE," +
        "(select 'YES' from dba_tab_privs con where grantee = '" + username + "' and GLO.table_name = con.table_name  and PRIVILEGE = 'DELETE')as PRI_DELETE, " +
        "(select grantable from dba_tab_privs con where grantee = '" + username + "' and GLO.table_name = con.table_name and PRIVILEGE = 'DELETE') as GA_DELETE FROM ALL_TABLES GLO WHERE TABLESPACE_NAME = 'DA_ATBM'";
            table_User = Class.DB_Config.GetDataToTable(sql); //Đọc dữ liệu từ bảng
            B_Datagrid.ItemsSource = null;
            B_Datagrid.ItemsSource = table_User.DefaultView; //Nguồn dữ liệu
        }
        private void B_Datagrid_Loaded(object sender, RoutedEventArgs e)
        {
            B_GetList_Table();
            Label_error.Content = "";
        }

        private void CB_GetList_Table()
        {
            DataTable table_User;
            string sql = "SELECT distinct table_name TBNAME FROM ALL_TABLES WHERE TABLESPACE_NAME = 'DA_ATBM'";
            table_User = Class.DB_Config.GetDataToTable(sql); //Đọc dữ liệu từ bảng
            CB_Datagrid.ItemsSource = null;
            CB_Datagrid.ItemsSource = table_User.DefaultView; //Nguồn dữ liệu
        }

        private void CB_GetList_Column()
        {
            if (CB_Datagrid.SelectedIndex >= 0)
            {
                DataRowView curRow = CB_Datagrid.SelectedItem as DataRowView;
                C_table_name = curRow.Row.ItemArray[0].ToString();
                DataTable table_User;
                string sql = "SELECT column_name CLMN, (select PRIVILEGE from dba_col_privs d where u.column_name = d.column_name and grantee = '"+username+"') as PR_UPDATE, (select GRantable from dba_col_privs d where u.column_name = d.column_name and grantee = '" + username + "') as GR_UPDATE FROM user_tab_cols u WHERE table_name = '" +C_table_name+"'";
                table_User = Class.DB_Config.GetDataToTable(sql); //Đọc dữ liệu từ bảng
                C_Datagrid.ItemsSource = null;
                C_Datagrid.ItemsSource = table_User.DefaultView; //Nguồn dữ liệu
            }
        }

        private bool B_CheckIfTableSelected(DataGrid dataGrid, int checkboxColumnIndex)
        {
            int count = 0;

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
                        count++;
                    }
                }
            }
            if (count > 0) return true;
            return false;
        }
        private bool ND_GanQuyen(string username, string role_name, CheckBox checkbox )
        {
            string sql;
            if (((username[0] >= 'a') && (username[0] <= 'z')) || ((username[0] >= 'A') && (username[0] <= 'Z')))
            {
                sql = "grant " + role_name + " to " + username+" ";
            }
            else
            {
                sql = "grant "+role_name+" to  USER " + "\"" + username + "\" ";
            }
            if (checkbox.IsChecked == true)
            {
                sql += checkbox.Content.ToString();
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
                ND_HuyQuyen(username, role_name);
                if (ND_GanQuyen(username, role_name, VT_CheckBox_GrantOption))
                    count++;
            }
            Label_error.Content = "Gán "+ count + " thành công, "+ (list.Count - count) + " thất bại";
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
            Label_error.Content = "Huỷ "+count + " thành công, " + (list.Count - count) + " thất bại";
            VT_GetList_Role();
        }

        private void Q_Button_GanRole_Click(object sender, RoutedEventArgs e)
        {
            var list = GetSelectedIndexes(Q_Datagrid, 3);
            int count = 0;
            foreach (var role_name in list)
            {
                ND_HuyQuyen(username, role_name);
                if (ND_GanQuyen(username, role_name, Q_CheckBox_GrantOption))
                    count++;
            }
            Q_GetList_Privilige();
            Label_error.Content = "Gán " + count + " thành công, " + (list.Count - count) + " thất bại";
        }

        private void Q_Button_HuyRole_Click(object sender, RoutedEventArgs e)
        {
            var list = GetSelectedIndexes(Q_Datagrid, 3);
            int count = 0;
            foreach (var role_name in list)
            {
                if (ND_HuyQuyen(username, role_name))
                    count++;
            }
            Q_GetList_Privilige();
            Label_error.Content = "Huỷ " + count + " thành công, " + (list.Count - count) + " thất bại";
        }

        private void B_Button_GanRole_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;
            int ListCount = 0;
            var list = GetSelectedIndexes(B_Datagrid, 9);
            ListCount += list.Count;
            foreach (var role_name in list)
            {
                if (ND_GanQuyen(username, "select on " + role_name, B_CheckBox_GrantOption))
                    count++;
            }

            list = GetSelectedIndexes(B_Datagrid, 10);
            ListCount += list.Count;
            foreach (var role_name in list)
            {
                if (ND_GanQuyen(username, "insert on " + role_name, B_CheckBox_GrantOption))
                    count++;
            }

            list = GetSelectedIndexes(B_Datagrid, 11);
            ListCount += list.Count;
            foreach (var role_name in list)
            {
                if (ND_GanQuyen(username, "update on " + role_name, B_CheckBox_GrantOption))
                    count++;
            }

            list = GetSelectedIndexes(B_Datagrid, 12);
            ListCount += list.Count;
            foreach (var role_name in list)
            {
                if (ND_GanQuyen(username, "delete on " + role_name, B_CheckBox_GrantOption))
                    count++;
            }
            ListCount += list.Count;

            Label_error.Content = "Gán " + count + " thành công, " + (ListCount - count) + " thất bại";
            B_GetList_Table();
        }

        private void B_Button_HuyRole_Click(object sender, RoutedEventArgs e)
        {

            int count = 0;
            int ListCount = 0;
            var list = GetSelectedIndexes(B_Datagrid, 9);
            ListCount += list.Count;
            foreach (var role_name in list)
            {
                if (ND_HuyQuyen(username, "select on " + role_name))
                    count++;
            }

            list = GetSelectedIndexes(B_Datagrid, 10);
            ListCount += list.Count;
            foreach (var role_name in list)
            {
                if (ND_HuyQuyen(username, "insert on " + role_name))
                    count++;
            }

            list = GetSelectedIndexes(B_Datagrid, 11);
            ListCount += list.Count;
            foreach (var role_name in list)
            {
                if (ND_HuyQuyen(username, "update on " + role_name))
                    count++;
            }

            list = GetSelectedIndexes(B_Datagrid, 12);
            ListCount += list.Count;
            foreach (var role_name in list)
            {
                if (ND_HuyQuyen(username, "delete on " + role_name))
                    count++;
            }
            ListCount += list.Count;

            Label_error.Content = "Huỷ " + count + " thành công, " + (ListCount - count) + " thất bại";
            B_GetList_Table();
        }

        private void CB_Datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CB_GetList_Column();
        }


        private void CB_Datagrid_Loaded(object sender, RoutedEventArgs e)
        {
            CB_GetList_Table();
            Label_error.Content = "";
        }

        private void C_Button_GanRole_Click(object sender, RoutedEventArgs e)
        {
            var list = GetSelectedIndexes(C_Datagrid, 3);
            int count = 0;
            foreach (var clmn in list)
            {
                //grant update(CLMN) on tabne_name to user
                ND_HuyQuyen(username, "update(" + clmn + ") on " + C_table_name);
                if (ND_GanQuyen(username, "update("+ clmn + ") on "+ C_table_name, C_CheckBox_GrantOption))
                    count++;
            }
            Label_error.Content = "Gán " + count + " thành công, " + (list.Count - count) + " thất bại";
            CB_GetList_Column();
        }

        private void C_Button_HuyRole_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;
            ND_HuyQuyen(username, "update on " + C_table_name);
            count++;
            Label_error.Content = "Huỷ " + count + " thành công, " + (1 - count) + " thất bại";
            CB_GetList_Column();
        }

        private void C_Datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CB_GetList_Table();
        }
    }
}
