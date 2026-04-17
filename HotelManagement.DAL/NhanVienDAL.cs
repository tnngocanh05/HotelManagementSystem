using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using HotelManagement.DTO;

namespace HotelManagement.DAL
{
    public class NhanVienDAL
    {
        // 🔥 Singleton
        private static NhanVienDAL instance;
        public static NhanVienDAL Instance
        {
            get
            {
                if (instance == null)
                    instance = new NhanVienDAL();
                return instance;
            }
        }

        private NhanVienDAL() { }

        // 🔥 Sửa lại dùng DBConnection
        private SqlConnection GetConnection()
        {
            DBConnection db = new DBConnection();
            return db.GetConnection();
        }

        #region GET ALL
        public List<NhanVienDTO> GetAll()
        {
            List<NhanVienDTO> list = new List<NhanVienDTO>();

            string query = @"SELECT MaNhanVien, HoTen, NgaySinh, GioiTinh,
                                    SDT, CCCD, Email, MaChucVu
                             FROM NhanVien";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();

                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(MapNhanVien(rd));
                    }
                }
            }

            return list;
        }
        #endregion

        #region INSERT
        public bool Insert(NhanVienDTO nv)
        {
            string query = @"INSERT INTO NhanVien
                            (HoTen, NgaySinh, GioiTinh, SDT, CCCD, Email, MaChucVu)
                            VALUES
                            (@HoTen, @NgaySinh, @GioiTinh, @SDT, @CCCD, @Email, @MaChucVu)";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();

                AddParams(cmd, nv);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
        #endregion

        #region UPDATE
        public bool Update(NhanVienDTO nv)
        {
            string query = @"UPDATE NhanVien SET
                            HoTen = @HoTen,
                            NgaySinh = @NgaySinh,
                            GioiTinh = @GioiTinh,
                            SDT = @SDT,
                            CCCD = @CCCD,
                            Email = @Email,
                            MaChucVu = @MaChucVu
                            WHERE MaNhanVien = @MaNhanVien";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();

                cmd.Parameters.AddWithValue("@MaNhanVien", nv.MaNV);
                AddParams(cmd, nv);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
        #endregion

        #region DELETE
        public bool Delete(int maNV)
        {
            string query = @"DELETE FROM NhanVien WHERE MaNhanVien = @MaNV";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@MaNV", maNV);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
        #endregion

        #region SEARCH
        public List<NhanVienDTO> Search(string keyword)
        {
            List<NhanVienDTO> list = new List<NhanVienDTO>();

            string query = @"SELECT * FROM NhanVien
                             WHERE HoTen LIKE @Keyword
                                OR SDT LIKE @Keyword
                                OR Email LIKE @Keyword";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();

                cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(MapNhanVien(rd));
                    }
                }
            }

            return list;
        }
        #endregion

        #region MAPPING
        private NhanVienDTO MapNhanVien(SqlDataReader rd)
        {
            return new NhanVienDTO
            {
                MaNV = Convert.ToInt32(rd["MaNhanVien"]),
                HoTen = rd["HoTen"].ToString(),
                NgaySinh = rd["NgaySinh"] == DBNull.Value ? null : (DateTime?)rd["NgaySinh"],
                GioiTinh = rd["GioiTinh"].ToString(),
                SDT = rd["SDT"].ToString(),
                CCCD = rd["CCCD"].ToString(),
                Email = rd["Email"].ToString(),
                MaChucVu = rd["MaChucVu"] == DBNull.Value ? null : (int?)rd["MaChucVu"]
            };
        }
        #endregion

        #region PARAMS
        private void AddParams(SqlCommand cmd, NhanVienDTO nv)
        {
            cmd.Parameters.AddWithValue("@HoTen", nv.HoTen ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@NgaySinh", (object)nv.NgaySinh ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@GioiTinh", nv.GioiTinh ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@SDT", nv.SDT ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CCCD", nv.CCCD ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", nv.Email ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@MaChucVu", (object)nv.MaChucVu ?? DBNull.Value);
        }
        #endregion
    }
}