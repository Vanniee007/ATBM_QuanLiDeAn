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
    public partial class NhanSu_Main : Window
    {
        string username;
        public NhanSu_Main(string username_)
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
            NV_lb_title.Content = "Nhân viên "+ TT_tb_phongban.Text;
        }
        /*=============================================TABITEM: CONGVIEC============================================
         * =========================================================================================================*/
 
    
        private void CV_datagird_Loaded(object sender, RoutedEventArgs e)
        {
            NhanVien_Main.CV_get_DSCongViec(CV_datagird);
        }
        /*=============================================TABITEM:PHONGBAN============================================
        * =========================================================================================================*/
        
        private void PB_Get_Data()
        {
            NhanVien_Main.PB_get_DSPhongBan(PB_datagird);
            PB_LayDanhSach_TruongPhong();
            PB_LayDanhSach_PhongBan(PB_Combobox_MaPhong);
        }
        private void PB_datagird_Loaded(object sender, RoutedEventArgs e)
        {
            PB_Get_Data();
        }
        /*=============================================TABITEM: ============================================
      * =========================================================================================================*/
  
        private void DA_datagird_Loaded(object sender, RoutedEventArgs e)
        {
            NhanVien_Main.DA_get_DSDeAn(DA_datagird);
        }


        /*=============================================TABITEM:NHANVIEN============================================* =========================================================================================================*/
        private  void NV_LayDanhSach_NhanVien()
        {
            try
            {
                DataTable table_User;
                string sql;
                sql = "select * from ATBM_ADMIN.NS_XEMNHANVIEN";
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
        /*============================================= TABITEM: PHAN CONG ============================================
        */



        private void PB_LayDanhSach_TruongPhong()
        {
            try
            {
                DataTable table_User;
                string sql;
                sql = "select MANV from ATBM_ADMIN.NS_XEMNHANVIEN";
                table_User = Class.DB_Config.GetDataToTable(sql);
                var ds = new List<string>();
                DataRow r;
                for (int i = 0; i < table_User.Rows.Count; i++)
                {
                    r = table_User.Rows[i];
                    ds.Add(r[0].ToString());
                }
                PB_Combobox_MaTruongPhong.DataContext = ds;
            }
            catch { }
        }
        public static void PB_LayDanhSach_PhongBan(ComboBox Combobox_)
        {
            try
            {
                DataTable table_User;
                string sql;
                sql = "select MAPHG from ATBM_ADMIN.PHONGBAN";
                table_User = Class.DB_Config.GetDataToTable(sql);
                var ds = new List<string>();
                DataRow r;
                for (int i = 0; i < table_User.Rows.Count; i++)
                {
                    r = table_User.Rows[i];
                    ds.Add(r[0].ToString());
                }
                Combobox_.DataContext = ds;
            }
            catch { }
        }

        private void PB_datagird_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (PB_datagird.SelectedIndex.ToString() != null)
                {
                    DataRowView rowview = (DataRowView)PB_datagird.SelectedItem;
                    if (rowview != null)
                    {
                        PB_Combobox_MaPhong.Text = rowview["MAPHG"].ToString();
                        PB_tb_TenPhong.Text = rowview["TENPHG"].ToString();
                        PB_Combobox_MaTruongPhong.Text = rowview["TRPHG"].ToString();
                    }
                }
            }
            catch
            { }
        }

        private void PB_Combobox_MaTruongPhong_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataTable table_User;
                string sql;
                //sql = "select distinct MADA from ATBM_ADMIN.NV_DEAN";
                sql = "select TENNV from ATBM_ADMIN.NS_XEMNHANVIEN where MANV = '" + PB_Combobox_MaTruongPhong.SelectedItem.ToString() + "'";
                table_User = Class.DB_Config.GetDataToTable(sql);
                PB_tb_TenTruongPhong.Text = table_User.Rows[0][0].ToString();
            }
            catch { }
        }


        private void PB_tb_Luu_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                if (!InputValidation.ValidUsername(PB_Combobox_MaPhong.Text))
                {
                    SupportFunction.ShowError(lb_error, "Mã phòng không hợp lệ");
                    return;
                }
                if (!InputValidation.ValidUsername(PB_Combobox_MaTruongPhong.Text))
                {
                    SupportFunction.ShowError(lb_error, "Mã nhân viên không hợp lệ");
                    return;
                }
                DataTable table_User;
                string sql;
                sql = "select * from ATBM_ADMIN.PHONGBAN where MAPHG = '" + PB_Combobox_MaPhong.Text + "'";
                table_User = Class.DB_Config.GetDataToTable(sql);
                if (table_User.Rows.Count > 0)
                {
                    //sửa
                    sql = "begin ATBM_ADMIN.NS_SUA_PHONGBAN('" + PB_Combobox_MaPhong.Text + "','" + PB_tb_TenPhong.Text + "','" + PB_Combobox_MaTruongPhong.Text + "'); end;";
                    Class.DB_Config.RunSqlDel("ALTER SESSION SET \"_ORACLE_SCRIPT\" = TRUE");
                    bool kq = Class.DB_Config.RunSQL(sql);
                    PB_Get_Data();
                    if (kq)
                    {
                        SupportFunction.ShowSuccess(lb_error, "Sửa phòng ban thành công");
                    }
                    else
                    {
                        SupportFunction.ShowError(lb_error, "Sửa phòng ban thất bại");
                    }

                }
                else
                {
                    //thêm
                    sql = "begin ATBM_ADMIN.NS_THEM_PHONGBAN('" + PB_Combobox_MaPhong.Text + "','" + PB_tb_TenPhong.Text + "','" + PB_Combobox_MaTruongPhong.Text + "'); end;";
                    Class.DB_Config.RunSqlDel("ALTER SESSION SET \"_ORACLE_SCRIPT\" = TRUE");
                    bool kq = Class.DB_Config.RunSQL(sql);
                    if (kq)
                    {
                        SupportFunction.ShowSuccess(lb_error, "Thêm phòng ban thành công");
                        PB_Get_Data();
                    }
                    else
                    {
                        SupportFunction.ShowError(lb_error, "Thêm phòng ban thất bại");
                    }
                }
            }
            catch { }
        }

        private void NV_tb_Them_Click(object sender, RoutedEventArgs e)
        {
            string username__ = "";
            try
            {
                if (PB_datagird.SelectedIndex.ToString() != null)
                {
                    DataRowView rowview = (DataRowView)NV_datagird.SelectedItem;
                    if (rowview != null)
                    {
                        username__ = rowview["MANV"].ToString();
                    }
                }
            }
            catch
            { }
            NhanSu_ThemNhanVien ns_them = new NhanSu_ThemNhanVien("THEMNHANVIEN", "");
            ns_them.ShowDialog();
            NV_LayDanhSach_NhanVien();


        }

        private void NV_tb_Sua_Click(object sender, RoutedEventArgs e)
        {
            string username__ = "";
            try
            {
                if (PB_datagird.SelectedIndex.ToString() != null)
                {
                    DataRowView rowview = (DataRowView)NV_datagird.SelectedItem;
                    if (rowview != null)
                    {
                        username__ = rowview["MANV"].ToString();
                    }
                }
                else
                {
                    SupportFunction.ShowError(lb_error, "Vui lòng chọn nhân viên cần sửa");
                }
            }
            catch
            { }
            NhanSu_ThemNhanVien ns_them = new NhanSu_ThemNhanVien("SUANHANVIEN", username__);
            ns_them.ShowDialog();
            NV_LayDanhSach_NhanVien();
        }

        private void Windows_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
