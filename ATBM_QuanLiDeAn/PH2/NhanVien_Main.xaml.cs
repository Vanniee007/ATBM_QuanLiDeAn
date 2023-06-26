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

namespace ATBM_QuanLiDeAn.PH2
{
    /// <summary>
    /// </summary>
    public partial class NhanVien_Main : Window
    {
        InputValidation validation = new InputValidation();
        string username;
        public NhanVien_Main(string username_)
        {
            InitializeComponent();
            username = username_;
        }
        private void Btn_dangxuat_Click(object sender, RoutedEventArgs e)
        {
            Class.DB_Config.Disconnect(); 
            Login_Window lg = new Login_Window(username);
            this.Close();
            lg.Show();

        }
        private void lb_information_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TT_tabitem.Focus();
        }
        private void bt_mini_click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void bt_max_click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Normal)
            {
                this.WindowState = System.Windows.WindowState.Maximized;
            }
            else
            {
                this.WindowState = System.Windows.WindowState.Normal;
            }
        }

        private void bt_close_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        /*=============================================TABITEM: THONG TIN ============================================
         * =========================================================================================================*/

        private void Tt_capnhatthongtincanhan_click(object sender, RoutedEventArgs e)
        {
            TT_ThayDoiTTCaNhan(TT_tb_ngaysinh, TT_tb_diachi, TT_tb_sodienthoai, lb_error);

        }
        private void Tt_doimatkhau_loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Tt_capnhatmatkhau_click(object sender, RoutedEventArgs e)
        {

        }


        //có thể dùng lại ở các role khác
        public static void TT_ThayDoiTTCaNhan(TextBox ngaySinhTextBox, TextBox diaChiTextBox, TextBox soDienThoaiTextBox, Label lb_error)
        {
            try
            {
                string ngaySinh = ngaySinhTextBox.Text;
                string diaChi = diaChiTextBox.Text;
                string soDienThoai = soDienThoaiTextBox.Text;

                if (!InputValidation.ValidDate(ngaySinh))
                {
                    SupportFunction.ShowError(lb_error, "Ngày sinh không hợp lệ");
                    return;
                }
                if (!InputValidation.ValidPhoneNumber(soDienThoai))
                {
                    SupportFunction.ShowError(lb_error, "Số điện thoại không hợp lệ");
                    return;
                }

                string sql = "begin ATBM_ADMIN.NV_SUATHONGTIN(TO_DATE('" + ngaySinh + "','dd/mm/yyyy'),'" + diaChi + "','" + soDienThoai + "'); end;";
                Class.DB_Config.RunSqlDel("ALTER SESSION SET \"_ORACLE_SCRIPT\" = TRUE");
                bool kq = Class.DB_Config.RunSQL(sql);

                if (kq)
                {
                    SupportFunction.ShowSuccess(lb_error, "Sửa thông tin thành công");
                }
                else
                {
                    SupportFunction.ShowError(lb_error, "Sửa thông tin thất bại");
                }
            }
            catch { }
        }

        //có thể dùng lại ở các role khác
        public static void TT_Load(TextBox Ma, TextBox Ten, TextBox NS, TextBox GioiTinh, TextBox DiaChi, TextBox SDT, TextBox Luong, TextBox PhuCap, TextBox VaiTro, TextBox PhongBan, Label lb_information)
        {

            try
            {
                DataTable table_User;
                string sql;
                //sql = "select distinct MADA from ATBM_ADMIN.NV_DEAN";
                sql = "select * from ATBM_ADMIN.NV_XemThongTinChinhMinh";
                table_User = Class.DB_Config.GetDataToTable(sql);
                Ma.Text = table_User.Rows[0]["MANV"].ToString();
                Ten.Text = table_User.Rows[0]["TENNV"].ToString();
                NS.Text = SupportFunction.FormatShortDate(table_User.Rows[0]["NGAYSINH"].ToString());
                GioiTinh.Text = table_User.Rows[0]["PHAI"].ToString();
                DiaChi.Text = table_User.Rows[0]["DIACHI"].ToString();

                SDT.Text = "0" + table_User.Rows[0]["SODT"].ToString();
                Luong.Text = table_User.Rows[0]["LUONG"].ToString();
                PhuCap.Text = table_User.Rows[0]["PHUCAP"].ToString();
                VaiTro.Text = table_User.Rows[0]["VAITRO"].ToString();
                PhongBan.Text = table_User.Rows[0]["PHG"].ToString();
                lb_information.Content = "Xin chào, " + Ten.Text;
            }
            catch { }
        }
        private void TT_Tabitem_Loaded(object sender, RoutedEventArgs e)
        {
            TT_Load(TT_tb_manv, TT_tb_hoten, TT_tb_ngaysinh, TT_tb_gioitinh, TT_tb_diachi, TT_tb_sodienthoai, TT_tb_luong, TT_tb_phucap, TT_tb_vaitro, TT_tb_phongban, lb_information);
        }




        /*=============================================TABITEM: CONGVIEC============================================
         * =========================================================================================================*/
       
        //có thể dùng lại ở các role khác
        public static void CV_get_DSCongViec(DataGrid CV_datagird)
        {
            try
            {
                DataTable table_User;
                string sql;
                sql = "select * from ATBM_ADMIN.NV_XemThongTinPhanCong";
                table_User = Class.DB_Config.GetDataToTable(sql); // Đọc dữ liệu từ bảng
                CV_datagird.ItemsSource = null;
                CV_datagird.ItemsSource = table_User.DefaultView; // Nguồn dữ liệu
            }
            catch { }
        }
        private void CV_datagird_Loaded(object sender, RoutedEventArgs e)
        {
            CV_get_DSCongViec(CV_datagird);
        }



        /*=============================================TABITEM: PHONGBAN============================================
        * =========================================================================================================*/

        //có thể dùng lại ở các role khác
        public static void PB_get_DSPhongBan(DataGrid dataGrid)
        {
            try
            {
                DataTable table_User;
                string sql;
                sql = "select * from ATBM_ADMIN.NV_XemThongTinPhongBan";
                table_User = Class.DB_Config.GetDataToTable(sql); // Đọc dữ liệu từ bảng
                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = table_User.DefaultView; // Nguồn dữ liệu
            }
            catch { }
        }


        private void PB_datagird_Loaded(object sender, RoutedEventArgs e)
        {
            PB_get_DSPhongBan(PB_datagird);
        }
        /*=============================================TABITEM: ============================================
      * =========================================================================================================*/
        public static void DA_get_DSDeAn(DataGrid dataGrid)
        {
            try
            {
                DataTable table_User;
                string sql;
                sql = "select * from ATBM_ADMIN.NV_XemThongTinDeAn";
                table_User = Class.DB_Config.GetDataToTable(sql); // Đọc dữ liệu từ bảng
                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = table_User.DefaultView; // Nguồn dữ liệu
            }
            catch { }
        }

        private void DA_datagird_Loaded(object sender, RoutedEventArgs e)
        {
            DA_get_DSDeAn(DA_datagird);
        }

    }
}
