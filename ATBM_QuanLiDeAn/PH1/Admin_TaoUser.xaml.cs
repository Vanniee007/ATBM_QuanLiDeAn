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
        }

        private void Button_TaoUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = TB_Username.Text.ToString();
                string password = TB_Password.Password.ToString();
                string sql = "begin THEM_NHANVIEN('"+TB_Username.Text+"','"+TB_Password.Password+"','"+TB_Role.Text+"'); end;";
                bool kq = Class.DB_Config.RunSQL(sql);
                if (kq)
                {
                    Label_error.Content = "Tạo tài khoản thành công";
                }
                else
                {
                    Label_error.Content = "Tạo tài khoản thất bại";
                }


            }
            catch { }
        }
    }
}
