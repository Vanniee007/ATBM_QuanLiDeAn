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
    public partial class TruongPhong_Main : Window
    {
        string username;
        public TruongPhong_Main(string username_)
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
        

        private void PB_datagird_Loaded(object sender, RoutedEventArgs e)
        {
            NhanVien_Main.PB_get_DSPhongBan(PB_datagird);
        }
        /*=============================================TABITEM: ============================================
      * =========================================================================================================*/
  
        private void DA_datagird_Loaded(object sender, RoutedEventArgs e)
        {
            NhanVien_Main.DA_get_DSDeAn(DA_datagird);
        }


        /*=============================================TABITEM:NHANVIEN============================================* =========================================================================================================*/
        private void NV_LayDanhSach_NhanVien()
        {
            try
            {
                DataTable table_User;
                string sql;
                sql = "select * from ATBM_ADMIN.TP_NHANVIEN";
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
        private void PC_Get_Data()
        {
            try
            {
                DataTable  table_User = new DataTable();
                string sql;
                sql = "select * from ATBM_ADMIN.TP_PHANCONG";
                table_User = Class.DB_Config.GetDataToTable(sql); //Đọc dữ liệu từ bảng
                PC_datagird.ItemsSource = null;
                PC_datagird.ItemsSource = table_User.DefaultView; //Nguồn dữ liệu
                PC_LayDanhSach_DoAn();
                PC_LayDanhSach_NhanVien();
            }
            catch { }

        }


        private void PC_datagird_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (PC_datagird.SelectedIndex.ToString() != null)
                {
                    DataRowView rowview = (DataRowView)PC_datagird.SelectedItem;
                    if (rowview != null)
                    {
                        PC_Combobox_MaNV.Text= rowview["MANV"].ToString();
                        PC_Combobox_MaDA.Text= rowview["MADA"].ToString();
                        PC_tb_ThoiGian.Text = SupportFunction.FormatShortDate(rowview["THOIGIAN"].ToString());
                    }
                }
            }
            catch
            { }
        }

        private void PC_LayDanhSach_NhanVien()
        {
            try
            {
                DataTable table_User;
                string sql;
                sql = "select MaNV from ATBM_ADMIN.TP_NHANVIEN";
                table_User = Class.DB_Config.GetDataToTable(sql);
                var ds = new List<string>();
                DataRow r;
                for (int i = 0; i < table_User.Rows.Count; i++)
                {
                    r = table_User.Rows[i];
                    ds.Add(r[0].ToString());
                }
                PC_Combobox_MaNV.DataContext = ds;
            }
            catch { }
        }
        private void PC_LayDanhSach_DoAn()
        {
            try
            {
                DataTable table_User;
                string sql;
                //sql = "select distinct MADA from ATBM_ADMIN.NV_DEAN";
                sql = "select distinct MADA from ATBM_ADMIN.NV_XemThongTinDeAn";
                table_User = Class.DB_Config.GetDataToTable(sql);
                var ds = new List<string>();
                DataRow r;
                for (int i = 0; i < table_User.Rows.Count; i++)
                {
                    r = table_User.Rows[i];
                    ds.Add(r[0].ToString());
                }
                PC_Combobox_MaDA.DataContext = ds;
            }
            catch { }
        }

        private void PC_datagird_Loaded(object sender, RoutedEventArgs e)
        {
            PC_Get_Data();
        }

        private void PC_Combobox_MaNV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataTable table_User;
                string sql;
                //sql = "select distinct MADA from ATBM_ADMIN.NV_DEAN";
                sql = "select TENNV from ATBM_ADMIN.TP_NHANVIEN where MANV = '"+PC_Combobox_MaNV.SelectedItem.ToString()+"'";
                table_User = Class.DB_Config.GetDataToTable(sql);
                PC_tb_TenNV.Text = table_User.Rows[0][0].ToString();   
            }
            catch { }
        }
        private void PC_Combobox_MaDA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataTable table_User;
                string sql;
                //sql = "select distinct MADA from ATBM_ADMIN.NV_DEAN";
                sql = "select TENDA, NGAYBD from ATBM_ADMIN.NV_XemThongTinDeAn where MADA = '" + PC_Combobox_MaDA.SelectedItem + "'";
                table_User = Class.DB_Config.GetDataToTable(sql);
                PC_tb_TenDA.Text = table_User.Rows[0][0].ToString();
                PC_tb_NgayBD.Text = SupportFunction.FormatShortDate(table_User.Rows[0][1].ToString());
            }
            catch { }
        }

        private void PC_tb_Xoa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable table_User;
                string sql;
                //sql = "select distinct MADA from ATBM_ADMIN.NV_DEAN";
                sql = "select * from ATBM_ADMIN.TP_PHANCONG where MADA = '" + PC_Combobox_MaDA.Text + "' and MANV = '" + PC_Combobox_MaNV.Text + "'";
                table_User = Class.DB_Config.GetDataToTable(sql);
                if (table_User.Rows.Count > 0)
                {
                    Class.DB_Config.RunSqlDel("ALTER SESSION SET \"_ORACLE_SCRIPT\" = TRUE");
                    
                    sql = "begin ATBM_ADMIN.TP_XoaPhanCong('" +  PC_Combobox_MaNV.Text + "','"+ PC_Combobox_MaDA.Text + "'); end;";
                    bool kq = Class.DB_Config.RunSQL(sql);
                    PC_Get_Data();
                    if (kq)
                    {
                        SupportFunction.ShowSuccess(lb_error, "Xoá phân công thành công");
                    }
                    else
                    {

                        SupportFunction.ShowError(lb_error, "Xoá phân công thất bại");
                    }
                }
                else
                {
                    SupportFunction.ShowError(lb_error, "Vui lòng chọn đúng dòng cần xoá");
                }
            }
            catch { }



        }

        private void PC_tb_Luu_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (!InputValidation.ValidUsername(PC_Combobox_MaDA.Text))
                {
                    SupportFunction.ShowError(lb_error, "Mã đề án không hợp lệ");
                    return;
                }
                if (!InputValidation.ValidUsername(PC_Combobox_MaNV.Text))
                {
                    SupportFunction.ShowError(lb_error, "Mã nhân viên không hợp lệ");
                    return;
                }
                if (!InputValidation.ValidDate(PC_tb_ThoiGian.Text))
                {
                    SupportFunction.ShowError(lb_error, "Thời gian không hợp lệ");
                    return;
                }
                DataTable table_User;

                string sql;
                //sql = "select distinct MADA from ATBM_ADMIN.NV_DEAN";
                sql = "select * from ATBM_ADMIN.TP_PHANCONG where MADA = '" + PC_Combobox_MaDA.Text + "' and MANV = '"+PC_Combobox_MaNV.Text+"'";
                table_User = Class.DB_Config.GetDataToTable(sql);
                if (table_User.Rows.Count > 0)
                {
                    //sửa
                    sql = "begin ATBM_ADMIN.TP_SuaPhanCong('" + PC_Combobox_MaNV.Text + "','" + PC_Combobox_MaDA.Text + "',TO_DATE('"+PC_tb_ThoiGian.Text+ "','dd/mm/yyyy')); end;";
                    Class.DB_Config.RunSqlDel("ALTER SESSION SET \"_ORACLE_SCRIPT\" = TRUE");
                    bool kq = Class.DB_Config.RunSQL(sql);
                    PC_Get_Data();
                    if (kq)
                    {
                        SupportFunction.ShowSuccess(lb_error, "Sửa phân công thành công");
                    }
                    else
                    {
                        SupportFunction.ShowError(lb_error, "Sửa phân công thất bại");
                    }

                }
                else
                {
                    //thêm
                    sql = "BEGIN ATBM_ADMIN.TP_ThemPhanCong('" + PC_Combobox_MaNV.Text + "','" + PC_Combobox_MaDA.Text + "',TO_DATE('" + PC_tb_ThoiGian.Text + "','dd/mm/yyyy')); end;";
                    Class.DB_Config.RunSqlDel("ALTER SESSION SET \"_ORACLE_SCRIPT\" = TRUE");
                    bool kq = Class.DB_Config.RunSQL(sql);
                    PC_Get_Data();
                    PC_Get_Data();
                    if (kq)
                    {
                        SupportFunction.ShowSuccess(lb_error, "Thêm phân công thành công");
                    }
                    else
                    {
                        SupportFunction.ShowError(lb_error, "Thêm phân công thất bại");
                    }
                }
            }
            catch { }
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
