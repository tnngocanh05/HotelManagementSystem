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
                        lp.TenLoaiPhong
                    FROM Phong p
                    INNER JOIN LoaiPhong lp 
                        ON p.MaLoaiPhong = lp.MaLoaiPhong
                    ORDER BY p.Tang, p.SoPhong";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader rd = cmd.ExecuteReader();

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

            return list;
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

                SqlDataReader rd = cmd.ExecuteReader();

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