using HotelManagement.DTO;
using System;
using System.Data.SqlClient;

namespace HotelManagement.DAL
{
    public class KhachHangDAL
    {
        private DBConnection db = new DBConnection();

        // 🔎 Lấy khách hàng theo CCCD
        public KhachHangDTO GetByCCCD(string cccd)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = "SELECT * FROM KhachHang WHERE CCCD = @CCCD";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CCCD", cccd);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new KhachHangDTO
                    {
                        MaKhachHang = Convert.ToInt32(reader["MaKhachHang"]),
                        HoTen = reader["HoTen"].ToString(),
                        CCCD = reader["CCCD"].ToString(),
                        SDT = reader["SDT"].ToString()
                    };
                }

                return null;
            }
        }

        // ➕ Thêm khách hàng mới và trả về ID
        public int InsertAndReturnId(KhachHangDTO kh)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = @"
                    INSERT INTO KhachHang(HoTen, CCCD, SDT)
                    VALUES (@HoTen, @CCCD, @SDT);
                    SELECT SCOPE_IDENTITY();
                ";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@HoTen", kh.HoTen);
                cmd.Parameters.AddWithValue("@CCCD", kh.CCCD);
                cmd.Parameters.AddWithValue("@SDT", kh.SDT);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}