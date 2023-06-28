using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices;
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
    /// Interaction logic for NhanSu_ThemNhanVien.xaml
    /// </summary>
    public partial class NhanSu_ThemNhanVien : Window
    {
        int LoaiChucNang;
        string username;
        public NhanSu_ThemNhanVien(string ChucNang_, string username_)
        {
            InitializeComponent();
            username = username_;
            tb_manv.Text = username_;
            switch (ChucNang_)
            {
                case "THEMNHANVIEN":
                    LoaiChucNang = 1;
                    lb_title.Content = "Thêm nhân viên";
                    break;
                case "SUANHANVIEN":
                    LoaiChucNang = 2;
                    lb_title.Content = "Chỉnh sửa nhân viên";
                    tb_manv.IsReadOnly = true;
                    tb_manv.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ebecf0"));
                    break;
                case "DANGNHAPLANDAU":
                    LoaiChucNang = 3;
                    lb_title.Content = "Chỉnh sửa thông tin";
                    tb_manv.IsReadOnly = true;
                    tb_manv.Background =new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ebecf0"));

                    break;
            }
        }

        private void capnhatthongtincanhan_click(object sender, RoutedEventArgs e)
        {
            if (!InputValidation.ValidUsername(tb_manv.Text))
            {
                SupportFunction.ShowError(lb_thongtincanhan_errorout, "Mã nhân viên không hợp lệ");
                return;
            }
            if (!InputValidation.ValidDate(tb_ngaysinh.Text))
            {
                SupportFunction.ShowError(lb_thongtincanhan_errorout, "Ngày sinh không hợp lệ");
                return;
            }
            if (!InputValidation.ValidPhoneNumber(tb_sodienthoai.Text))
            {
                SupportFunction.ShowError(lb_thongtincanhan_errorout, "Số điện thoại không hợp lệ");
                return;
            }

            try
            {
                //Nhân sự - Thêm nhân viên mới 
                if (LoaiChucNang == 1)
                {
                    DataTable table_User;
                    string sql;
                    sql = "select * from ATBM_ADMIN.NS_XEMNHANVIEN where MANV = '" + tb_manv.Text + "'";
                    table_User = Class.DB_Config.GetDataToTable(sql);
                    if (table_User.Rows.Count == 0)
                    {
                        //sửa
                        sql = "begin ATBM_ADMIN.NS_THEM_NHANVIEN('" + tb_manv.Text + "','" + tb_hoten.Text + "','" + Combobox_gioitinh.Text + "',TO_DATE('" + tb_ngaysinh.Text + "','dd/mm/yyyy'),'" + tb_diachi.Text + "','" + tb_sodienthoai.Text + "','" + Combobox_vaitro.Text + "','" + Combobox_nguoiQL.Text + "','" + Combobox_phongban.Text + "'); end;";
                        Class.DB_Config.RunSqlDel("ALTER SESSION SET \"_ORACLE_SCRIPT\" = TRUE");
                        bool kq = Class.DB_Config.RunSQL(sql);
                        if (kq)
                        {
                            SupportFunction.ShowSuccess(lb_thongtincanhan_errorout, "Thêm nhân viên thành công");
                        }
                        else
                        {
                            SupportFunction.ShowError(lb_thongtincanhan_errorout, "Thêm nhân viên thất bại");
                        }

                    }
                    else
                    {
                        //Nhân viên đã tồn tại
                        SupportFunction.ShowError(lb_thongtincanhan_errorout, "Nhân viên đã tồn tại");

                    }

                }
                //Nhân sự - Sửa nhân viên 
                if (LoaiChucNang == 2)
                {
                    DataTable table_User;
                    string sql;
                    sql = "select * from ATBM_ADMIN.NS_XEMNHANVIEN where MANV = '" + tb_manv.Text + "'";
                    table_User = Class.DB_Config.GetDataToTable(sql);
                    if (table_User.Rows.Count == 1)
                    {
                        //sửa
                        sql = "begin ATBM_ADMIN.NS_SUA_NHANVIEN('" + tb_manv.Text + "','" + tb_hoten.Text + "','" + Combobox_gioitinh.Text + "',TO_DATE('" + tb_ngaysinh.Text + "','dd/mm/yyyy'),'" + tb_diachi.Text + "','" + tb_sodienthoai.Text + "','" + Combobox_vaitro.Text + "','" + Combobox_nguoiQL.Text + "','" + Combobox_phongban.Text + "'); end;";
                        Class.DB_Config.RunSqlDel("ALTER SESSION SET \"_ORACLE_SCRIPT\" = TRUE");
                        bool kq = Class.DB_Config.RunSQL(sql);
                        if (kq)
                        {
                            SupportFunction.ShowSuccess(lb_thongtincanhan_errorout, "Sửa nhân viên thành công");
                        }
                        else
                        {
                            SupportFunction.ShowError(lb_thongtincanhan_errorout, "Sửa nhân viên thất bại");
                        }

                    }
                    else
                    {
                        //Nhân viên không tồn tại
                        SupportFunction.ShowError(lb_thongtincanhan_errorout, "Nhân viên không tồn tại");

                    }

                }


                //Tất cả nhân viên - Đăng nhập lần đầu 
                if (LoaiChucNang == 3)
                {
                    string sql = "begin ATBM_ADMIN.NV_SUATHONGTIN(TO_DATE('" + tb_ngaysinh.Text + "','dd/mm/yyyy'),'" + tb_diachi.Text + "','" + tb_sodienthoai.Text + "'); end;";
                    Class.DB_Config.RunSqlDel("ALTER SESSION SET \"_ORACLE_SCRIPT\" = TRUE");
                    bool kq = Class.DB_Config.RunSQL(sql);
                    if (kq)
                    {
                        SupportFunction.ShowSuccess(lb_thongtincanhan_errorout, "Sửa thông tin thành công");
                        CauHoiBaoMat c = new CauHoiBaoMat(username,1);
                        c.Show();
                        this.Close();
                        return;
                    }
                    else
                    {
                        SupportFunction.ShowError(lb_thongtincanhan_errorout, "Sửa thông tin thất bại");
                    }
                }




            }
            catch
            {
                SupportFunction.ShowError(lb_thongtincanhan_errorout, "Vui lòng nhập thông tin hợp lệ");
            }
            
        }

        private void NS_LayDanhSach_NguoiQuanLy()
        {
            try
            {
                DataTable table_User;
                string sql;
                if (LoaiChucNang == 3)
                    sql = "select * from ATBM_ADMIN.NV_XemThongTinChinhMinh where MANV = '" + username + "'";
                else
                    sql = "select * from ATBM_ADMIN.NS_XEMNHANVIEN where MANV = '" + username + "'";
                table_User = Class.DB_Config.GetDataToTable(sql);
                var ds = new List<string>();
                DataRow r;
                for (int i = 0; i < table_User.Rows.Count; i++)
                {
                    r = table_User.Rows[i];
                    ds.Add(r[0].ToString());
                }
                Combobox_nguoiQL.DataContext = ds;
            }
            catch { }
        }
        private void tb_phongban_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataTable table_User;
                string sql;
                //sql = "select distinct MADA from ATBM_ADMIN.NV_DEAN";
                sql = "select TENPHG from ATBM_ADMIN.NV_XemThongTinPhongBan where MAPHG = '" + Combobox_phongban.SelectedItem.ToString() + "'";
                table_User = Class.DB_Config.GetDataToTable(sql);
                tb_tenphongban.Text = table_User.Rows[0][0].ToString();
            }
            catch { }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string sql;
                if (LoaiChucNang == 3)
                    sql = "select * from ATBM_ADMIN.NV_XemThongTinChinhMinh where MANV = '" + username + "'";
                else
                    sql = "select * from ATBM_ADMIN.NS_XEMNHANVIEN where MANV = '" + username + "'";
                DataTable table_User;
                table_User = Class.DB_Config.GetDataToTable(sql);
                NhanSu_Main.PB_LayDanhSach_PhongBan(Combobox_phongban);
                NS_LayDanhSach_NguoiQuanLy();
                tb_hoten.Text = table_User.Rows[0]["TENNV"].ToString();
                tb_ngaysinh.Text = SupportFunction.FormatShortDate(table_User.Rows[0]["NGAYSINH"].ToString());
                Combobox_gioitinh.Text = table_User.Rows[0]["PHAI"].ToString();
                tb_diachi.Text = table_User.Rows[0]["DIACHI"].ToString();
                tb_sodienthoai.Text = "0" + table_User.Rows[0]["SODT"].ToString();
                Combobox_vaitro.Text = table_User.Rows[0]["VAITRO"].ToString();
                Combobox_nguoiQL.Text = table_User.Rows[0]["MANQL"].ToString();
                Combobox_phongban.Text = table_User.Rows[0]["PHG"].ToString();

            }

            catch { }

        }

        private void tb_manv_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
