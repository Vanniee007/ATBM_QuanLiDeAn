using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ATBM_QuanLiDeAn.PH1
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Admin_main : Window
    {
        public Admin_main()
        {
            InitializeComponent();
        }


        private void ND_HienThi(string ob)
        {
            DataTable tbl = new DataTable();
            string sql = "SELECT USER_ID, OBJECT_TYPE, CREATED, STATUS" +
                " FROM USER_OBJECTS WHERE OBJECT_TYPE != 'SEQUENCE'";
            if (ob != "Tất cả")
            {
                sql = sql + " AND OBJECT_TYPE = '" + ob.ToUpper() + "'";
            }
            tbl = Class.DB_Config.GetDataToTable(sql);
            ND_Datagrid.ItemsSource = null;
            ND_Datagrid.ItemsSource = tbl.DefaultView;
        }
        private void Role_HienThi()
        {
            try
            {
                DataTable tbl = new DataTable();
                string sql = "SELECT distinct GRANTED_ROLE AS ROLE FROM USER_ROLE_PRIVS WHERE GRANTED_ROLE != 'RESOURCE'";
                tbl = Class.DB_Config.GetDataToTable(sql);
                Role_Datagrid.ItemsSource = null;
                Role_Datagrid.ItemsSource = tbl.DefaultView;

            }
            catch { }

        }
        private void ND_Datagrid_Loaded(object sender, RoutedEventArgs e)
        {
            ND_Get_DSUser();
        }

        private void ND_Get_DSUser()
        {
            DataTable table_User;
            string sql;
            sql = "SELECT USERNAME, ACCOUNT_STATUS AS STATUS, CREATED, EXPIRY_DATE, LAST_LOGIN " +
                "FROM DBA_USERS WHERE  default_tablespace = 'DA_ATBM' and  EXpirY_date is not NULL";
            //if (cbOnly.IsChecked == false)
            //{
            //    sql = sql + " AND USERNAME LIKE 'U%'";
            //}
            table_User = Class.DB_Config.GetDataToTable(sql); //Đọc dữ liệu từ bảng
            ND_Datagrid.ItemsSource = null;
            ND_Datagrid.ItemsSource = table_User.DefaultView; //Nguồn dữ liệu
            ND_ConfigButton_NotSelected();

        }
        private void ND_ConfigButton_Selected()
        {
            ND_Button_CreateUser.IsEnabled = true;
            ND_Button_XoaUser.IsEnabled = true;
            //ND_Button_CapQuyen.IsEnabled = true;
            ND_button_DoiMatKhau.IsEnabled = true;
            ND_button_KhoaUser.IsEnabled = true;
            ND_button_EditUser.IsEnabled = true;

        }
        private void ND_ConfigButton_NotSelected()
        {
            ND_Button_CreateUser.IsEnabled = true;
            ND_Button_XoaUser.IsEnabled = false;
            //ND_Button_CapQuyen.IsEnabled = false;
            ND_button_DoiMatKhau.IsEnabled = false;
            ND_button_KhoaUser.IsEnabled = false;
            ND_button_EditUser.IsEnabled = false;
        }
        private void ND_ConfigButton_LOCKUNLOCK()
        {
            if (ND_Datagrid.SelectedIndex >= 0)
            {
                DataRowView curRow = ND_Datagrid.SelectedItem as DataRowView;
                string STATUS = curRow.Row.ItemArray[1].ToString();


                if (STATUS.ToUpper() == "OPEN")
                    ND_button_KhoaUser.Content = "Khoá User";
                else
                    ND_button_KhoaUser.Content = "Mở khoá User";
            }
        }
        private void Tab_User_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void ND_Button_CreateUser_Click(object sender, RoutedEventArgs e)
        {
            Admin_TaoUser TaoUser = new Admin_TaoUser();
            TaoUser.ShowDialog();
            ND_Get_DSUser();
        }

        private void ND_Datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ND_ConfigButton_Selected();
            ND_ConfigButton_LOCKUNLOCK();
        }

        private void ND_Button_XoaUser_Click(object sender, RoutedEventArgs e)
        {
            string sql;
            if (ND_Datagrid.SelectedIndex >= 0)
            {
                DataRowView curRow = ND_Datagrid.SelectedItem as DataRowView;
                string Username = curRow.Row.ItemArray[0].ToString();
                if (MessageBox.Show("Xác nhận xoá user " + Username, "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (((Username[0] >= 'a') && (Username[0] <= 'z')) || ((Username[0] >= 'A') && (Username[0] <= 'Z')))
                    {
                        sql = "DROP USER " + Username + " CASCADE";
                    }
                    else
                    {
                        sql = "DROP USER \"" + Username + "\" CASCADE";
                    }
                    Class.DB_Config.RunSqlDel("ALTER SESSION SET \"_ORACLE_SCRIPT\" = TRUE");
                    bool kq = Class.DB_Config.RunSQL(sql);
                    if (kq)
                    {
                        ND_Label_error.Content = "Xoá tài khoản thành công";
                    }
                    else
                    {
                        ND_Label_error.Content = "Xoá tài khoản thất bại";
                    }
                    ND_Get_DSUser();
                    ND_ConfigButton_NotSelected();
                }
                return;
            }

        }

        private void ND_button_DoiMatKhau_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ND_Datagrid.SelectedIndex >= 0)
                {
                    DataRowView curRow = ND_Datagrid.SelectedItem as DataRowView;
                    string Username = curRow.Row.ItemArray[0].ToString();
                    Admin_DoiMatKhau DoiMK = new Admin_DoiMatKhau(Username);
                    DoiMK.ShowDialog();
                    ND_Get_DSUser();
                }
            }
            catch
            { }
        }

        private void ND_button_KhoaUser_Click(object sender, RoutedEventArgs e)
        {
            string sql;
            if (ND_Datagrid.SelectedIndex >= 0)
            {
                DataRowView curRow = ND_Datagrid.SelectedItem as DataRowView;
                string STATUS = curRow.Row.ItemArray[1].ToString();
                string Username = curRow.Row.ItemArray[0].ToString();
                if (MessageBox.Show("Xác nhận khoá user " + Username, "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {

                    if (STATUS.ToUpper() == "OPEN") 
                        STATUS = "LOCK";
                    else
                        STATUS = "UNLOCK";

                    if (((Username[0] >= 'a') && (Username[0] <= 'z')) || ((Username[0] >= 'A') && (Username[0] <= 'Z')))
                    {
                        sql = "ALTER USER " + Username + " ACCOUNT "+ STATUS;
                    }
                    else
                    {
                        sql = "ALTER USER \"" + Username + "\" ACCOUNT" + STATUS;
                    }
                    Class.DB_Config.RunSqlDel("ALTER SESSION SET \"_ORACLE_SCRIPT\" = TRUE");
                    bool kq = Class.DB_Config.RunSQL(sql);
                    if (kq)
                    {
                        ND_Label_error.Content = "Khoá tài khoản thành công";
                    }
                    else
                    {
                        ND_Label_error.Content = "Khoá tài khoản thất bại";
                    }
                    ND_Get_DSUser();
                    ND_ConfigButton_NotSelected();
                }
                return;
            }
        }

        private void ND_button_EditUser_Click(object sender, RoutedEventArgs e)
        {
            if (ND_Datagrid.SelectedIndex >= 0)
            {
                DataRowView curRow = ND_Datagrid.SelectedItem as DataRowView;
                string STATUS = curRow.Row.ItemArray[1].ToString();
                string Username = curRow.Row.ItemArray[0].ToString();
                Admin_GanQuyenUser TaoUser = new Admin_GanQuyenUser(Username);
                TaoUser.ShowDialog();
                ND_Get_DSUser();
            }
        }


        private void Role_loaded(object sender, RoutedEventArgs e)
        {
            Role_HienThi();
        }

        private void role_bt_xemquyen_click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Role_Datagrid.SelectedIndex.ToString() != null)
                {
                    DataRowView rowview = (DataRowView)Role_Datagrid.SelectedItem;
                    if (rowview != null)
                    {
                        Role_XemQuyen rxq = new Role_XemQuyen(rowview["ROLE"].ToString());
                        rxq.Show();
                    }
                    else
                    {
                        role_lb_errorout.Content = "Bạn chưa chọn role để xem!!!";
                        role_lb_errorout.Background = Brushes.IndianRed;
                    }
                }


            }
            catch { }
        }

        private void role_bt_capquyen_click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Role_Datagrid.SelectedIndex.ToString() != null)
                {
                    DataRowView rowview = (DataRowView)Role_Datagrid.SelectedItem;
                    if (rowview != null)
                    {
                        Role_capquyen rxq = new Role_capquyen(rowview["ROLE"].ToString());
                        rxq.Show();
                    }
                    else
                    {
                        role_lb_errorout.Content = "Bạn chưa chọn role để cấp quyền!!!";
                        role_lb_errorout.Background = Brushes.IndianRed;
                    }
                }


            }
            catch { }
        }

        private void role_bt_roleuser_click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Role_Datagrid.SelectedIndex.ToString() != null)
                {
                    DataRowView rowview = (DataRowView)Role_Datagrid.SelectedItem;
                    if (rowview != null)
                    {
                        Role_User ru = new Role_User(rowview["ROLE"].ToString());
                        ru.Show();
                    }
                    else
                    {
                        role_lb_errorout.Content = "Bạn chưa chọn role để grant cho user!!!";
                        role_lb_errorout.Background = Brushes.IndianRed;
                    }
                }


            }
            catch { }

        }

        private void role_bt_themrole_click(object sender, RoutedEventArgs e)
        {
            try
            {
                Role_TaoRole rtr = new Role_TaoRole();
                rtr.Show();
            }
            catch { }
        }

        private void role_bt_xoarole_click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Role_Datagrid.SelectedIndex.ToString() != null)
                {
                    DataRowView rowview = (DataRowView)Role_Datagrid.SelectedItem;
                    if (rowview != null)
                    {
                        if (MessageBox.Show("Xác nhận xoá user " + rowview["ROLE"].ToString(), "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            string rolename = rowview["ROLE"].ToString();
                            string sql = "ALTER SESSION SET \"_ORACLE_SCRIPT\" = true";
                            Class.DB_Config.RunSqlDel(sql);
                            sql = "DROP ROLE " + rolename;
                            Class.DB_Config.RunSqlDel(sql);
                            role_lb_errorout.Content = "Xóa thành công!!!";
                            role_lb_errorout.Background = Brushes.LightGreen;
                            Role_HienThi();
                        }

                    }
                    else
                    {
                        role_lb_errorout.Content = "Bạn chưa chọn role để xóa!!!";
                        role_lb_errorout.Background = Brushes.IndianRed;
                    }
                }
            }
            catch
            {

            }
        }

        private void Role_Datagrid_Loaded(object sender, RoutedEventArgs e)
        {
            Role_HienThi();
        }

        private void role_bt_refresh_click(object sender, RoutedEventArgs e)
        {
            try
            {
                Role_HienThi();
            }
            catch { }
        }

        private void Tab_Object_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void DT_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                DataTable tbl = new DataTable();
                string typeFunction = "";
                typeFunction = DT_ComboBox.Text;
                string sql = "SELECT * FROM DBA_OBJECTS WHERE object_type = '" + typeFunction + "'";
                tbl = Class.DB_Config.GetDataToTable(sql);
                DT_Datagrid.ItemsSource = null;
                DT_Datagrid.ItemsSource = tbl.DefaultView;

            }
            catch { }
        }
    }
}
