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

namespace ATBM_QuanLiDeAn.PH1
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Admin_main : Window
    {
        public Admin_main()
        {
            InitializeComponent();
        }


        private void HienThi(string ob)
        {
            DataTable tbl = new DataTable();
            string sql = "SELECT USER_ID, OBJECT_TYPE, CREATED, STATUS" +
                " FROM USER_OBJECTS WHERE OBJECT_TYPE != 'SEQUENCE'";
            if (ob != "Tất cả")
            {
                sql = sql + " AND OBJECT_TYPE = '" + ob.ToUpper() + "'";
            }
            tbl = Class.DB_Config.GetDataToTable(sql);
            ND_Datagrid.ItemsSource = null;
            ND_Datagrid.ItemsSource = tbl.DefaultView;
        }
        private void ND_Datagrid_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
