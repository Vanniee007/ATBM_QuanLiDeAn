using ATBM_QuanLiDeAn.PH1;
using ATBM_QuanLiDeAn.PH2;
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
using System.Windows.Shapes;

namespace ATBM_QuanLiDeAn
{
    /// <summary>
    /// Interaction logic for LoginWindows.xaml
    /// </summary>       
    public partial class Login_Window : Window
    {
        InputValidation validation = new InputValidation();
        public Login_Window(string username)
        {
            InitializeComponent();
            tb_username.Text = username;
            tb_username.Focus();
            tb_username.Text = "NV001";
            tb_password.Password = "1";
        }

        //animation cửa số đăng nhập
        private void tb_username_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tb_username.Text) && tb_username.Text.Length > 0)
            {
                lb_username.Visibility = Visibility.Collapsed;
            }
            else
            {
                lb_username.Visibility = Visibility.Visible;
            }
        }
        private void Hd_HienMaLoi_Label(int ketqua)
        {
            if (ketqua > 0)
            {
                // đăng nhập thành công    
                login_lb_error.Content = "Đăng nhập thành công";
                login_lb_error.Foreground = Brushes.DarkGreen;
            }
            else
            {
                // đăng nhập thất bại
                switch (ketqua)
                {
                    case -1:
                        login_lb_error.Content = "Có trường trống";
                        break;
                    case -2:
                        login_lb_error.Content = "username không được chứa khoảng trắng";
                        break;
                    case -3:
                        login_lb_error.Content = "password không được chứa khoảng trắng";
                        break;
                    case -4:
                        login_lb_error.Content = "username hoặc password sai";
                        break;
                    case -5:
                        login_lb_error.Content = "Tài khoản bị khoá";
                        break;
                    case 0:
                        login_lb_error.Content = "Lỗi hệ thống";
                        break;
                    default:
                        break;
                }
                login_lb_error.Foreground = Brushes.IndianRed;
            }
        }
        private void Login()
        {
            if (!InputValidation.ValidUsername(tb_username.Text))
            {
                lb_error.Content = "Tên đăng nhập không hợp lệ";
                lb_error.Foreground = Brushes.IndianRed;
                return;
            }
            if (!InputValidation.ValidPassword(tb_password.Password) )
            {
                lb_error.Content = "Mật khẩu không hợp lệ";
                lb_error.Foreground = Brushes.IndianRed;
                return;
            }


            string username = tb_username.Text.ToString();
            string password = tb_password.Password.ToString();

            bool isConnect = Class.DB_Config.Connect(username, password);
            if (isConnect)  // Kết nối thành công
            {
                try
                {
                    DataTable dt = new DataTable();
                    string sql = "SELECT * FROM DBA_ROLE_PRIVS WHERE GRANTEE = '" + username + "' and granted_role != 'CONNECT'";
                    try
                    {
                        // lấy tạm để biết lấy được
                        dt = Class.DB_Config.GetDataToTable(sql); //Đọc danh sách role của user hiện tại

                        if (dt.Rows.Count >= 1)
                        {
                            // Có nhiều hơn 1 role -> admin 
                            Admin_main ad = new Admin_main();
                            ad.Show();
                            this.Close();
                            return;
                        }
                    }
                    catch { }
                    sql = "select VAITRO from ATBM_ADMIN.LAYVAITRO"; 
                    dt = Class.DB_Config.GetDataToTable(sql);
                    string VaiTro = dt.Rows[0][0].ToString();
                    switch (VaiTro)
                    {
                        case "Nhân viên":
                            NhanVien_Main nv = new NhanVien_Main(username);
                            nv.Show();
                            this.Close();
                            break;

                        case "Trưởng phòng":
                            TruongPhong_Main tp = new TruongPhong_Main(username);
                            tp.Show();
                            this.Close();
                            break;

                        case "QL trực tiếp":
                            //QLTrucTiep_Main ql = new QLTrucTiep_Main(username);
                            //ql.Show();
                            //this.Close();
                            break;
                    }


                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }
        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login();
            }
        }

        private void DangNhap_Click(object sender, RoutedEventArgs e)
        {
            Login();

        }

        private void tb_password_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tb_password.Password) && tb_password.Password.Length > 0)
            {
                lb_password.Visibility = Visibility.Collapsed;
            }
            else
            {
                lb_password.Visibility = Visibility.Visible;
            }
        }

        private void bt_mini_click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void bt_close_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Windows_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Windows_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

    }
}
