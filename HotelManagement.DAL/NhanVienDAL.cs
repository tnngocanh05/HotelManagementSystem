using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using HotelManagement.DTO;

namespace HotelManagement.DAL
{
    public class NhanVienDAL
    {
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyKhachSan;Integrated Security=True";

        public List<NhanVienDTO> LayDanhSach()
        {
            List<NhanVienDTO> list = new List<NhanVienDTO>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM NhanVien";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new NhanVienDTO
                    {
                        MaNhanVien = (int)dr["MaNhanVien"],
                        HoTen = dr["HoTen"].ToString(),
                        NgaySinh = Convert.ToDateTime(dr["NgaySinh"]),
                        GioiTinh = dr["GioiTinh"].ToString(),
                        SDT = dr["SDT"].ToString(),
                        CCCD = dr["CCCD"].ToString(),
                        Email = dr["Email"].ToString(),
                        MaChucVu = (int)dr["MaChucVu"]
                    });
                }
            }
            return list;
        }

        public bool Them(NhanVienDTO nv)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO NhanVien (HoTen, NgaySinh, GioiTinh, SDT, CCCD, Email, MaChucVu) " +
                             "VALUES (@ten, @ns, @gt, @sdt, @cccd, @mail, @maCV)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ten", nv.HoTen);
                cmd.Parameters.AddWithValue("@ns", nv.NgaySinh);
                cmd.Parameters.AddWithValue("@gt", nv.GioiTinh);
                cmd.Parameters.AddWithValue("@sdt", nv.SDT);
                cmd.Parameters.AddWithValue("@cccd", nv.CCCD);
                cmd.Parameters.AddWithValue("@mail", nv.Email);
                cmd.Parameters.AddWithValue("@maCV", nv.MaChucVu);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Sua(NhanVienDTO nv)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "UPDATE NhanVien SET HoTen=@ten, NgaySinh=@ns, GioiTinh=@gt, SDT=@sdt, " +
                             "CCCD=@cccd, Email=@mail, MaChucVu=@maCV WHERE MaNhanVien=@ma";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ten", nv.HoTen);
                cmd.Parameters.AddWithValue("@ns", nv.NgaySinh);
                cmd.Parameters.AddWithValue("@gt", nv.GioiTinh);
                cmd.Parameters.AddWithValue("@sdt", nv.SDT);
                cmd.Parameters.AddWithValue("@cccd", nv.CCCD);
                cmd.Parameters.AddWithValue("@mail", nv.Email);
                cmd.Parameters.AddWithValue("@maCV", nv.MaChucVu);
                cmd.Parameters.AddWithValue("@ma", nv.MaNhanVien);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Xoa(int ma)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "DELETE FROM NhanVien WHERE MaNhanVien=@ma";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ma", ma);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}