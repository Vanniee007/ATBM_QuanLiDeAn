using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
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
    /// Interaction logic for Role_TaoRole.xaml
    /// </summary>
    public partial class Role_TaoRole : Window
    {
        public Role_TaoRole()
        {
            InitializeComponent();
        }

        private void Role_tr_bt_them_click(object sender, RoutedEventArgs e)
        {
            try
            {
               
                if (Role_tb_namerole.Text.ToString() != "")
                {
                    DataTable dt = new DataTable();
                    string sql = "SELECT * FROM DBA_ROLES WHERE ROLE = '" + Role_tb_namerole.Text.ToString().ToUpper() + "'"; // lấy tạm để biết lấy được
                    dt = Class.DB_Config.GetDataToTable(sql); //Đọc danh sách role của user hiện tại

                    if (dt.Rows.Count == 1)
                    {
                        role_tr_lb_errorout.Background = Brushes.IndianRed;
                        role_tr_lb_errorout.Content = "Role đã tồn tại";
                        return;
                    }
                    else
                    {
                        string rolename = Role_tb_namerole.Text.ToString();
                        sql = "ALTER SESSION SET \"_ORACLE_SCRIPT\" = true";
                        Class.DB_Config.RunSqlDel(sql);
                        sql = "CREATE ROLE " + rolename;
                        Class.DB_Config.RunSqlDel(sql);
                        role_tr_lb_errorout.Background = Brushes.LightGreen;
                        role_tr_lb_errorout.Content = "Tạo role thành công.";
                    }

                   
                }
                else
                {
                    role_tr_lb_errorout.Background = Brushes.IndianRed;
                    role_tr_lb_errorout.Content = "Tên role rỗng";
                }
            }
            catch { }
          
       

        }

        private void Role_tr_bt_quaylai_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
