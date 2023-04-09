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
    /// Interaction logic for Role_XemQuyen.xaml
    /// </summary>
    public partial class Role_XemQuyen : Window
    {
        public string role;
        public Role_XemQuyen(string role_)
        {
            InitializeComponent();
            role = role_;

        }
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public void role_xemquyen_hienThi()
        {
            try
            {
                DataTable tbl = new DataTable();
                string sql = "select* from ROLE_SYS_PRIVS where ROLE='"+role+"'";
                tbl = Class.DB_Config.GetDataToTable(sql);
                Role_xq_sys_Datagrid.ItemsSource = null;
                Role_xq_sys_Datagrid.ItemsSource = tbl.DefaultView;
                Role_xq_sys_lb.Content = "Quyền trên hệ thống của role: " + role;

            }
            catch { }
        }
        private void Xemquyen_tab_loaded(object sender, RoutedEventArgs e)
        {
            role_xemquyen_hienThi();
        }

        private void Role_xq_sys_Datagrid_Loaded(object sender, RoutedEventArgs e)
        {
            role_xemquyen_hienThi();
        }

        private void role_xq_bt_quaylai_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void role_xq_bt_lammoi_click(object sender, RoutedEventArgs e)
        {
            role_xemquyen_hienThi();
        }
        private void role_xq_bt_xoa_click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Role_xq_sys_Datagrid.SelectedIndex.ToString() != null)
                {
                    DataRowView rowview = (DataRowView)Role_xq_sys_Datagrid.SelectedItem;
                    if (rowview != null)
                    {
                        if (MessageBox.Show("Xác nhận xoá user " + rowview["ROLE"].ToString(), "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            
                            string sql = "ALTER SESSION SET \"_ORACLE_SCRIPT\" = true";
                            Class.DB_Config.RunSqlDel(sql);
                            sql = "ROVOKE  " + rowview["PRIVILEGE"].ToString() + " FROM " + role;
                            Class.DB_Config.RunSqlDel(sql);
                            role_xq_lb_errorout.Content = "Xóa thành công!!!";
                            role_xq_lb_errorout.Background = Brushes.LightGreen;
                           
                        }

                    }
                    else
                    {
                        role_xq_lb_errorout.Content = "Bạn chưa chọn quyền để xóa!!!";
                        role_xq_lb_errorout.Background = Brushes.IndianRed;
                    }
                }
            }
            catch
            {

            }
        }
        public void role_xemquyen_tab_hienThi()
        {
            try
            {
                DataTable tbl = new DataTable();
                string sql = "select* from ROLE_TAB_PRIVS where ROLE='" + role + "'";
                tbl = Class.DB_Config.GetDataToTable(sql);
                Role_xq_tab_Datagrid.ItemsSource = null;
                Role_xq_tab_Datagrid.ItemsSource = tbl.DefaultView;
                Role_xq_tab_lb.Content = "Quyền trên tab của role: " + role;

            }
            catch { }
        }
        private void Xemquyen_sys_loaded(object sender, RoutedEventArgs e)
        {
            role_xemquyen_tab_hienThi();
        }

        private void Role_xq_tab_Datagrid_Loaded(object sender, RoutedEventArgs e)
        {
            role_xemquyen_tab_hienThi();
        }

        private void role_xq_tab_bt_quaylai_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void role_xq_tab_bt_xoa_click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Role_xq_tab_Datagrid.SelectedIndex.ToString() != null)
                {
                    DataRowView rowview = (DataRowView)Role_xq_tab_Datagrid.SelectedItem;
                    if (rowview != null)
                    {
                        if (MessageBox.Show("Xác nhận xoá quyền " + rowview["PRIVILEGE"].ToString()+ " trên "+ rowview["TABLE_NAME"].ToString()+" của "+role, "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            string rolename = rowview["PRIVILEGE"].ToString();
                            string sql = "ALTER SESSION SET \"_ORACLE_SCRIPT\" = true";
                            Class.DB_Config.RunSqlDel(sql);
                            sql = "REVOKE " + rowview["PRIVILEGE"].ToString() + " ON " + rowview["TABLE_NAME"].ToString() + " FROM " + role ;
                            Class.DB_Config.RunSqlDel(sql);
                            role_xq_tab_lb_errorout.Content = "Xóa thành công!!!";
                            role_xq_tab_lb_errorout.Background = Brushes.LightGreen;
                           
                        }

                    }
                    else
                    {
                        role_xq_tab_lb_errorout.Content = "Bạn chưa chọn role để xóa!!!";
                        role_xq_tab_lb_errorout.Background = Brushes.IndianRed;
                    }
                }
            }
            catch
            {

            }
        }

        private void role_xq_bt_tab_lammoi_click(object sender, RoutedEventArgs e)
        {
            role_xemquyen_tab_hienThi();
        }
    }
}
