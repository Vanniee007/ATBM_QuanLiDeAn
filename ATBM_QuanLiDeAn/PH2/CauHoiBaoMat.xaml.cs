using System;
using System.Collections.Generic;
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
    /// Interaction logic for CauHoiBaoMat.xaml
    /// </summary>
    public partial class CauHoiBaoMat : Window
    {
        string username;
        int Loai;
        public CauHoiBaoMat(string username_, int Loai_)
        {
            InitializeComponent();
            username= username_;
            Loai = Loai_;
        }

        private void bt_Luu_Click(object sender, RoutedEventArgs e)
        {
            if (tb_traloi.Text.Split().Length==0)
            {
                return;
            }    
            try
            {
                if (Loai == 1)
                {
                    
                    string sql = "BEGIN ATBM_ADMIN.NV_ThemSua_CauHoiBaoMat('" + cbb_CauHoi.SelectedIndex + "','" + tb_traloi.Text + "');END;";
                    Class.DB_Config.RunSqlDel("ALTER SESSION SET \"_ORACLE_SCRIPT\" = TRUE");
                    bool kq = Class.DB_Config.RunSQL(sql);

                    if (kq)
                    {
                        //SupportFunction.ShowSuccess(, "Sửa thông tin thành công");
                        Login_Window login_ = new Login_Window(username);
                        login_.Show();
                        this.Close();
                    }
                    else
                    {
                        //SupportFunction.ShowError(lb_error, "Sửa thông tin thất bại");
                    }
                }   
                else
                {

                }
            }
            catch { }
        }
    }
}
