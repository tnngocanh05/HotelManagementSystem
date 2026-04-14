using HotelManagement.DTO;
using System;
using System.Data.SqlClient;

namespace HotelManagement.DAL
{
    public class DatPhongDAL
    {
        private DBConnection db = new DBConnection();

        // 🔎 Lấy MaPhong từ SoPhong
        public int GetMaPhongBySoPhong(string soPhong)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = "SELECT MaPhong FROM Phong WHERE SoPhong = @SoPhong";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SoPhong", soPhong);

                object result = cmd.ExecuteScalar();

                if (result != null)
                    return Convert.ToInt32(result);

                return -1;
            }
        }

        // ➕ Thêm đặt phòng
        public bool InsertDatPhong(DatPhongDTO dp)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = @"
                    INSERT INTO DatPhong
                    (MaKhachHang, MaPhong, ThoiGianDat, ThoiGianNhan, NgayTraDuKien, SoNguoi, TrangThai)
                    VALUES
                    (@MaKhachHang, @MaPhong, @ThoiGianDat, @ThoiGianNhan, @NgayTraDuKien, @SoNguoi, @TrangThai)
                ";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@MaKhachHang", dp.MaKhachHang);
                cmd.Parameters.AddWithValue("@MaPhong", dp.MaPhong);
                cmd.Parameters.AddWithValue("@ThoiGianDat", dp.ThoiGianDat);
                cmd.Parameters.AddWithValue("@ThoiGianNhan", dp.ThoiGianNhan);
                cmd.Parameters.AddWithValue("@NgayTraDuKien", dp.NgayTraDuKien);
                cmd.Parameters.AddWithValue("@SoNguoi", dp.SoNguoi);
                cmd.Parameters.AddWithValue("@TrangThai", dp.TrangThai);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // 🔄 Update trạng thái phòng
        public bool UpdateTinhTrangPhong(int maPhong, string tinhTrang)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = "UPDATE Phong SET TinhTrang = @TinhTrang WHERE MaPhong = @MaPhong";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@TinhTrang", tinhTrang);
                cmd.Parameters.AddWithValue("@MaPhong", maPhong);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // 📌 Lấy chi tiết lưu trú theo số phòng
        public ChiTietLuuTruDTO GetChiTietLuuTruBySoPhong(string soPhong)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT TOP 1
                        dp.MaDatPhong,
                        dp.MaKhachHang,
                        dp.MaPhong,
                        p.SoPhong,
                        lp.TenLoaiPhong,
                        kh.HoTen,
                        kh.CCCD,
                        kh.SDT,
                        dp.ThoiGianNhan,
                        dp.NgayTraDuKien,
                        dp.TrangThai
                    FROM DatPhong dp
                    INNER JOIN KhachHang kh ON dp.MaKhachHang = kh.MaKhachHang
                    INNER JOIN Phong p ON dp.MaPhong = p.MaPhong
                    INNER JOIN LoaiPhong lp ON p.MaLoaiPhong = lp.MaLoaiPhong
                    WHERE p.SoPhong = @SoPhong
                      AND dp.TrangThai IN (N'Đang ở', N'Đã đặt')
                    ORDER BY dp.ThoiGianNhan DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SoPhong", soPhong);

                SqlDataReader rd = cmd.ExecuteReader();

                if (rd.Read())
                {
                    return new ChiTietLuuTruDTO
                    {
                        MaDatPhong = (int)rd["MaDatPhong"],
                        MaKhachHang = (int)rd["MaKhachHang"],
                        MaPhong = (int)rd["MaPhong"],
                        SoPhong = rd["SoPhong"].ToString(),
                        TenLoaiPhong = rd["TenLoaiPhong"].ToString(),
                        HoTen = rd["HoTen"].ToString(),
                        CCCD = rd["CCCD"].ToString(),
                        SDT = rd["SDT"].ToString(),
                        ThoiGianNhan = (DateTime)rd["ThoiGianNhan"],
                        NgayTraDuKien = (DateTime)rd["NgayTraDuKien"],
                        TrangThai = rd["TrangThai"].ToString()
                    };
                }

                return null;
            }
        }

        // 📌 Lấy thông tin đặt phòng theo MaDatPhong
        public DatPhongDTO GetDatPhongById(int maDatPhong)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT MaDatPhong, MaKhachHang, MaPhong, ThoiGianDat, ThoiGianNhan,
                           NgayTraDuKien, SoNguoi, TrangThai
                    FROM DatPhong
                    WHERE MaDatPhong = @MaDatPhong";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaDatPhong", maDatPhong);

                SqlDataReader rd = cmd.ExecuteReader();

                if (rd.Read())
                {
                    return new DatPhongDTO
                    {
                        MaDatPhong = (int)rd["MaDatPhong"],
                        MaKhachHang = (int)rd["MaKhachHang"],
                        MaPhong = (int)rd["MaPhong"],
                        ThoiGianDat = (DateTime)rd["ThoiGianDat"],
                        ThoiGianNhan = (DateTime)rd["ThoiGianNhan"],
                        NgayTraDuKien = (DateTime)rd["NgayTraDuKien"],
                        SoNguoi = (int)rd["SoNguoi"],
                        TrangThai = rd["TrangThai"].ToString()
                    };
                }

                return null;
            }
        }

        // 📌 Cập nhật trạng thái đặt phòng
        public bool UpdateTrangThaiDatPhong(int maDatPhong, string trangThai)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = "UPDATE DatPhong SET TrangThai = @TrangThai WHERE MaDatPhong = @MaDatPhong";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TrangThai", trangThai);
                cmd.Parameters.AddWithValue("@MaDatPhong", maDatPhong);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // 📌 Tính tổng tiền dịch vụ theo MaDatPhong
        public decimal GetTongTienDichVu(int maDatPhong)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT ISNULL(SUM(ThanhTien), 0)
                    FROM ChiTietDichVu
                    WHERE MaDatPhong = @MaDatPhong";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaDatPhong", maDatPhong);

                object result = cmd.ExecuteScalar();
                return Convert.ToDecimal(result);
            }
        }
    }
}