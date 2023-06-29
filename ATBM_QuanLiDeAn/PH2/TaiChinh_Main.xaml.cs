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
    public partial class TaiChinh_Main : Window
    {
        InputValidation validation = new InputValidation();
        string username;
        public TaiChinh_Main(string username_)
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



        /*=============================================TABITEM: THÔNG TIN ============================================
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
                //Giải mã 
                MaHoa mahoa = new MaHoa();
                string manv= table_User.Rows[0]["MANV"].ToString();
                string privateKey = mahoa.LoadPrivateKeyFromOracle(manv);
                string encryptedLuong = table_User.Rows[0]["LUONG"].ToString();
                string encryptedPhuCap = table_User.Rows[0]["PHUCAP"].ToString();
                if (!string.IsNullOrEmpty(privateKey) && !string.IsNullOrEmpty(encryptedLuong) && !string.IsNullOrEmpty(encryptedPhuCap))
                {
                    byte[] encryptedLuongBytes = mahoa.Bytes(encryptedLuong);
                    byte[] encryptedPhuCapBytes = mahoa.Bytes(encryptedPhuCap);
                    string decryptedLuong = mahoa.RSADecrypt(encryptedLuongBytes, privateKey);
                    string decryptedPhuCap = mahoa.RSADecrypt(encryptedPhuCapBytes, privateKey);
                    // Xử lý giá trị mới của cột "Luong"
                    Luong.Text = decryptedLuong;
                    PhuCap.Text = decryptedPhuCap;

                }
             
                //Truyền giữ liệu vào cb, tb...
                Ma.Text = table_User.Rows[0]["MANV"].ToString();
                Ten.Text = table_User.Rows[0]["TENNV"].ToString();
                NS.Text = SupportFunction.FormatShortDate(table_User.Rows[0]["NGAYSINH"].ToString());
                GioiTinh.Text = table_User.Rows[0]["PHAI"].ToString();
                DiaChi.Text = table_User.Rows[0]["DIACHI"].ToString();
                SDT.Text = "0" + table_User.Rows[0]["SODT"].ToString();
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




        /*=============================================TABITEM: CÔNG VIỆC (PHÂN CÔNG)============================================
         * =========================================================================================================*/
       
        //có thể dùng lại ở các role khác
        public static void CV_get_DSCongViec(string LocXem, DataGrid dataGrid)
        {
            try
            {
                DataTable table_User;
                string sql;
                if (LocXem == "Bạn")
                {
                    sql = "select * from ATBM_ADMIN.NV_XemThongTinPhanCong";
                    table_User = Class.DB_Config.GetDataToTable(sql); // Đọc dữ liệu từ bảng
                    dataGrid.ItemsSource = null;
                    dataGrid.ItemsSource = table_User.DefaultView; // Nguồn dữ liệu
                }
                else
                {
                    sql = "select * from ATBM_ADMIN.TC_XEMPHANCONG";
                    table_User = Class.DB_Config.GetDataToTable(sql); // Đọc dữ liệu từ bảng
                    dataGrid.ItemsSource = null;
                    dataGrid.ItemsSource = table_User.DefaultView; // Nguồn dữ liệu
                }
                
            }
            catch { }
        }
        private void CV_datagird_Loaded(object sender, RoutedEventArgs e)
        {
            Cv_cb_LocCongViec.Text = "Bạn";
            CV_get_DSCongViec("Bạn",CV_datagird);
        }

        private void Cv_cb_LocCongViec_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            {
                try
                {
                    ComboBoxItem comboBoxItem = (ComboBoxItem)e.AddedItems[0];
                    string Nguoi = comboBoxItem.Content.ToString();
                    CV_get_DSCongViec(Nguoi, CV_datagird);
                }
                catch
                {
                }
            }
        }
        /*=============================================TABITEM: NHÂN VIÊN (CHỈNH SỬA LƯƠNG, PHỤ CẤP)============================================
     * =========================================================================================================*/
        public static void NV_get_DSNhanVien(DataGrid dataGrid)
        {
            try
            {
                DataTable table_User;
                string sql;
                sql = "select * from ATBM_ADMIN.TC_XEMNHANVIEN";
                table_User = Class.DB_Config.GetDataToTable(sql); // Đọc dữ liệu từ bảng

                // Xử lý và cập nhật dữ liệu cột "Luong" và "PhuCap"
                foreach (DataRow row in table_User.Rows)
                {
                    // Lấy giá trị cũ của cột "Luong"
                    string luong = row["LUONG"].ToString();
                    string phucap = row["PHUCAP"].ToString();
                    string manv = row["MANV"].ToString();

                    // Tạo đối tượng MaHoa
                    MaHoa mahoa = new MaHoa();

                    // Gọi phương thức trên đối tượng mahoa
                    string privateKey = mahoa.LoadPrivateKeyFromOracle(manv);
                    string encryptedLuong = mahoa.GetEncryptedSalaryFromOracle(manv);
                    string encryptedPhuCap = mahoa.GetEncryptedAllowanceFromOracle(manv);

                    if (!string.IsNullOrEmpty(privateKey) && !string.IsNullOrEmpty(encryptedLuong) && !string.IsNullOrEmpty(encryptedPhuCap))
                    {
                        byte[] encryptedLuongBytes = mahoa.Bytes(encryptedLuong);
                        byte[] encryptedPhuCapBytes = mahoa.Bytes(encryptedPhuCap);
                        string decryptedLuong = mahoa.RSADecrypt(encryptedLuongBytes, privateKey);
                        string decryptedPhuCap = mahoa.RSADecrypt(encryptedPhuCapBytes, privateKey);
                        // Xử lý giá trị mới của cột "Luong"

                        luong = decryptedLuong;
                        phucap = decryptedPhuCap;
                    }


                    // Cập nhật giá trị mới vào cột "Luong"
                    row["LUONG"] = luong;
                    row["PHUCAP"] = phucap;
                    // Tương tự, bạn có thể thực hiện xử lý và cập nhật cho cột "PhuCap" hoặc bất kỳ cột nào khác tùy theo yêu cầu của bạn
                }

                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = table_User.DefaultView; // Nguồn dữ liệu

            }
            catch { }
        }

        private void NV_LayDanhSach_NhanVien()
        {
            try
            {
                DataTable table_User;
                string sql;
                sql = "select MANV from ATBM_ADMIN.TC_XEMNHANVIEN";
                table_User = Class.DB_Config.GetDataToTable(sql);
                var ds = new List<string>();
                DataRow r;
                for (int i = 0; i < table_User.Rows.Count; i++)
                {
                    r = table_User.Rows[i];
                    ds.Add(r[0].ToString());
                }
                NV_cb_MaNV.DataContext = ds;
            }
            catch { }
        }
        private void NV_datagird_Loaded(object sender, RoutedEventArgs e)
        {
            NV_get_DSNhanVien(NV_datagird);
            NV_LayDanhSach_NhanVien();
        }
        private void NV_datagird_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (NV_datagird.SelectedIndex.ToString() != null)
                {
                    DataRowView rowview = (DataRowView)NV_datagird.SelectedItem;
                    if (rowview != null)
                    {
                        NV_cb_MaNV.Text = rowview["MANV"].ToString();
                        NV_tb_Luong.Text = rowview["LUONG"].ToString();
                        NV_tb_PhuCap.Text = rowview["PHUCAP"].ToString();
                    }
                }
            }
            catch
            { }
        }



        private void NV_bt_CapNhat_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (!InputValidation.ValidUsername(NV_cb_MaNV.Text))
                {
                    SupportFunction.ShowError(lb_error, "Mã nhân viên không hợp lệ");
                    return;
                }
                if (!InputValidation.ValidCheckLuong_PhuCap(NV_tb_Luong.Text))
                {
                    SupportFunction.ShowError(lb_error, "Lương không hợp lệ");
                    return;
                }
                if (!InputValidation.ValidCheckLuong_PhuCap(NV_tb_PhuCap.Text))
                {
                    SupportFunction.ShowError(lb_error, "Phụ cấp không hợp lệ");
                    return;
                }
                string sql;
                MaHoa mahoa = new MaHoa();
                string publicKey = mahoa.LoadPublicKeyFromOracle(NV_cb_MaNV.Text);
                if (string.IsNullOrEmpty(publicKey))
                {
                    mahoa.GenerateAndSaveKeys(NV_cb_MaNV.Text);
                    publicKey = mahoa.LoadPublicKeyFromOracle(NV_cb_MaNV.Text);
                }
                string luong = NV_tb_Luong.Text;
                string phuCap = NV_tb_PhuCap.Text;
                string encryptedLuong = mahoa.RSAEncrypt(luong, publicKey);
                string encryptedPhuCap = mahoa.RSAEncrypt(phuCap, publicKey);

                sql = "begin ATBM_ADMIN.TC_UPD_LUONG_PHUCAP('" + NV_cb_MaNV.Text + "','" + encryptedLuong + "','" + encryptedPhuCap + "'); end;";
                Class.DB_Config.RunSqlDel("ALTER SESSION SET \"_ORACLE_SCRIPT\" = TRUE");
                bool kq = Class.DB_Config.RunSQL(sql);
                if (kq)
                {
                    SupportFunction.ShowSuccess(lb_error, "Cập nhật thành công");
                    NV_get_DSNhanVien(NV_datagird);
                }
                else
                {
                    SupportFunction.ShowError(lb_error, "Cập nhật thất bại");
                }
            }

            catch { }
        }
        /*=============================================TABITEM: PHÒNG BAN============================================
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
        /*=============================================TABITEM: ĐỀ ÁN ============================================
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
