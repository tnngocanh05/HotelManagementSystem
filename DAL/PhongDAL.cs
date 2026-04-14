using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using QuanLyKhachSanWeb.DTO;

namespace QuanLyKhachSanWeb.DAL
{
    public class PhongDAL : DBConnect
    {
        public List<PhongDTO> LayDanhSachPhong()
        {
            var danhSach = new List<PhongDTO>();
            const string query = @"
SELECT p.MaPhong,
       p.SoPhong,
       p.MaLoaiPhong,
       lp.TenLoaiPhong,
       lp.SoNguoiToiDa,
       lp.GiaTien,
       p.TinhTrang,
       p.Tang
FROM Phong p
JOIN LoaiPhong lp
    ON p.MaLoaiPhong = lp.MaLoaiPhong
ORDER BY p.SoPhong";

            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        danhSach.Add(new PhongDTO
                        {
                            MaPhong = Convert.ToInt32(reader["MaPhong"]),
                            SoPhong = reader["SoPhong"].ToString(),
                            TenLoaiPhong = reader["TenLoaiPhong"].ToString(),
                            SoNguoiToiDa = Convert.ToInt32(reader["SoNguoiToiDa"]),
                            GiaTien = Convert.ToDecimal(reader["GiaTien"]),
                            TinhTrang = reader["TinhTrang"].ToString(),
                            Tang = Convert.ToInt32(reader["Tang"]),
                            MaLoaiPhong = Convert.ToInt32(reader["MaLoaiPhong"])
                        });
                    }
                }
            }

            return danhSach;
        }

        public List<LoaiPhongDTO> LayDanhSachLoaiPhong()
        {
            var danhSach = new List<LoaiPhongDTO>();
            const string query = @"
SELECT MaLoaiPhong, TenLoaiPhong, SoNguoiToiDa, GiaTien
FROM LoaiPhong
ORDER BY TenLoaiPhong";

            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        danhSach.Add(new LoaiPhongDTO
                        {
                            MaLoaiPhong = Convert.ToInt32(reader["MaLoaiPhong"]),
                            TenLoaiPhong = reader["TenLoaiPhong"].ToString(),
                            SoNguoiToiDa = Convert.ToInt32(reader["SoNguoiToiDa"]),
                            GiaTien = Convert.ToDecimal(reader["GiaTien"])
                        });
                    }
                }
            }

            return danhSach;
        }

        public bool ThemPhong(PhongDTO phong)
        {
            const string query = @"
INSERT INTO Phong (SoPhong, TinhTrang, Tang, MaLoaiPhong)
VALUES (@SoPhong, @TinhTrang, @Tang, @MaLoaiPhong)";

            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@SoPhong", phong.SoPhong);
                command.Parameters.AddWithValue("@TinhTrang", phong.TinhTrang);
                command.Parameters.AddWithValue("@Tang", phong.Tang);
                command.Parameters.AddWithValue("@MaLoaiPhong", phong.MaLoaiPhong);

                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool SuaPhong(PhongDTO phong)
        {
            const string query = @"
UPDATE Phong
SET SoPhong = @SoPhong,
    TinhTrang = @TinhTrang,
    Tang = @Tang,
    MaLoaiPhong = @MaLoaiPhong
WHERE MaPhong = @MaPhong";

            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@MaPhong", phong.MaPhong);
                command.Parameters.AddWithValue("@SoPhong", phong.SoPhong);
                command.Parameters.AddWithValue("@TinhTrang", phong.TinhTrang);
                command.Parameters.AddWithValue("@Tang", phong.Tang);
                command.Parameters.AddWithValue("@MaLoaiPhong", phong.MaLoaiPhong);

                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool XoaPhong(int maPhong)
        {
            const string query = "DELETE FROM Phong WHERE MaPhong = @MaPhong";

            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@MaPhong", maPhong);
                return command.ExecuteNonQuery() > 0;
            }
        }
    }
}
