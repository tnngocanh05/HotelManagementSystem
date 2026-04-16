using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using HotelManagement.DTO;

namespace HotelManagement.DAL
{
    public class DichVuDAL
    {
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyKhachSan;Integrated Security=True";

        public List<DichVuDTO> LayDanhSach()
        {
            List<DichVuDTO> list = new List<DichVuDTO>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT MaDichVu, TenDichVu, DonGia, LoaiDichVu FROM DichVu";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new DichVuDTO
                    {
                        MaDichVu = (int)dr["MaDichVu"],
                        TenDichVu = dr["TenDichVu"].ToString(),
                        DonGia = Convert.ToDecimal(dr["DonGia"]),
                        LoaiDichVu = dr["LoaiDichVu"].ToString()
                    });
                }
            }
            return list;
        }

        public bool Them(DichVuDTO dv)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO DichVu (TenDichVu, DonGia, LoaiDichVu) VALUES (@ten, @gia, @loai)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ten", dv.TenDichVu);
                cmd.Parameters.AddWithValue("@gia", dv.DonGia);
                cmd.Parameters.AddWithValue("@loai", dv.LoaiDichVu);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Sua(DichVuDTO dv)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "UPDATE DichVu SET TenDichVu=@ten, DonGia=@gia, LoaiDichVu=@loai WHERE MaDichVu=@ma";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ten", dv.TenDichVu);
                cmd.Parameters.AddWithValue("@gia", dv.DonGia);
                cmd.Parameters.AddWithValue("@loai", dv.LoaiDichVu);
                cmd.Parameters.AddWithValue("@ma", dv.MaDichVu);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Xoa(int ma)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "DELETE FROM DichVu WHERE MaDichVu=@ma";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ma", ma);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}