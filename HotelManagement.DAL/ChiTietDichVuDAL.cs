using HotelManagement.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace HotelManagement.DAL
{
    public class ChiTietDichVuDAL
    {
        private static ChiTietDichVuDAL instance;
        public static ChiTietDichVuDAL Instance
        {
            get
            {
                if (instance == null)
                    instance = new ChiTietDichVuDAL();
                return instance;
            }
        }

        private SqlConnection GetConnection()
        {
            DBConnection db = new DBConnection();
            return db.GetConnection();
        }

        #region GET ALL
        public List<ChiTietDichVuDTO> GetAll()
        {
            List<ChiTietDichVuDTO> list = new List<ChiTietDichVuDTO>();

            string query = "SELECT * FROM ChiTietDichVu";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(Map(rd));
                    }
                }
            }

            return list;
        }
        #endregion

        #region INSERT
        public bool Insert(ChiTietDichVuDTO ct)
        {
            string query = @"INSERT INTO ChiTietDichVu
                        (MaDatPhong, MaDichVu, SoLuong, ThanhTien)
                        VALUES (@MaDP, @MaDV, @SL, @Tien)";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();

                cmd.Parameters.AddWithValue("@MaDP", ct.MaDatPhong);
                cmd.Parameters.AddWithValue("@MaDV", ct.MaDichVu);
                cmd.Parameters.AddWithValue("@SL", ct.SoLuong);
                cmd.Parameters.AddWithValue("@Tien", ct.ThanhTien);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
        #endregion

        #region UPDATE
        public bool Update(ChiTietDichVuDTO ct)
        {
            string query = @"UPDATE ChiTietDichVu
                         SET MaDatPhong=@MaDP,
                             MaDichVu=@MaDV,
                             SoLuong=@SL,
                             ThanhTien=@Tien
                         WHERE MaCTDV=@Ma";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();

                cmd.Parameters.AddWithValue("@Ma", ct.MaCTDV);
                cmd.Parameters.AddWithValue("@MaDP", ct.MaDatPhong);
                cmd.Parameters.AddWithValue("@MaDV", ct.MaDichVu);
                cmd.Parameters.AddWithValue("@SL", ct.SoLuong);
                cmd.Parameters.AddWithValue("@Tien", ct.ThanhTien);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
        #endregion

        #region DELETE
        public bool Delete(int ma)
        {
            string query = "DELETE FROM ChiTietDichVu WHERE MaCTDV=@Ma";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@Ma", ma);
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        #endregion

        #region GET BY MÃ ĐẶT PHÒNG
        public List<ChiTietDichVuDTO> GetByMaDatPhong(int maDP)
        {
            List<ChiTietDichVuDTO> list = new List<ChiTietDichVuDTO>();

            string query = "SELECT * FROM ChiTietDichVu WHERE MaDatPhong=@MaDP";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@MaDP", maDP);

                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(Map(rd));
                    }
                }
            }

            return list;
        }
        #endregion

        #region MAP
        private ChiTietDichVuDTO Map(SqlDataReader rd)
        {
            return new ChiTietDichVuDTO
            {
                MaCTDV = Convert.ToInt32(rd["MaCTDV"]),
                
                MaDatPhong = Convert.ToInt32(rd["MaDatPhong"]),
                MaDichVu = Convert.ToInt32(rd["MaDichVu"]),
                SoLuong = Convert.ToInt32(rd["SoLuong"]),
                ThanhTien = Convert.ToDecimal(rd["ThanhTien"])
            };
        }
        #endregion
    }
}