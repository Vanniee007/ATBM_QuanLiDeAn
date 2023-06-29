using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
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
    public partial class QLTrucTiep_Main : Window
    {
        string username;
        public QLTrucTiep_Main(string username_)
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
            NhanVien_Main.TT_ThayDoiTTCaNhan(TT_tb_ngaysinh, TT_tb_diachi, TT_tb_sodienthoai, lb_error);

        }

        private void Tt_doimatkhau_loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Tt_capnhatmatkhau_click(object sender, RoutedEventArgs e)
        {

        }



        private void TT_Tabitem_Loaded(object sender, RoutedEventArgs e)
        {
            NhanVien_Main.TT_Load(TT_tb_manv, TT_tb_hoten, TT_tb_ngaysinh, TT_tb_gioitinh, TT_tb_diachi, TT_tb_sodienthoai, TT_tb_luong, TT_tb_phucap, TT_tb_vaitro, TT_tb_phongban, lb_information);
        }
        /*=============================================TABITEM: CONGVIEC============================================
         * =========================================================================================================*/


        /*=============================================TABITEM:PHONGBAN============================================
        * =========================================================================================================*/


        /*============================================= TABITEM: NHANVIEN ============================================
        */

        private void NV_LayDanhSach_NhanVien()
        {
            try
            {
                DataTable table_User;
                string sql;
                sql = "select * from ATBM_ADMIN.QL_XEMNHANVIEN";
                table_User = Class.DB_Config.GetDataToTable(sql);
                NV_datagird.ItemsSource = null;
                NV_datagird.ItemsSource = table_User.DefaultView;
            }
            catch { }
        }

        private void NV_datagird_Loaded(object sender, RoutedEventArgs e)
        {
            NV_LayDanhSach_NhanVien();
        }

        private void DA_datagird_Loaded(object sender, RoutedEventArgs e)
        {
            NhanVien_Main.DA_get_DSDeAn(DA_datagird);

        }

        private void PB_datagird_Loaded(object sender, RoutedEventArgs e)
        {
            NhanVien_Main.PB_get_DSPhongBan(PB_datagird);
        }

        private void Windows_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Cv_cb_DSNhanVien_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             
        }

        private void CV_get_DSCongViec(DataGrid dataGrid)
        {
            try
            {
                DataTable table_User;
                string sql;
                sql = "select * from ATBM_ADMIN.QL_XEMPHANCONG";
                table_User = Class.DB_Config.GetDataToTable(sql); // Đọc dữ liệu từ bảng
                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = table_User.DefaultView; // Nguồn dữ liệu

            }
            catch { }
        }
        private void CV_datagird_Loaded(object sender, RoutedEventArgs e)
        {
            CV_get_DSCongViec(CV_datagird);
        }
    }
}
