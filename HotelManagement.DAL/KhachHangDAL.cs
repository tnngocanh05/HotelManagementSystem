using HotelManagement.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace HotelManagement.DAL
{
    public class KhachHangDAL
    {
        private static KhachHangDAL instance;
        public static KhachHangDAL Instance
        {
            get
            {
                if (instance == null)
                    instance = new KhachHangDAL();
                return instance;
            }
        }

        private KhachHangDAL() { }

        private SqlConnection GetConnection()
        {
            DBConnection db = new DBConnection();
            return db.GetConnection();
        }

        #region GET ALL
        public List<KhachHangDTO> GetAll()
        {
            List<KhachHangDTO> list = new List<KhachHangDTO>();

            string query = @"SELECT MaKhachHang, HoTen, CCCD, SDT
                         FROM KhachHang";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();

                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(MapKhachHang(rd));
                    }
                }
            }

            return list;
        }
        #endregion

        #region INSERT
        public bool Insert(KhachHangDTO kh)
        {
            string query = @"INSERT INTO KhachHang
                        (HoTen, CCCD, SDT)
                        VALUES
                        (@HoTen, @CCCD, @SDT)";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();

                AddParams(cmd, kh);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
        #endregion

        #region UPDATE
        public bool Update(KhachHangDTO kh)
        {
            string query = @"UPDATE KhachHang SET
                        HoTen = @HoTen,
                        CCCD = @CCCD,
                        SDT = @SDT
                        WHERE MaKhachHang = @MaKhachHang";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();

                cmd.Parameters.AddWithValue("@MaKhachHang", kh.MaKhachHang);
                AddParams(cmd, kh);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
        #endregion

        #region DELETE
        public bool Delete(int maKH)
        {
            string query = @"DELETE FROM KhachHang WHERE MaKhachHang = @MaKH";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@MaKH", maKH);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
        #endregion

        #region SEARCH
        public List<KhachHangDTO> Search(string keyword)
        {
            List<KhachHangDTO> list = new List<KhachHangDTO>();

            string query = @"SELECT * FROM KhachHang
                         WHERE HoTen LIKE @Keyword
                            OR CCCD LIKE @Keyword
OR SDT LIKE @Keyword";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();

                cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(MapKhachHang(rd));
                    }
                }
            }

            return list;
        }
        #endregion

        #region MAPPING
        private KhachHangDTO MapKhachHang(SqlDataReader rd)
        {
            return new KhachHangDTO
            {
                MaKhachHang = Convert.ToInt32(rd["MaKhachHang"]),
                HoTen = rd["HoTen"].ToString(),
                CCCD = rd["CCCD"].ToString(),
                SDT = rd["SDT"].ToString()
            };
        }
        #endregion

        #region PARAMS COMMON
        private void AddParams(SqlCommand cmd, KhachHangDTO kh)
        {
            cmd.Parameters.AddWithValue("@HoTen", kh.HoTen ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CCCD", kh.CCCD ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@SDT", kh.SDT ?? (object)DBNull.Value);
        }
        #endregion
    }
}