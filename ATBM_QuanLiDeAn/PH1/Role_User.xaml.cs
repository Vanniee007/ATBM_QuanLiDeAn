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
    /// Interaction logic for Role_User.xaml
    /// </summary>
    public partial class Role_User : Window
    {
        public string nameRole;
        public Role_User(string nameRole_)
        {
            InitializeComponent();
            this.nameRole = nameRole_;
        }
        public void HienThiDataGrid()
        {
            try
            {
                DataTable tbl = new DataTable();
                string sql = "SELECT * FROM dba_ROLE_PRIVS where grantee in (SELECT username FROM dba_users\r\n       where default_tablespace = 'USERS' and EXpiry_date is not null) and GRANTED_ROLE='"+nameRole+"'\r\n";
                tbl = Class.DB_Config.GetDataToTable(sql);
                UserRole_Datagrid.ItemsSource = null;
                UserRole_Datagrid.ItemsSource = tbl.DefaultView;
                lb_role_name.Content = "role: " + nameRole;
            }
            catch { }
        }

        private void UserRole_Datagrid_Loaded(object sender, RoutedEventArgs e)
        {
            HienThiDataGrid();
        }

        private void userrole_bt_xoa_click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (UserRole_Datagrid.SelectedIndex.ToString() != null)
                {
                    DataRowView rowview = (DataRowView)UserRole_Datagrid.SelectedItem;
                    if (rowview != null)
                    {
                        string username = rowview["GRANTEE"].ToString();
                        if (MessageBox.Show("Xác nhận xoá user " + username + "khỏi role "+nameRole, "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            
                            string sql = "REVOKE " + nameRole + " FROM " + username;
                            Class.DB_Config.RunSqlDel(sql);
                            roleuser_lb_errorout.Content = "Xóa thành công!!!";
                            roleuser_lb_errorout.Background = Brushes.LightGreen;
                        }
                        
                    }
                    else
                    {
                        roleuser_lb_errorout.Content = "Bạn chưa chọn role để xem!!!";
                        roleuser_lb_errorout.Background = Brushes.IndianRed;
                    }
                }


            }
            catch { }
        }

        private void userrole_bt_gan_click(object sender, RoutedEventArgs e)
        {

        }

        private void userrole_bt_refresh_click(object sender, RoutedEventArgs e)
        {
            HienThiDataGrid();
        }

        private void userrole_bt_return_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
