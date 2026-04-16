using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using HotelManagement.DTO;

namespace HotelManagement.DAL
{
    public class LoaiPhongDAL
    {
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyKhachSan;Integrated Security=True";

        public List<LoaiPhongDTO> LayDanhSach()
        {
            List<LoaiPhongDTO> list = new List<LoaiPhongDTO>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM LoaiPhong";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new LoaiPhongDTO
                    {
                        MaLoaiPhong = (int)dr["MaLoaiPhong"],
                        TenLoaiPhong = dr["TenLoaiPhong"].ToString(),
                        SoNguoiToiDa = (int)dr["SoNguoiToiDa"],
                        GiaTien = (decimal)dr["GiaTien"]
                    });
                }
            }
            return list;
        }

        public bool Them(LoaiPhongDTO lp)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO LoaiPhong(TenLoaiPhong, SoNguoiToiDa, GiaTien) VALUES(@ten, @so, @gia)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ten", lp.TenLoaiPhong);
                cmd.Parameters.AddWithValue("@so", lp.SoNguoiToiDa);
                cmd.Parameters.AddWithValue("@gia", lp.GiaTien);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public bool SuaLoaiPhong(LoaiPhongDTO lp)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "UPDATE LoaiPhong SET TenLoaiPhong = @ten, SoNguoiToiDa = @sl, GiaTien = @gia WHERE MaLoaiPhong = @ma";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ten", lp.TenLoaiPhong);
                cmd.Parameters.AddWithValue("@sl", lp.SoNguoiToiDa);
                cmd.Parameters.AddWithValue("@gia", lp.GiaTien);
                cmd.Parameters.AddWithValue("@ma", lp.MaLoaiPhong);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool XoaLoaiPhong(int ma)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "DELETE FROM LoaiPhong WHERE MaLoaiPhong = @ma";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ma", ma);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}