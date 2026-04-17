using HotelManagement.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace HotelManagement.DAL
{
    public class DichVuDAL
    {
        private static DichVuDAL instance;
        public static DichVuDAL Instance
        {
            get
            {
                if (instance == null)
                    instance = new DichVuDAL();
                return instance;
            }
        }

        private SqlConnection GetConnection()
        {
            DBConnection db = new DBConnection();
            return db.GetConnection();
        }

        #region GET ALL
        public List<DichVuDTO> GetAll()
        {
            List<DichVuDTO> list = new List<DichVuDTO>();

            string query = @"SELECT MaDichVu, TenDichVu, DonGia, LoaiDichVu FROM DichVu";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();

                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(MapDichVu(rd));
                    }
                }
            }

            return list;
        }
        #endregion

        #region INSERT
        public bool Insert(DichVuDTO dv)
        {
            string query = @"INSERT INTO DichVu(TenDichVu, DonGia, LoaiDichVu)
                         VALUES(@Ten, @Gia, @Loai)";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                AddParams(cmd, dv);
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        #endregion

        #region UPDATE
        public bool Update(DichVuDTO dv)
        {
            string query = @"UPDATE DichVu
                         SET TenDichVu=@Ten, DonGia=@Gia, LoaiDichVu=@Loai
                         WHERE MaDichVu=@Ma";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();

                cmd.Parameters.AddWithValue("@Ma", dv.MaDichVu);
                AddParams(cmd, dv);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
        #endregion

        #region DELETE
        public bool Delete(int ma)
        {
            string query = "DELETE FROM DichVu WHERE MaDichVu=@Ma";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@Ma", ma);
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        #endregion

        #region MAP
        private DichVuDTO MapDichVu(SqlDataReader rd)
        {
            return new DichVuDTO
            {
                MaDichVu = Convert.ToInt32(rd["MaDichVu"]),
                TenDichVu = rd["TenDichVu"].ToString(),
                DonGia = Convert.ToDecimal(rd["DonGia"]),
                LoaiDichVu = rd["LoaiDichVu"].ToString()
            };
        }
        #endregion

        #region PARAM
        private void AddParams(SqlCommand cmd, DichVuDTO dv)
        {
            cmd.Parameters.AddWithValue("@Ten", dv.TenDichVu);
            cmd.Parameters.AddWithValue("@Gia", dv.DonGia); cmd.Parameters.AddWithValue("@Loai", dv.LoaiDichVu);
        }
        #endregion

        #region SEARCH
        public List<DichVuDTO> Search(string keyword)
        {
            List<DichVuDTO> list = new List<DichVuDTO>();

            string query = @"SELECT * FROM DichVu
                     WHERE TenDichVu LIKE @kw
                        OR LoaiDichVu LIKE @kw";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();

                cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");

                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(MapDichVu(rd));
                    }
                }
            }

            return list;
        }
        #endregion
    }

}