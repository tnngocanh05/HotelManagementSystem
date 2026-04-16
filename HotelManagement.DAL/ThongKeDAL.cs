using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace HotelManagement.DAL
{
    public class ThongKeDAL
    {
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyKhachSan;Integrated Security=True";

        // 1. Thống kê Card: Tổng doanh thu
        public decimal LayTongDoanhThu()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT SUM(TongTien) FROM HoaDon";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                var res = cmd.ExecuteScalar();
                return res != DBNull.Value ? Convert.ToDecimal(res) : 0;
            }
        }

        // 2. Biểu đồ CỘT: Doanh thu theo khoảng thời gian
        public Dictionary<string, decimal> LayDoanhThuTheoKhoang(DateTime tuNgay, DateTime denNgay)
        {
            Dictionary<string, decimal> result = new Dictionary<string, decimal>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"SELECT CAST(NgayThanhToan AS DATE) as Ngay, SUM(TongTien) as DoanhThu 
                               FROM HoaDon 
                               WHERE CAST(NgayThanhToan AS DATE) BETWEEN @tuNgay AND @denNgay
                               GROUP BY CAST(NgayThanhToan AS DATE) 
                               ORDER BY Ngay ASC";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@tuNgay", tuNgay);
                cmd.Parameters.AddWithValue("@denNgay", denNgay);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    result.Add(Convert.ToDateTime(dr["Ngay"]).ToString("dd/MM"), Convert.ToDecimal(dr["DoanhThu"]));
                }
            }
            return result;
        }

        // 3. Biểu đồ TRÒN: Tình trạng phòng
        public Dictionary<string, int> LayTinhTrangPhong()
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT TinhTrang, COUNT(*) as SoLuong FROM Phong GROUP BY TinhTrang";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    result.Add(dr["TinhTrang"].ToString(), Convert.ToInt32(dr["SoLuong"]));
                }
            }
            return result;
        }

        // 4. Biểu đồ TRÒN: Dịch vụ sử dụng
        public Dictionary<string, int> LayThongKeDichVu()
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"SELECT dv.TenDichVu, SUM(ct.SoLuong) as TongSL 
                               FROM ChiTietDichVu ct 
                               JOIN DichVu dv ON ct.MaDichVu = dv.MaDichVu 
                               GROUP BY dv.TenDichVu";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    result.Add(dr["TenDichVu"].ToString(), Convert.ToInt32(dr["TongSL"]));
                }
            }
            return result;
        }
    }
}