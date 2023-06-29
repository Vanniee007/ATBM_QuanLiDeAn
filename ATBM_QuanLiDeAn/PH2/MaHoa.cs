using ATBM_QuanLiDeAn.Class;
using System;
using System.Security.Cryptography;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Collections.Generic;

namespace ATBM_QuanLiDeAn.PH2
{

    internal class MaHoa
    {
        private const int KeySize = 2048;//RSA 2048 
        public void GenerateAndSaveKeys(string employeeId)// hàm tạo cặp khóa key gồm container là manv+ngày tạo 
        {
            string publicKey;
            string privateKey;
            string containerName = $"KeyContainer_{DateTime.Now.ToString("yyyyMMddhhmmss")}_{employeeId}";


            CspParameters cspParams = new CspParameters
            {
                KeyContainerName = containerName
            };

            using (var rsa = new RSACryptoServiceProvider(KeySize, cspParams))
            {
                publicKey = Convert.ToBase64String(rsa.ExportCspBlob(false));
                privateKey = Convert.ToBase64String(rsa.ExportCspBlob(true));

                // Lưu khóa vào bảng trong Oracle
                SaveKeysToOracle(employeeId, publicKey, privateKey);
            }
        }
        public List<string> GetEmployeeIdsFromOracle()// hàm này lấy toàn bộ mã nv 
        {
            List<string> employeeIds = new List<string>();

            string query = "select MANV from ATBM_ADMIN.TC_XEMNHANVIEN";
            OracleCommand command = new OracleCommand(query, DB_Config.Conn);

            try
            {
                OracleDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string employeeId = reader.GetString(0);
                    employeeIds.Add(employeeId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred: " + ex.Message);
            }

            return employeeIds;
        }
        public byte[] Bytes(string hexString)
        {
            if (hexString.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                hexString = hexString.Substring(2); // Loại bỏ tiền tố "0x"
            }

            byte[] bytes = new byte[hexString.Length / 2];

            for (int i = 0; i < hexString.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }

            return bytes;
        }


        public string RSAEncrypt(string plainText, string publicKey)//hàm mã hóa 
        {
            byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(plainText);

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(KeySize))
            {
                rsa.ImportCspBlob(Convert.FromBase64String(publicKey));
                byte[] encryptedBytes = rsa.Encrypt(bytesToEncrypt, false);
                string encryptedText = Convert.ToBase64String(encryptedBytes);
                return "0x" + BitConverter.ToString(encryptedBytes).Replace("-", "");
            }
        }

        public string RSADecrypt(byte[] encryptedData, string privateKey)//hàm giải mã 
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(KeySize))
            {
                rsa.ImportCspBlob(Convert.FromBase64String(privateKey));
                byte[] decryptedBytes = rsa.Decrypt(encryptedData, false);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }


        public void SaveKeysToOracle(string employeeId, string publicKey, string privateKey)// hàm lưu trữ khóa vào bảng trong oracle 
        {

            string procedureName = "ATBM_ADMIN.manage_THUONG";
            OracleCommand command = new OracleCommand(procedureName, DB_Config.Conn);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new OracleParameter("p_MANV", OracleDbType.Varchar2)).Value = employeeId;
            command.Parameters.Add(new OracleParameter("p_DIPTHUONG", OracleDbType.Varchar2)).Value = publicKey;
            command.Parameters.Add(new OracleParameter("p_TIENTHUONG", OracleDbType.Varchar2)).Value = privateKey;

            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                
            }


        }
        public void UpdateKeys()
        {
            List<string> employeeIds = GetEmployeeIdsFromOracle();

            foreach (string employeeId in employeeIds)
            {
                // Tạo cặp khóa mới cho giáo viên
                string publicKey = LoadPublicKeyFromOracle(employeeId);
                string privateKey = LoadPrivateKeyFromOracle(employeeId);
                // Lấy dữ liệu đã mã hóa từ Oracle
                string encryptedLuong = GetEncryptedSalaryFromOracle(employeeId);
                string encryptedPhuCap = GetEncryptedAllowanceFromOracle(employeeId);

                // Kiểm tra và giải mã dữ liệu nếu tồn tại
                if (!string.IsNullOrEmpty(encryptedLuong) && !string.IsNullOrEmpty(encryptedPhuCap))
                {
                    byte[] encryptedLuongBytes = Bytes(encryptedLuong);
                    byte[] encryptedPhuCapBytes = Bytes(encryptedPhuCap);
                    string decryptedLuong = RSADecrypt(encryptedLuongBytes, privateKey);
                    string decryptedPhuCap = RSADecrypt(encryptedPhuCapBytes, privateKey);

                    GenerateAndSaveKeys(employeeId);
                    publicKey = LoadPublicKeyFromOracle(employeeId);
                    // Mã hóa lại dữ liệu bằng khóa công khai mới
                    string reencryptedLuong = RSAEncrypt(decryptedLuong, publicKey);
                    string reencryptedPhuCap = RSAEncrypt(decryptedPhuCap, publicKey);

                    // Cập nhật giá trị đã mã hóa mới vào Oracle
                    SaveEncryptedValuesToOracle(employeeId, reencryptedLuong, reencryptedPhuCap);

                }
            }
        }

        public string LoadPublicKeyFromOracle(string employeeId)//hàm lấy ra publickey 
        {
            string publicKey = string.Empty;
            string query = "SELECT DIPTHUONG FROM ATBM_ADMIN.THUONG WHERE MANV = :employeeId";
            OracleCommand command = new OracleCommand(query, DB_Config.Conn);
            command.Parameters.Add(new OracleParameter("employeeId", OracleDbType.Varchar2)).Value = employeeId;

            try
            {
                object result = command.ExecuteScalar();
                publicKey = result != null ? result.ToString() : string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred: " + ex.Message);
            }

            return publicKey;
        }
        public string LoadPrivateKeyFromOracle(string employeeId)//hàm lấy ra privatekey 
        {
            string privateKey = string.Empty;
            string query = "SELECT TIENTHUONG FROM ATBM_ADMIN.THUONG WHERE MANV = :employeeId";
            OracleCommand command = new OracleCommand(query, DB_Config.Conn);
            command.Parameters.Add(new OracleParameter("employeeId", OracleDbType.Varchar2)).Value = employeeId;

            try
            {
                object result = command.ExecuteScalar();
                privateKey = result != null ? result.ToString() : string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred: " + ex.Message);
            }
            return privateKey;
        }
        public void SaveEncryptedValuesToOracle(string employeeId, string encryptedLuong, string encryptedPhuCap)// hàm này cập nhật giá trị mã hóa lương và phụ cấp mới 
        {
            string procedureName = "ATBM_ADMIN.TC_UPD_LUONG_PHUCAP";
            OracleCommand command = new OracleCommand(procedureName, DB_Config.Conn);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new OracleParameter("p_manv", OracleDbType.Varchar2)).Value = employeeId;
            command.Parameters.Add(new OracleParameter("LUONGMOI", OracleDbType.Varchar2)).Value = encryptedLuong;
            command.Parameters.Add(new OracleParameter("PHUCAPMOI", OracleDbType.Varchar2)).Value = encryptedPhuCap;

            try
            {
                command.ExecuteNonQuery();
                Console.WriteLine("Encrypted values saved to Oracle for employee: " + employeeId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred: " + ex.Message);
            }
        }
        //public void DecryptSalaryAndAllowanceForAllEmployees()// hàm này dùng để giải mã toàn bộ lương và phụ cấp 
        //{
        //    KeyGenerator keyGenerator = new KeyGenerator();
        //    List<string> employeeIds = keyGenerator.GetEmployeeIdsFromOracle();

        //    foreach (string employeeId in employeeIds)
        //    {
        //        // Lấy khóa riêng từ Oracle
        //        string privateKey = keyGenerator.LoadPrivateKeyFromOracle(employeeId);

        //        // Lấy dữ liệu đã mã hóa từ Oracle
        //        string encryptedLuong = GetEncryptedSalaryFromOracle(employeeId);
        //        string encryptedPhuCap = GetEncryptedAllowanceFromOracle(employeeId);

        //        // Kiểm tra và giải mã dữ liệu nếu tồn tại
        //        if (!string.IsNullOrEmpty(encryptedLuong) && !string.IsNullOrEmpty(encryptedPhuCap))
        //        {
        //            byte[] encryptedLuongBytes = keyGenerator.Bytes(encryptedLuong);
        //            byte[] encryptedPhuCapBytes = keyGenerator.Bytes(encryptedPhuCap);
        //            string decryptedLuong = keyGenerator.RSADecrypt(encryptedLuongBytes, privateKey);
        //            string decryptedPhuCap = keyGenerator.RSADecrypt(encryptedPhuCapBytes, privateKey);

        //            // In ra kết quả giải mã lương và phụ cấp của mỗi nhân viên
        //            Console.WriteLine("Employee ID: " + employeeId);
        //            Console.WriteLine("Decrypted Salary: " + decryptedLuong);
        //            Console.WriteLine("Decrypted Allowance: " + decryptedPhuCap);
        //            Console.WriteLine();
        //        }
        //    }
        //}

        public string GetEncryptedSalaryFromOracle(string employeeId)// hàm lấy ra giá trị lương mã hóa lưu trong bảng 
        {
            string query = "SELECT LUONG FROM ATBM_ADMIN.TC_XEMNHANVIEN WHERE MANV = :employeeId";
            OracleCommand command = new OracleCommand(query, DB_Config.Conn);
            command.Parameters.Add(new OracleParameter("employeeId", OracleDbType.Varchar2)).Value = employeeId;

            try
            {
                object result = command.ExecuteScalar();
                return result != null ? result.ToString() : string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred: " + ex.Message);
                return string.Empty;
            }
        }

        public string GetEncryptedAllowanceFromOracle(string employeeId)// hàm lấy ra giá trị phụ cấp mã hóa lưu trong bảng 
        {
            string query = "SELECT PHUCAP FROM ATBM_ADMIN.TC_XEMNHANVIEN WHERE MANV = :employeeId";
            OracleCommand command = new OracleCommand(query, DB_Config.Conn);
            command.Parameters.Add(new OracleParameter("employeeId", OracleDbType.Varchar2)).Value = employeeId;

            try
            {
                object result = command.ExecuteScalar();
                return result != null ? result.ToString() : string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred: " + ex.Message);
                return string.Empty;
            }
        }
    }
}
