using HotelManagement.DTO;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace HotelManagement.DAL
{
    public class DichVuDAL
    {
        private DBConnection db = new DBConnection();

        public List<DichVuDTO> GetAllDichVu()
        {
            List<DichVuDTO> list = new List<DichVuDTO>();

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT MaDichVu, TenDichVu, DonGia, LoaiDichVu
                    FROM DichVu
                    ORDER BY TenDichVu";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    list.Add(new DichVuDTO
                    {
                        MaDichVu = (int)rd["MaDichVu"],
                        TenDichVu = rd["TenDichVu"].ToString(),
                        DonGia = (decimal)rd["DonGia"],
                        LoaiDichVu = rd["LoaiDichVu"].ToString()
                    });
                }
            }

            return list;
        }
    }
}