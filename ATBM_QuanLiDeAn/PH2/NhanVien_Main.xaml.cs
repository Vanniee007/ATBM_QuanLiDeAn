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
            Login_Window lg = new Login_Window(username);
            this.Close();
            lg.Show();

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


        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Tt_capnhatthongtincanhan_click(object sender, RoutedEventArgs e)
        {
            try
            {
                //sửa
                string sql = "begin ATBM_ADMIN.NV_SUATHONGTIN(TO_DATE('" + TT_tb_ngaysinh.Text + "','dd/mm/yyyy'),'" + TT_tb_diachi.Text + "','" + TT_tb_sodienthoai.Text + "'); end;";
                Class.DB_Config.RunSqlDel("ALTER SESSION SET \"_ORACLE_SCRIPT\" = TRUE");
                bool kq = Class.DB_Config.RunSQL(sql);

                if (kq)
                {
                    Tt_lb_thongtincanhan_errorout.Content = "Sửa thông tin thành công";
                    Tt_lb_thongtincanhan_errorout.Foreground = Brushes.Green;
                }
                else
                {
                    Tt_lb_thongtincanhan_errorout.Content = "Sửa thông tin thất bại";
                    Tt_lb_thongtincanhan_errorout.Foreground = Brushes.IndianRed;
                }

            }
            catch { }
        
        }

        private void Tt_doimatkhau_loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Tt_capnhatmatkhau_click(object sender, RoutedEventArgs e)
        {

        }


        
      
        private void TT_Load()
        {

            try
            {
                DataTable table_User;
                string sql;
                //sql = "select distinct MADA from ATBM_ADMIN.NV_DEAN";
                sql = "select * from ATBM_ADMIN.NV_XemThongTinChinhMinh";
                table_User = Class.DB_Config.GetDataToTable(sql);
                TT_tb_magv.Text = table_User.Rows[0]["MANV"].ToString();
                TT_tb_hoten.Text = table_User.Rows[0]["TENNV"].ToString();
                TT_tb_ngaysinh.Text = table_User.Rows[0]["NGAYSINH"].ToString();
                TT_tb_gioitinh.Text = table_User.Rows[0]["PHAI"].ToString();
                TT_tb_diachi.Text = table_User.Rows[0]["DIACHI"].ToString();
                TT_tb_sodienthoai.Text = "0" + table_User.Rows[0]["SODT"].ToString();
                TT_tb_luong.Text = table_User.Rows[0]["LUONG"].ToString();
                TT_tb_phucap.Text = table_User.Rows[0]["PHUCAP"].ToString();
                TT_tb_vaitro.Text = table_User.Rows[0]["VAITRO"].ToString();
                TT_tb_phongban.Text = table_User.Rows[0]["PHG"].ToString();
            }
            catch { }
        }
        private void TT_Tabitem_Loaded(object sender, RoutedEventArgs e)
        {
            TT_Load();
        }
        /*=============================================TABITEM: CONGVIEC============================================
         * =========================================================================================================*/
        private void CV_loaded(object sender, RoutedEventArgs e)
        {
            CV_get_DSCongViec();
        }
        private void CV_get_DSCongViec()
        {
            try
            {
                DataTable table_User;
                string sql;
                sql = "select * from ATBM_ADMIN.NV_XemThongTinPhanCong";
                table_User = Class.DB_Config.GetDataToTable(sql); //Đọc dữ liệu từ bảng
                CV_datagird.ItemsSource = null;
                CV_datagird.ItemsSource = table_User.DefaultView; //Nguồn dữ liệu

            }
            catch { }

        }
        private void CV_datagird_Loaded(object sender, RoutedEventArgs e)
        {
            CV_get_DSCongViec();
        }
        /*=============================================TABITEM: PHONGBAN============================================
        * =========================================================================================================*/
        private void PB_get_DSPhongBan()
        {
            try
            {
                DataTable table_User;
                string sql;
                sql = "select * from ATBM_ADMIN.NV_XemThongTinPhongBan";
                table_User = Class.DB_Config.GetDataToTable(sql); //Đọc dữ liệu từ bảng
                PB_datagird.ItemsSource = null;
                PB_datagird.ItemsSource = table_User.DefaultView; //Nguồn dữ liệu

            }
            catch { }

        }
        private void PB_loaded(object sender, RoutedEventArgs e)
        {
            PB_get_DSPhongBan();
        }

        private void PB_datagird_Loaded(object sender, RoutedEventArgs e)
        {
            PB_get_DSPhongBan();
        }
        /*=============================================TABITEM: ============================================
      * =========================================================================================================*/
        private void DA_get_DSDeAn()
        {
            try
            {
                DataTable table_User;
                string sql;
                sql = "select * from ATBM_ADMIN.NV_XemThongTinDeAn";
                table_User = Class.DB_Config.GetDataToTable(sql); //Đọc dữ liệu từ bảng
                DA_datagird.ItemsSource = null;
                DA_datagird.ItemsSource = table_User.DefaultView; //Nguồn dữ liệu

            }
            catch { }

        }
        private void DA_loaded(object sender, RoutedEventArgs e)
        {
            DA_get_DSDeAn();
        }

        private void DA_datagird_Loaded(object sender, RoutedEventArgs e)
        {
            DA_get_DSDeAn();
        }
    }
}
