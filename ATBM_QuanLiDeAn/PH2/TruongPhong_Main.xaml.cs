using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
    public partial class TruongPhong_Main : Window
    {
        InputValidation validation = new InputValidation();
        string username;
        public TruongPhong_Main(string username_)
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
                if (!validation.ValidUsername(TT_tb_ngaysinh.Text))
                {
                    //lb_error.Content = "Tên đăng nhập không hợp lệ";
                    //lb_error.Foreground = Brushes.IndianRed;
                    return;
                }
                if (!validation.ValidPassword(TT_tb_diachi.Text))
                {
                    //lb_error.Content = "Mật khẩu không hợp lệ";
                    //lb_error.Foreground = Brushes.IndianRed;
                    return;
                }
                if (!validation.ValidPassword(TT_tb_sodienthoai.Text))
                {
                    //lb_error.Content = "Mật khẩu không hợp lệ";
                    //lb_error.Foreground = Brushes.IndianRed;
                    return;
                }

                MessageBox.Show("Cập nhật thông tin");
            }
            catch { }
        }

        private void Tt_doimatkhau_loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Tt_capnhatmatkhau_click(object sender, RoutedEventArgs e)
        {

        }


        private void PB_Get_DSUser()
        {
            try
            {
                DataTable table_User;
                string sql;
                sql = "select * from ATBM_ADMIN.TP_NHANVIEN";
                //if (cbOnly.IsChecked == false)
                //{
                //    sql = sql + " AND USERNAME LIKE 'U%'";
                //}
                table_User = Class.DB_Config.GetDataToTable(sql); //Đọc dữ liệu từ bảng
                PB_datagird.ItemsSource = null;
                PB_datagird.ItemsSource = table_User.DefaultView; //Nguồn dữ liệu

            }
            catch { }

        }
        private void PB_loaded(object sender, RoutedEventArgs e)
        {
            PB_Get_DSUser();
        }

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

        private void CV_loaded(object sender, RoutedEventArgs e)
        {
        }
        private void CV_Get_Data()
        {
            try
            {
                DataTable table_User;
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
        private void PC_loaded(object sender, RoutedEventArgs e)
        {
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
                        //PC_tb_TenNV.Text = rowview["MANV"].ToString();
                        PC_Combobox_MaDA.Text= rowview["MADA"].ToString();
                        //PC_tb_TenDA.Text = rowview["MADA"].ToString();
                        //PC_tb_NgayBD.Text = rowview["NGAYBD"].ToString();
                        PC_tb_ThoiGian.Text = FormatShortDate(rowview["THOIGIAN"].ToString());
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
                sql = "select MANV from ATBM_ADMIN.TP_NHANVIEN";
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
        private void CV_datagird_Loaded(object sender, RoutedEventArgs e)
        {
            CV_Get_Data();
        }

        private void PC_Combobox_MaNV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataTable table_User;
                string sql;
                //sql = "select distinct MADA from ATBM_ADMIN.NV_DEAN";
                sql = "select TENNV from ATBM_ADMIN.TP_NHANVIEN where MANV = '"+PC_Combobox_MaNV.Text+"'";
                table_User = Class.DB_Config.GetDataToTable(sql);
                PC_tb_TenNV.Text = table_User.Rows[0][0].ToString();   
            }
            catch { }
        }
        private string FormatShortDate(string dateTimeString)
        {
            DateTime dateTime;

            // Using DateTime.Parse()
            dateTime = DateTime.Parse(dateTimeString);

            // Using DateTime.TryParse()
            if (DateTime.TryParse(dateTimeString, out dateTime))
            {
                // Conversion successful
                // Format the DateTime object to date string
                string dateString = dateTime.ToString("dd/MM/yyyy");
                return dateString;
            }
            return dateTimeString;
        }
        private void PC_Combobox_MaDA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataTable table_User;
                string sql;
                //sql = "select distinct MADA from ATBM_ADMIN.NV_DEAN";
                sql = "select TENDA, NGAYBD from ATBM_ADMIN.NV_XemThongTinDeAn where MADA = '" + PC_Combobox_MaDA.Text + "'";
                table_User = Class.DB_Config.GetDataToTable(sql);
                PC_tb_TenDA.Text = table_User.Rows[0][0].ToString();
                PC_tb_NgayBD.Text = FormatShortDate(table_User.Rows[0][1].ToString());
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
                        PC_lb_error.Content = "Xoá phân công thành công";
                    }
                    else
                    {
                        PC_lb_error.Content = "Xoá phân công thất bại";
                    }
                }
                else
                {
                    MessageBox.Show("vui lòng chọn đúng dòng cần xoá");

                }
            }
            catch { }


        }

        private void PC_tb_Luu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
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
                        PC_lb_error.Content = "Sửa phân công thành công";
                    }
                    else
                    {
                        PC_lb_error.Content = "Sửa phân công thất bại";
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
                        PC_lb_error.Content = "Thêm phân công thành công";
                    }
                    else
                    {
                        PC_lb_error.Content = "Thêm phân công thất bại";
                    }

                }

            }
            catch { }
        }

        private void PC_tb_ThoiGian_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void TT_Load()
        {

            try
            {
                DataTable table_User;
                string sql;
                //sql = "select distinct MADA from ATBM_ADMIN.NV_DEAN";
                sql = "select * from ATBM_ADMIN.TP_NHANVIEN  where MANV = SYS_CONTEXT('USERENV', 'SESSION_USER')";
                table_User = Class.DB_Config.GetDataToTable(sql);
                TT_tb_magv.Text = table_User.Rows[0]["MANV"].ToString();
                TT_tb_hoten.Text = table_User.Rows[0]["TENNV"].ToString();
                TT_tb_ngaysinh.Text = table_User.Rows[0]["NGAYSINH"].ToString();
                TT_cb_gioitinh.Text = table_User.Rows[0]["PHAI"].ToString();
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

        private void DA_datagird_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
