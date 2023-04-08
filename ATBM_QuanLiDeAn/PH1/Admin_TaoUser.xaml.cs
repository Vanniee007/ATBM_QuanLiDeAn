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
    /// Interaction logic for Admin_TaoUser.xaml
    /// </summary>
    public partial class Admin_TaoUser : Window
    {
        public Admin_TaoUser()
        {
            InitializeComponent();
            DSRole();
        }

        private void DSRole()
        {
            string sql = "SELECT GRANTED_ROLE AS ROLE FROM USER_ROLE_PRIVS WHERE GRANTED_ROLE != 'RESOURCE'";
            DataTable dt = new DataTable();
            dt = Class.DB_Config.GetDataToTable(sql);
            TB_Role.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TB_Role.Items.Add(dt.Rows[i]["ROLE"]);
            }
            //cbRole.ItemsSource = null;
            //cbRole.ItemsSource = dt.DefaultView;
        }

        private void Button_TaoUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = TB_Username.Text.ToString();
                string password = TB_Password.Password.ToString();
                string sql;
                if (((username[0] >= 'a') && (username[0] <= 'z')) || ((username[0] >= 'A') && (username[0] <= 'Z')))
                {
                    sql = "CREATE USER " + username + " IDENTIFIED BY " + password;
                }
                else
                {
                    sql = "CREATE USER " + "\"" + username + "\"" + " IDENTIFIED BY " + password;
                }
                Class.DB_Config.RunSqlDel("ALTER SESSION SET \"_ORACLE_SCRIPT\" = TRUE");
                bool kq = Class.DB_Config.RunSQL(sql);
                if (kq)
                {
                    Label_error.Content = "Tạo tài khoản thành công";
                }
                else
                {
                    Label_error.Content = "Tạo tài khoản thất bại";
                }
                Class.DB_Config.RunSqlDel("grant connect to " + username.ToUpper());

                //Nếu chọn role gán cùng
                if (TB_Role.SelectedItem.ToString()!="")
                {
                    sql = "GRANT " + TB_Role.SelectedItem.ToString() + " TO " + username;
                    kq = Class.DB_Config.RunSQL(sql);
                    if (kq)
                    {
                        Label_error.Content = "Gán tài khoản thành công";
                    }
                }


            }
            catch { }
        }
    }
}
