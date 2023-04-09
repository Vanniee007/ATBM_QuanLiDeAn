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
    public partial class Admin_DoiMatKhau : Window
    {
        public Admin_DoiMatKhau(string username)
        {
            InitializeComponent();
            TB_Username.Text = username;
            TB_Username.IsEnabled = false;
        }


        private void Button_DoiMatKhau_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = TB_Username.Text.ToString();
                string password = TB_Password.Password.ToString();
                string sql;
                if (((username[0] >= 'a') && (username[0] <= 'z')) || ((username[0] >= 'A') && (username[0] <= 'Z')))
                {
                    sql = "ALTER USER " + username + " IDENTIFIED BY " + password;
                }
                else
                {
                    sql = "ALTER USER " + "\"" + username + "\"" + " IDENTIFIED BY " + password;
                }
                Class.DB_Config.RunSqlDel("ALTER SESSION SET \"_ORACLE_SCRIPT\" = TRUE");
                bool kq = Class.DB_Config.RunSQL(sql);
                if (kq)
                {
                    Label_error.Content = "Đổi mật khẩu thành công";
                }
                else
                {
                    Label_error.Content = "Đổi mật khẩu thất bại";
                }
            }
            catch { }
        }


        private void TB_Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (TB_Password.Password.ToString() != TB_NewPassword.Password.ToString())
            {
                Label_error.Content = "Mật khẩu không trùng khớp";
                Button_DoiMatKhau.IsEnabled = false;
            }
            else
            {
                Label_error.Content = "";
                Button_DoiMatKhau.IsEnabled = true;
            }
        }

        private void TB_NewPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (TB_Password.Password.ToString() != TB_NewPassword.Password.ToString())
            {
                Label_error.Content = "Mật khẩu không trùng khớp";
                Button_DoiMatKhau.IsEnabled = false;
            }
            else
            {
                Label_error.Content = "";
                Button_DoiMatKhau.IsEnabled = true;
            }
        }
    }
}
