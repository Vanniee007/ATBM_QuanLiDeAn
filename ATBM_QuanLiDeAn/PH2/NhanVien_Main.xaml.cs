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
    /// Interaction logic for NhanVien_Main.xaml
    /// </summary>
    public partial class NhanVien_Main : Window
    {
        string username;
        public NhanVien_Main(string username_)
        {
            InitializeComponent();
            username= username_;
        }
    }
}
