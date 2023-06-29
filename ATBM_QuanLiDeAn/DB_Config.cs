using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Runtime.InteropServices.WindowsRuntime;

namespace ATBM_QuanLiDeAn.Class
{
    class DB_Config
    {
        public static OracleConnection Conn;  //Khai báo đối tượng kết nối
                                   

        public static bool Connect(string username, string password)
        {
            string host = "localhost";
            string port = "1521";
            string sid = "xe";

            string strConn = "Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = "
            + host + ")(PORT = " + port + "))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = "
            + sid + ")));Password=" + password + ";User ID=" + username;


            Conn = new OracleConnection(strConn);

            try
            {
                if (Conn == null)
                {
                    Conn = new OracleConnection(strConn);
                    return true;
                }
                if (Conn.State == ConnectionState.Closed)
                {
                    Conn.Open();    
                    return true;
                }
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        public static void Disconnect()
        {
            if (Conn.State == ConnectionState.Open)
            {
                Conn.Close();   	//Đóng kết nối
                Conn.Dispose(); 	//Giải phóng tài nguyên
                Conn = null;
            }
        }

        //Lấy dữ liệu vào bảng
        public static DataTable GetDataToTable(string sql)
        {
            sql = "((" + sql + "))";
            OracleDataAdapter DataAdapter = new OracleDataAdapter(); //Định nghĩa đối tượng thuộc lớp SqlDataAdapter
            //Tạo đối tượng thuộc lớp OracleCommand
            DataAdapter.SelectCommand = new OracleCommand();
            DataAdapter.SelectCommand.Connection = DB_Config.Conn; //Kết nối cơ sở dữ liệu
            DataAdapter.SelectCommand.CommandText = sql; //Lệnh SQL
            //Khai báo đối tượng table thuộc lớp DataTable
            DataTable table = new DataTable();
            DataAdapter.Fill(table);
            return table;
        }

        public static bool CheckValue(string sql)
        {
            sql = "((" + sql + "))";
            OracleDataAdapter dap = new OracleDataAdapter(sql, Conn);
            DataTable table = new DataTable();
            dap.Fill(table);
            if (table.Rows.Count > 0)
                return true;
            else return false;
        }

        public static bool RunSQL(string sql)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = Conn;
            cmd.CommandText = sql;
            int kq = 0;
            try
            {
                kq = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
            cmd.Dispose();
            cmd = null;
            if (kq < 0)
                return true;
            return false;
        }

        public static void RunSqlDel(string sql)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DB_Config.Conn;
            cmd.CommandText = sql;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
            cmd.Dispose();
            cmd = null;
        }



    }

}
