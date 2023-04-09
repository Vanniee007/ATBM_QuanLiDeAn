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
    /// Interaction logic for Role_capquyen.xaml
    /// </summary>
    public partial class Role_capquyen : Window
    {
        public string role;
        public Role_capquyen(string role)
        {
            InitializeComponent();
            this.role = role;
            role_cq_lb.Content = "Role: " + role;
            DSRole();
        }
        private void DSRole()
        {
            string sql = "SELECT object_name\r\nFROM all_objects\r\nwhere owner in (SELECT username FROM dba_users\r\n                where default_tablespace = 'USERS' and EXpiry_date is not null) and object_type='TABLE'";
            DataTable dt = new DataTable();
            dt = Class.DB_Config.GetDataToTable(sql);
            role_cq_cb_tab.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                role_cq_cb_tab.Items.Add(dt.Rows[i]["OBJECT_NAME"]);
            }
            
        }
       

        private void Role_cq_bt_quaylai_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Role_cq_bt_gan_click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(role_cq_cb_tab.Text=="" || role_cq_cb_tab.Text == "")
                {
                    role_cq_lb_errorout.Content = "Có trường rỗng!!!";
                    role_cq_lb_errorout.Background = Brushes.IndianRed;
                }
                else
                {
                    string sql = "ALTER SESSION SET \"_ORACLE_SCRIPT\" = true";
                    Class.DB_Config.RunSqlDel(sql);
                    sql = "GRANT " + role_cq_cb_quyen.Text + " ON " + role_cq_cb_tab.Text + " TO " + role;
                    Class.DB_Config.RunSqlDel(sql);
                    role_cq_lb_errorout.Content = "Cap quyen thành công!!!";
                    role_cq_lb_errorout.Background = Brushes.LightGreen;
                }
                
            }
            catch
            {

            }
        }
    }
}
