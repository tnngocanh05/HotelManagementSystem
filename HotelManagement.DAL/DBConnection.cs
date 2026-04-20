using System.Configuration;
using System.Data.SqlClient;

namespace HotelManagement.DAL
{
    public class DBConnection
    {
        private readonly string connectionString =
            ConfigurationManager.ConnectionStrings["QuanLyKhachSanConnection"]?.ConnectionString
            ?? @"Data Source=NHUNGOC\SQLEXPRESS;Initial Catalog=QuanLyKhachSan;Integrated Security=True;TrustServerCertificate=True";

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
