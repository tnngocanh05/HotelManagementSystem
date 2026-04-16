using HotelManagement.DTO;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace HotelManagement.DAL
{
    public class NhanVienDAL
    {
        private DBConnection db = new DBConnection();

        public List<NhanVienDTO> GetAllNhanVien()
        {
            List<NhanVienDTO> list = new List<NhanVienDTO>();

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = "SELECT MaNhanVien, HoTen FROM NhanVien ORDER BY HoTen";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    list.Add(new NhanVienDTO
                    {
                        MaNhanVien = (int)rd["MaNhanVien"],
                        HoTen = rd["HoTen"].ToString()
                    });
                }
            }

            return list;
        }
    }
}