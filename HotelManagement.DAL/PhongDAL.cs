using HotelManagement.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace HotelManagement.DAL
{
    public class PhongDAL
    {
        private DBConnection db = new DBConnection();

        public List<PhongDTO> GetAllRooms()
        {
            List<PhongDTO> list = new List<PhongDTO>();

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT 
                        p.MaPhong,
                        p.SoPhong,
                        p.TinhTrang,
                        p.Tang,
                        p.MaLoaiPhong,
                        lp.TenLoaiPhong,
                        lp.SoNguoiToiDa,
                        lp.GiaTien
                    FROM Phong p
                    INNER JOIN LoaiPhong lp 
                        ON p.MaLoaiPhong = lp.MaLoaiPhong
                    ORDER BY p.SoPhong";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        PhongDTO phong = new PhongDTO
                        {
                            MaPhong = (int)rd["MaPhong"],
                            SoPhong = rd["SoPhong"].ToString(),
                            TinhTrang = rd["TinhTrang"].ToString(),
                            Tang = (int)rd["Tang"],
                            MaLoaiPhong = (int)rd["MaLoaiPhong"],
                            TenLoaiPhong = rd["TenLoaiPhong"].ToString(),
                            SoNguoiToiDa = Convert.ToInt32(rd["SoNguoiToiDa"]),
                            GiaTien = Convert.ToDecimal(rd["GiaTien"])
                        };

                        list.Add(phong);
                    }
                }
            }

            return list;
        }

        public List<LoaiPhongDTO> GetAllRoomTypes()
        {
            List<LoaiPhongDTO> list = new List<LoaiPhongDTO>();

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                const string query = @"
                    SELECT MaLoaiPhong, TenLoaiPhong, SoNguoiToiDa, GiaTien
                    FROM LoaiPhong
                    ORDER BY TenLoaiPhong";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new LoaiPhongDTO
                        {
                            MaLoaiPhong = (int)rd["MaLoaiPhong"],
                            TenLoaiPhong = rd["TenLoaiPhong"].ToString(),
                            SoNguoiToiDa = Convert.ToInt32(rd["SoNguoiToiDa"]),
                            GiaTien = Convert.ToDecimal(rd["GiaTien"])
                        });
                    }
                }
            }

            return list;
        }

        public bool AddRoom(PhongDTO phong)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                const string query = @"
                    INSERT INTO Phong (SoPhong, TinhTrang, Tang, MaLoaiPhong)
                    VALUES (@SoPhong, @TinhTrang, @Tang, @MaLoaiPhong)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SoPhong", phong.SoPhong);
                    cmd.Parameters.AddWithValue("@TinhTrang", phong.TinhTrang);
                    cmd.Parameters.AddWithValue("@Tang", phong.Tang);
                    cmd.Parameters.AddWithValue("@MaLoaiPhong", phong.MaLoaiPhong);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateRoom(PhongDTO phong)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                const string query = @"
                    UPDATE Phong
                    SET SoPhong = @SoPhong,
                        TinhTrang = @TinhTrang,
                        Tang = @Tang,
                        MaLoaiPhong = @MaLoaiPhong
                    WHERE MaPhong = @MaPhong";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaPhong", phong.MaPhong);
                    cmd.Parameters.AddWithValue("@SoPhong", phong.SoPhong);
                    cmd.Parameters.AddWithValue("@TinhTrang", phong.TinhTrang);
                    cmd.Parameters.AddWithValue("@Tang", phong.Tang);
                    cmd.Parameters.AddWithValue("@MaLoaiPhong", phong.MaLoaiPhong);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteRoom(int maPhong)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                const string query = "DELETE FROM Phong WHERE MaPhong = @MaPhong";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaPhong", maPhong);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<PhongDTO> GetAvailableRooms(DateTime thoiGianNhanMoi, DateTime thoiGianTraMoi)
        {
            List<PhongDTO> list = new List<PhongDTO>();

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT 
                        p.MaPhong,
                        p.SoPhong,
                        p.TinhTrang,
                        p.Tang,
                        p.MaLoaiPhong,
                        lp.TenLoaiPhong
                    FROM Phong p
                    INNER JOIN LoaiPhong lp ON p.MaLoaiPhong = lp.MaLoaiPhong
                    WHERE p.MaPhong NOT IN
                    (
                        SELECT dp.MaPhong
                        FROM DatPhong dp
                        WHERE dp.TrangThai IN (N'Đã đặt', N'Đang ở')
                          AND @ThoiGianNhanMoi < dp.ThoiGianTra
                          AND @ThoiGianTraMoi > dp.ThoiGianNhan
                    )
                    ORDER BY p.Tang, p.SoPhong";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ThoiGianNhanMoi", thoiGianNhanMoi);
                cmd.Parameters.AddWithValue("@ThoiGianTraMoi", thoiGianTraMoi);

                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        PhongDTO phong = new PhongDTO
                        {
                            MaPhong = (int)rd["MaPhong"],
                            SoPhong = rd["SoPhong"].ToString(),
                            TinhTrang = rd["TinhTrang"].ToString(),
                            Tang = (int)rd["Tang"],
                            MaLoaiPhong = (int)rd["MaLoaiPhong"],
                            TenLoaiPhong = rd["TenLoaiPhong"].ToString()
                        };

                        list.Add(phong);
                    }
                }
            }

            return list;
        }

        public decimal GetGiaPhongByMaPhong(int maPhong)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = @"
            SELECT lp.GiaTien
            FROM Phong p
            INNER JOIN LoaiPhong lp ON p.MaLoaiPhong = lp.MaLoaiPhong
            WHERE p.MaPhong = @MaPhong";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaPhong", maPhong);

                object result = cmd.ExecuteScalar();
                if (result != null)
                    return Convert.ToDecimal(result);

                return 0;
            }
        }
    }
}
