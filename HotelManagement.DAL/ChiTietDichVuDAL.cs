using HotelManagement.DTO;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace HotelManagement.DAL
{
    public class ChiTietDichVuDAL
    {
        private DBConnection db = new DBConnection();

        public List<ChiTietDichVuDTO> GetByMaDatPhong(int maDatPhong)
        {
            List<ChiTietDichVuDTO> list = new List<ChiTietDichVuDTO>();

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT 
                        ctdv.MaCTDV,
                        ctdv.MaDatPhong,
                        ctdv.MaDichVu,
                        ctdv.SoLuong,
                        ctdv.ThanhTien,
                        dv.TenDichVu,
                        dv.LoaiDichVu,
                        dv.DonGia
                    FROM ChiTietDichVu ctdv
                    INNER JOIN DichVu dv ON ctdv.MaDichVu = dv.MaDichVu
                    WHERE ctdv.MaDatPhong = @MaDatPhong
                    ORDER BY ctdv.MaCTDV DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaDatPhong", maDatPhong);

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    list.Add(new ChiTietDichVuDTO
                    {
                        MaCTDV = (int)rd["MaCTDV"],
                        MaDatPhong = (int)rd["MaDatPhong"],
                        MaDichVu = (int)rd["MaDichVu"],
                        SoLuong = (int)rd["SoLuong"],
                        ThanhTien = (decimal)rd["ThanhTien"],
                        TenDichVu = rd["TenDichVu"].ToString(),
                        LoaiDichVu = rd["LoaiDichVu"].ToString(),
                        DonGia = (decimal)rd["DonGia"]
                    });
                }
            }

            return list;
        }

        public bool InsertChiTietDichVu(ChiTietDichVuDTO dto)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = @"
                    INSERT INTO ChiTietDichVu(MaDatPhong, MaDichVu, SoLuong, ThanhTien)
                    VALUES(@MaDatPhong, @MaDichVu, @SoLuong, @ThanhTien)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaDatPhong", dto.MaDatPhong);
                cmd.Parameters.AddWithValue("@MaDichVu", dto.MaDichVu);
                cmd.Parameters.AddWithValue("@SoLuong", dto.SoLuong);
                cmd.Parameters.AddWithValue("@ThanhTien", dto.ThanhTien);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}