using HotelManagement.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace HotelManagement.DAL
{
    public class PhongDAL
    {
        private DBConnection db = new DBConnection();

        // 1. Lấy tất cả phòng
        public List<PhongDTO> GetAllRooms()
        {
            List<PhongDTO> list = new List<PhongDTO>();
            try
            {
                using (SqlConnection conn = db.GetConnection())
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    string query = @"
                        SELECT p.MaPhong, p.SoPhong, p.TinhTrang, p.Tang, p.MaLoaiPhong, lp.TenLoaiPhong
                        FROM Phong p
                        INNER JOIN LoaiPhong lp ON p.MaLoaiPhong = lp.MaLoaiPhong
                        ORDER BY p.Tang, p.SoPhong";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader rd = cmd.ExecuteReader())
                        {
                            while (rd.Read())
                            {
                                list.Add(Mapping(rd));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy danh sách phòng: " + ex.Message);
            }
            return list;
        }

        // 2. Lấy phòng trống theo thời gian (Dùng cho chức năng đặt phòng)
        public List<PhongDTO> GetAvailableRooms(DateTime thoiGianNhanMoi, DateTime thoiGianTraMoi)
        {
            List<PhongDTO> list = new List<PhongDTO>();
            try
            {
                using (SqlConnection conn = db.GetConnection())
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string query = @"
                        SELECT p.MaPhong, p.SoPhong, p.TinhTrang, p.Tang, p.MaLoaiPhong, lp.TenLoaiPhong
                        FROM Phong p
                        INNER JOIN LoaiPhong lp ON p.MaLoaiPhong = lp.MaLoaiPhong
                        WHERE p.MaPhong NOT IN
                        (
                            SELECT dp.MaPhong
                            FROM DatPhong dp
                            WHERE dp.TrangThai IN (N'Đã đặt', N'Đang ở')
                              AND @Nhan < dp.ThoiGianTra
                              AND @Tra > dp.ThoiGianNhan
                        )
                        AND p.TinhTrang != N'Bảo trì'
                        ORDER BY p.Tang, p.SoPhong";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nhan", thoiGianNhanMoi);
                        cmd.Parameters.AddWithValue("@Tra", thoiGianTraMoi);

                        using (SqlDataReader rd = cmd.ExecuteReader())
                        {
                            while (rd.Read())
                            {
                                list.Add(Mapping(rd));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy phòng trống: " + ex.Message);
            }
            return list;
        }

        // 3. Thêm phòng mới
        public bool Them(PhongDTO p)
        {
            try
            {
                using (SqlConnection conn = db.GetConnection())
                {
                    string sql = "INSERT INTO Phong (SoPhong, Tang, MaLoaiPhong, TinhTrang) VALUES (@so, @tang, @maLoai, @tinhTrang)";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@so", p.SoPhong);
                    cmd.Parameters.AddWithValue("@tang", p.Tang);
                    cmd.Parameters.AddWithValue("@maLoai", p.MaLoaiPhong);
                    cmd.Parameters.AddWithValue("@tinhTrang", p.TinhTrang);
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm phòng: " + ex.Message);
                return false;
            }
        }

        // 4. Cập nhật phòng
        public bool Sua(PhongDTO p)
        {
            try
            {
                using (SqlConnection conn = db.GetConnection())
                {
                    string sql = "UPDATE Phong SET SoPhong=@so, Tang=@tang, MaLoaiPhong=@maLoai, TinhTrang=@tinhTrang WHERE MaPhong=@id";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@so", p.SoPhong);
                    cmd.Parameters.AddWithValue("@tang", p.Tang);
                    cmd.Parameters.AddWithValue("@maLoai", p.MaLoaiPhong);
                    cmd.Parameters.AddWithValue("@tinhTrang", p.TinhTrang);
                    cmd.Parameters.AddWithValue("@id", p.MaPhong);
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật phòng: " + ex.Message);
                return false;
            }
        }

        // 5. Xóa phòng
        public bool Xoa(int maPhong)
        {
            try
            {
                using (SqlConnection conn = db.GetConnection())
                {
                    string sql = "DELETE FROM Phong WHERE MaPhong = @ma";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@ma", maPhong);
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể xóa phòng (có thể đang có dữ liệu liên quan): " + ex.Message);
                return false;
            }
        }

        // 6. Lấy giá tiền phòng từ loại phòng
        public decimal GetGiaPhongByMaPhong(int maPhong)
        {
            try
            {
                using (SqlConnection conn = db.GetConnection())
                {
                    string query = @"
                        SELECT lp.GiaTien FROM Phong p
                        INNER JOIN LoaiPhong lp ON p.MaLoaiPhong = lp.MaLoaiPhong
                        WHERE p.MaPhong = @MaPhong";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaPhong", maPhong);
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToDecimal(result) : 0;
                }
            }
            catch { return 0; }
        }

        // Hàm hỗ trợ Mapping dữ liệu để tránh lặp code
        private PhongDTO Mapping(SqlDataReader rd)
        {
            return new PhongDTO
            {
                MaPhong = rd["MaPhong"] != DBNull.Value ? Convert.ToInt32(rd["MaPhong"]) : 0,
                SoPhong = rd["SoPhong"].ToString(),
                TinhTrang = rd["TinhTrang"].ToString(),
                Tang = rd["Tang"] != DBNull.Value ? Convert.ToInt32(rd["Tang"]) : 0,
                MaLoaiPhong = rd["MaLoaiPhong"] != DBNull.Value ? Convert.ToInt32(rd["MaLoaiPhong"]) : 0,
                TenLoaiPhong = rd["TenLoaiPhong"].ToString()
            };
        }
    }
}