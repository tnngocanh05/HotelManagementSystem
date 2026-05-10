using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using HotelManagement.DTO;

namespace HotelManagement.DAL
{
    public class DsDatPhongDAL
    {
        // 🔥 Singleton
        private static DsDatPhongDAL instance;
        public static DsDatPhongDAL Instance
        {
            get
            {
                if (instance == null)
                    instance = new DsDatPhongDAL();
                return instance;
            }
        }

        // 🔥 Lấy danh sách
        public List<DsDatPhongDTO> GetAll()
        {
            List<DsDatPhongDTO> list = new List<DsDatPhongDTO>();

            DBConnection db = new DBConnection();
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM DatPhong", conn);
                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    list.Add(new DsDatPhongDTO
                    {
                        MaDatPhong = (int)rd["MaDatPhong"],
                        MaKhachHang = (int)rd["MaKhachHang"],
                        MaPhong = (int)rd["MaPhong"],
                        ThoiGianDat = (DateTime)rd["ThoiGianDat"],
                        ThoiGianNhan = (DateTime)rd["ThoiGianNhan"],
                        NgayTraDuKien = (DateTime)rd["NgayTraDuKien"],
                        SoNguoi = (int)rd["SoNguoi"],
                        TrangThai = rd["TrangThai"].ToString()
                    });
                }
            }
            return list;
        }

        // 🔥 Thêm
        public bool Insert(DsDatPhongDTO dp)
        {
            DBConnection db = new DBConnection();
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string sql = @"INSERT INTO DatPhong 
                (MaKhachHang, MaPhong, ThoiGianNhan, NgayTraDuKien, SoNguoi, TrangThai)
                VALUES (@MaKH, @MaPhong, @Nhan, @Tra, @SoNguoi, @TrangThai)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaKH", dp.MaKhachHang);
                cmd.Parameters.AddWithValue("@MaPhong", dp.MaPhong);
                cmd.Parameters.AddWithValue("@Nhan", dp.ThoiGianNhan);
                cmd.Parameters.AddWithValue("@Tra", dp.NgayTraDuKien);
                cmd.Parameters.AddWithValue("@SoNguoi", dp.SoNguoi);
                cmd.Parameters.AddWithValue("@TrangThai", dp.TrangThai);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // 🔥 Xoá
        public bool Delete(int id)
        {
            DBConnection db = new DBConnection();
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM DatPhong WHERE MaDatPhong=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
