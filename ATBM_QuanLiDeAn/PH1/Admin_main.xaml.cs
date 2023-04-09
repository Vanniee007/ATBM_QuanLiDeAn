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
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Admin_main : Window
    {
        public Admin_main()
        {
            InitializeComponent();
        }

        
        private void HienThi()
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
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Role_loaded(object sender, RoutedEventArgs e)
        {
            HienThi();
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
                Role_TaoRole rtr= new Role_TaoRole();
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
                            HienThi();
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
            HienThi();
        }

        private void role_bt_refresh_click(object sender, RoutedEventArgs e)
        {
            try
            {
                HienThi();
            }
            catch { }
        }

        
    }
}
