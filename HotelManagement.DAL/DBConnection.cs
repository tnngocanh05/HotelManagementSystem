using System.Data.SqlClient;

namespace HotelManagement.DAL
{
    public class DBConnection
    {
        private readonly string connectionString =
            @"Data Source=DESKTOP-8AOU869\SQLEXPRESS;Initial Catalog=QuanLyKhachSan;Integrated Security=True;TrustServerCertificate=True";

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}