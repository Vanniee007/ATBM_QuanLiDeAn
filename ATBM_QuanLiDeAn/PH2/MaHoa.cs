using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ATBM_QuanLiDeAn.PH2
{
    
    internal class MaHoa
    {
        //private const int KeySize = 2048;//RSA 2048 
        //public void GenerateAndSaveKeys(string employeeId)// hàm tạo cặp khóa key gồm container là manv+ngày tạo 
        //{
        //    string publicKey;
        //    string privateKey;
        //    string containerName = $"KeyContainer_{DateTime.Now.ToString("yyyyMMdd")}_{employeeId}";


        //    CspParameters cspParams = new CspParameters
        //    {
        //        KeyContainerName = containerName
        //    };

        //    using (var rsa = new RSACryptoServiceProvider(KeySize, cspParams))
        //    {
        //        publicKey = Convert.ToBase64String(rsa.ExportCspBlob(false));
        //        privateKey = Convert.ToBase64String(rsa.ExportCspBlob(true));

        //        // Lưu khóa vào bảng trong Oracle
        //        SaveKeysToOracle(employeeId, publicKey, privateKey);
        //    }
        //}
        //public List<string> GetEmployeeIdsFromOracle()// hàm này lấy toàn bộ mã nv 
        //{
        //    List<string> employeeIds = new List<string>();

        //    using (OracleConnection connection = new OracleConnection(ConnectionString))
        //    {
        //        connection.Open();

        //        string query = "select MANV from ATBM_ADMIN.TC_XEMNHANVIEN";
        //        OracleCommand command = new OracleCommand(query, connection);

        //        try
        //        {
        //            OracleDataReader reader = command.ExecuteReader();
        //            while (reader.Read())
        //            {
        //                string employeeId = reader.GetString(0);
        //                employeeIds.Add(employeeId);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Error occurred: " + ex.Message);
        //        }
        //    }

        //    return employeeIds;
        //}

        //public string RSAEncrypt(string plainText, string publicKey)//hàm mã hóa 
        //{
        //    byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(plainText);

        //    using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(KeySize))
        //    {
        //        rsa.ImportCspBlob(Convert.FromBase64String(publicKey));
        //        byte[] encryptedBytes = rsa.Encrypt(bytesToEncrypt, false);
        //        string encryptedText = Convert.ToBase64String(encryptedBytes);
        //        return "0x" + BitConverter.ToString(encryptedBytes).Replace("-", "");
        //    }
        //}

        //public string RSADecrypt(byte[] encryptedData, string privateKey)//hàm giải mã 
        //{
        //    using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(KeySize))
        //    {
        //        rsa.ImportCspBlob(Convert.FromBase64String(privateKey));
        //        byte[] decryptedBytes = rsa.Decrypt(encryptedData, false);
        //        return Encoding.UTF8.GetString(decryptedBytes);
        //    }
        //}

        //public byte[] Bytes(string hexString) // hàm phụ trong hàm giải mã 
        //{
        //    if (hexString.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
        //    {
        //        hexString = hexString.Substring(2); // Loại bỏ tiền tố "0x"
        //    }

        //    byte[] bytes = new byte[hexString.Length / 2];

        //    for (int i = 0; i < hexString.Length; i += 2)
        //    {
        //        bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
        //    }

        //    return bytes;
        //}

        //public void SaveKeysToOracle(string employeeId, string publicKey, string privateKey)// hàm lưu trữ khóa vào bảng trong oracle 
        //{
        //    using (OracleConnection connection = new OracleConnection(ConnectionString))
        //    {
        //        connection.Open();

        //        string procedureName = "ATBM_ADMIN.manage_couple_of_keys";
        //        OracleCommand command = new OracleCommand(procedureName, connection);
        //        command.CommandType = CommandType.StoredProcedure;

        //        command.Parameters.Add(new OracleParameter("p_MANV", OracleDbType.Varchar2)).Value = employeeId;
        //        command.Parameters.Add(new OracleParameter("p_public_key", OracleDbType.Varchar2)).Value = publicKey;
        //        command.Parameters.Add(new OracleParameter("p_private_key", OracleDbType.Varchar2)).Value = privateKey;

        //        try
        //        {
        //            command.ExecuteNonQuery();
        //            Console.WriteLine("Keys saved to Oracle.");
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Error occurred: " + ex.Message);
        //        }
        //    }
        //}

        //public string LoadPublicKeyFromOracle(string employeeId)//hàm lấy ra publickey 
        //{
        //    string publicKey = string.Empty;

        //    using (OracleConnection connection = new OracleConnection(ConnectionString))
        //    {
        //        connection.Open();

        //        string query = "SELECT public_key FROM ATBM_ADMIN.COUPLE_OF_KEYS WHERE MANV = :employeeId";
        //        OracleCommand command = new OracleCommand(query, connection);
        //        command.Parameters.Add(new OracleParameter("employeeId", OracleDbType.Varchar2)).Value = employeeId;

        //        try
        //        {
        //            object result = command.ExecuteScalar();
        //            publicKey = result != null ? result.ToString() : string.Empty;
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Error occurred: " + ex.Message);
        //        }
        //    }

        //    return publicKey;
        //}
        //public string LoadPrivateKeyFromOracle(string employeeId)//hàm lấy ra privatekey 
        //{
        //    string privateKey = string.Empty;

        //    using (OracleConnection connection = new OracleConnection(ConnectionString))
        //    {
        //        connection.Open();

        //        string query = "SELECT private_key FROM ATBM_ADMIN.COUPLE_OF_KEYS WHERE MANV = :employeeId";
        //        OracleCommand command = new OracleCommand(query, connection);
        //        command.Parameters.Add(new OracleParameter("employeeId", OracleDbType.Varchar2)).Value = employeeId;

        //        try
        //        {
        //            object result = command.ExecuteScalar();
        //            privateKey = result != null ? result.ToString() : string.Empty;
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Error occurred: " + ex.Message);
        //        }
        //    }

        //    return privateKey;
        //}
        //public void EncryptAndSaveSalaryAndAllowance(KeyGenerator keyGenerator)//hàm này mã hoá toàn bộ thông tin dùng trong lần chạy đầu tiên 
        //{
        //    List<string> employeeIds = keyGenerator.GetEmployeeIdsFromOracle();
        //    using (OracleConnection connection = new OracleConnection(ConnectionString))
        //    {
        //        connection.Open();

        //        string query = "SELECT LUONG, PHUCAP FROM ATBM_ADMIN.TC_XEMNHANVIEN WHERE MANV = :employeeId";

        //        foreach (string employeeId in employeeIds)
        //        {
        //            string publicKey = keyGenerator.LoadPublicKeyFromOracle(employeeId);

        //            using (OracleCommand command = new OracleCommand(query, connection))
        //            {
        //                command.Parameters.Add(new OracleParameter("employeeId", employeeId));

        //                using (OracleDataReader reader = command.ExecuteReader())
        //                {
        //                    if (reader.Read())
        //                    {
        //                        string luong = reader.IsDBNull(0) ? null : reader.GetString(0);
        //                        string phuCap = reader.IsDBNull(1) ? null : reader.GetString(1);

        //                        string encryptedLuong = keyGenerator.RSAEncrypt(luong, publicKey);
        //                        string encryptedPhuCap = keyGenerator.RSAEncrypt(phuCap, publicKey);

        //                        keyGenerator.SaveEncryptedValuesToOracle(employeeId, encryptedLuong, encryptedPhuCap);

        //                        Console.WriteLine("Encrypted values saved for employee: " + employeeId);
        //                    }
        //                }
        //            }
        //        }

        //        connection.Close();
        //    }
        //}
        //public void SaveEncryptedValuesToOracle(string employeeId, string encryptedLuong, string encryptedPhuCap)// hàm này cập nhật giá trị mã hóa lương và phụ cấp mới 
        //{
        //    using (OracleConnection connection = new OracleConnection(ConnectionString))
        //    {
        //        connection.Open();

        //        string procedureName = "ATBM_ADMIN.TC_UPD_LUONG_PHUCAP";
        //        OracleCommand command = new OracleCommand(procedureName, connection);
        //        command.CommandType = CommandType.StoredProcedure;

        //        command.Parameters.Add(new OracleParameter("p_manv", OracleDbType.Varchar2)).Value = employeeId;
        //        command.Parameters.Add(new OracleParameter("LUONGMOI", OracleDbType.Varchar2)).Value = encryptedLuong;
        //        command.Parameters.Add(new OracleParameter("PHUCAPMOI", OracleDbType.Varchar2)).Value = encryptedPhuCap;

        //        try
        //        {
        //            command.ExecuteNonQuery();
        //            Console.WriteLine("Encrypted values saved to Oracle for employee: " + employeeId);
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Error occurred: " + ex.Message);
        //        }
        //    }
        //}
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

        //public string GetEncryptedSalaryFromOracle(string employeeId)// hàm lấy ra giá trị lương mã hóa lưu trong bảng 
        //{
        //    using (OracleConnection connection = new OracleConnection(ConnectionString))
        //    {
        //        connection.Open();

        //        string query = "SELECT LUONG FROM ATBM_ADMIN.TC_XEMNHANVIEN WHERE MANV = :employeeId";
        //        OracleCommand command = new OracleCommand(query, connection);
        //        command.Parameters.Add(new OracleParameter("employeeId", OracleDbType.Varchar2)).Value = employeeId;

        //        try
        //        {
        //            object result = command.ExecuteScalar();
        //            return result != null ? result.ToString() : string.Empty;
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Error occurred: " + ex.Message);
        //            return string.Empty;
        //        }
        //    }
        //}

        //public string GetEncryptedAllowanceFromOracle(string employeeId)// hàm lấy ra giá trị phụ cấp mã hóa lưu trong bảng 
        //{
        //    string query = "SELECT PHUCAP FROM ATBM_ADMIN.TC_XEMNHANVIEN WHERE MANV = :employeeId";
        //    OracleCommand command = new OracleCommand(query, connection);
        //    command.Parameters.Add(new OracleParameter("employeeId", OracleDbType.Varchar2)).Value = employeeId;

        //    try
        //    {
        //        object result = command.ExecuteScalar();
        //        return result != null ? result.ToString() : string.Empty;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error occurred: " + ex.Message);
        //        return string.Empty;
        //    }
        //}
    }
}
