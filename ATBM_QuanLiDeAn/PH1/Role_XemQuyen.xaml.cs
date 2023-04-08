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

namespace ATBM_QuanLiDeAn.PH1
{
    /// <summary>
    /// Interaction logic for Role_XemQuyen.xaml
    /// </summary>
    public partial class Role_XemQuyen : Window
    {
        public string role;
        public Role_XemQuyen(string role_)
        {
            InitializeComponent();
            role = role_;
        }

        private void Xemquyen_tab_loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Xemquyen_sys_loaded(object sender, RoutedEventArgs e)
        {

        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
