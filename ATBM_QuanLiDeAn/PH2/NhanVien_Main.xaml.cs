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

        private void PB_Get_Data()
        {
            try
            {
                DataTable table_User;
                string sql;
                sql = "select * from ATBM_ADMIN.TP_PHANCONG";
                //if (cbOnly.IsChecked == false)
                //{
                //    sql = sql + " AND USERNAME LIKE 'U%'";
                //}
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
                        PC_tb_NgayBD.Text = rowview["NGAYBD"].ToString();
                        PC_tb_ThoiGian.Text = rowview["THOIGIAN"].ToString();
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
                sql = "select distinct MADA from ATBM_ADMIN.DEAN";
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
            PB_Get_Data();
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

        private void PC_Combobox_MaDA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataTable table_User;
                string sql;
                //sql = "select distinct MADA from ATBM_ADMIN.NV_DEAN";
                sql = "select TENDA, NGAYBD from ATBM_ADMIN.DEAN where MADA = '" + PC_Combobox_MaDA.Text + "'";
                table_User = Class.DB_Config.GetDataToTable(sql);
                PC_tb_TenDA.Text = table_User.Rows[0][0].ToString();
                PC_tb_NgayBD.Text = table_User.Rows[0][1].ToString();
            }
            catch { }
        }

        private void PC_tb_Xoa_Click(object sender, RoutedEventArgs e)
        {
            DataTable table_User;
            string sql;
            //sql = "select distinct MADA from ATBM_ADMIN.NV_DEAN";
            sql = "select * from ATBM_ADMIN.TP_PHANCONG where MADA = '" + PC_Combobox_MaDA.Text + "' and MANV = '" + PC_Combobox_MaNV.Text + "'";
            table_User = Class.DB_Config.GetDataToTable(sql);
            if (table_User.Rows.Count > 0)
            {
                MessageBox.Show("xoá" + PC_Combobox_MaDA.Text + "/" + PC_Combobox_MaNV.Text);
            }
            else
            {
                MessageBox.Show("vui lòng chọn đúng dòng cần xoá");

            }


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
                    MessageBox.Show("sửa"+ PC_Combobox_MaDA.Text + "/" + PC_Combobox_MaNV.Text);
                }
                else
                {
                    //thêm
                    MessageBox.Show("thêm"+ PC_Combobox_MaDA.Text + "/" + PC_Combobox_MaNV.Text);
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

    }
}
