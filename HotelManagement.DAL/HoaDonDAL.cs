using HotelManagement.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace HotelManagement.DAL
{
    public class HoaDonDAL
    {
        private DBConnection db = new DBConnection();

        // =========================
        // 1. LẤY TOÀN BỘ DANH SÁCH HÓA ĐƠN
        // =========================
        public List<HoaDonDTO> GetAllHoaDon()
        {
            List<HoaDonDTO> list = new List<HoaDonDTO>();

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT 
                        hd.MaHoaDon,
                        hd.SoHoaDon,
                        hd.MaDatPhong,
                        hd.TienPhong,
                        hd.TienDichVu,
                        hd.TongTien,
                        hd.PhuongThucThanhToan,
                        hd.TienKhachDua,
                        hd.TienTraLai,
                        hd.MaNhanVienLap,
                        hd.TrangThai,
                        hd.NgayLap,
                        hd.NgayThanhToan,

                        p.SoPhong,
                        kh.HoTen AS HoTenKhachHang,
                        kh.CCCD,
                        kh.SDT,
                        lp.TenLoaiPhong,
                        ISNULL(nv.HoTen, N'') AS TenNhanVienLap,

                        DATEDIFF(DAY, CAST(dp.ThoiGianNhan AS DATE), dp.NgayTraDuKien) AS SoDem
                    FROM HoaDon hd
                    INNER JOIN DatPhong dp ON hd.MaDatPhong = dp.MaDatPhong
                    INNER JOIN KhachHang kh ON dp.MaKhachHang = kh.MaKhachHang
                    INNER JOIN Phong p ON dp.MaPhong = p.MaPhong
                    INNER JOIN LoaiPhong lp ON p.MaLoaiPhong = lp.MaLoaiPhong
                    LEFT JOIN NhanVien nv ON hd.MaNhanVienLap = nv.MaNhanVien
                    ORDER BY hd.MaHoaDon DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    HoaDonDTO hd = new HoaDonDTO
                    {
                        MaHoaDon = Convert.ToInt32(rd["MaHoaDon"]),
                        SoHoaDon = rd["SoHoaDon"] == DBNull.Value ? "" : rd["SoHoaDon"].ToString(),
                        MaDatPhong = Convert.ToInt32(rd["MaDatPhong"]),

                        TienPhong = Convert.ToDecimal(rd["TienPhong"]),
                        TienDichVu = Convert.ToDecimal(rd["TienDichVu"]),
                        TongTien = Convert.ToDecimal(rd["TongTien"]),

                        PhuongThucThanhToan = rd["PhuongThucThanhToan"] == DBNull.Value ? "" : rd["PhuongThucThanhToan"].ToString(),
                        TienKhachDua = rd["TienKhachDua"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rd["TienKhachDua"]),
                        TienTraLai = rd["TienTraLai"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rd["TienTraLai"]),

                        MaNhanVienLap = rd["MaNhanVienLap"] == DBNull.Value ? (int?)null : Convert.ToInt32(rd["MaNhanVienLap"]),
                        TenNhanVienLap = rd["TenNhanVienLap"].ToString(),

                        TrangThai = rd["TrangThai"].ToString(),
                        NgayLap = Convert.ToDateTime(rd["NgayLap"]),
                        NgayThanhToan = rd["NgayThanhToan"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rd["NgayThanhToan"]),

                        SoPhong = rd["SoPhong"].ToString(),
                        HoTenKhachHang = rd["HoTenKhachHang"].ToString(),
                        CCCD = rd["CCCD"].ToString(),
                        SDT = rd["SDT"].ToString(),
                        TenLoaiPhong = rd["TenLoaiPhong"].ToString(),
                        SoDem = Convert.ToInt32(rd["SoDem"])
                    };

                    list.Add(hd);
                }
            }

            return list;
        }

        // =========================
        // 2. TÌM KIẾM + LỌC NGÀY + LỌC TRẠNG THÁI
        // =========================
        public List<HoaDonDTO> SearchHoaDon(string keyword, DateTime? tuNgay, DateTime? denNgay, string trangThai)
        {
            List<HoaDonDTO> list = new List<HoaDonDTO>();

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT 
                        hd.MaHoaDon,
                        hd.SoHoaDon,
                        hd.MaDatPhong,
                        hd.TienPhong,
                        hd.TienDichVu,
                        hd.TongTien,
                        hd.PhuongThucThanhToan,
                        hd.TienKhachDua,
                        hd.TienTraLai,
                        hd.MaNhanVienLap,
                        hd.TrangThai,
                        hd.NgayLap,
                        hd.NgayThanhToan,

                        p.SoPhong,
                        kh.HoTen AS HoTenKhachHang,
                        kh.CCCD,
                        kh.SDT,
                        lp.TenLoaiPhong,
                        ISNULL(nv.HoTen, N'') AS TenNhanVienLap,

                        DATEDIFF(DAY, CAST(dp.ThoiGianNhan AS DATE), dp.NgayTraDuKien) AS SoDem
                    FROM HoaDon hd
                    INNER JOIN DatPhong dp ON hd.MaDatPhong = dp.MaDatPhong
                    INNER JOIN KhachHang kh ON dp.MaKhachHang = kh.MaKhachHang
                    INNER JOIN Phong p ON dp.MaPhong = p.MaPhong
                    INNER JOIN LoaiPhong lp ON p.MaLoaiPhong = lp.MaLoaiPhong
                    LEFT JOIN NhanVien nv ON hd.MaNhanVienLap = nv.MaNhanVien
                    WHERE
                        (@Keyword = '' OR
                         p.SoPhong LIKE '%' + @Keyword + '%' OR
                         kh.SDT LIKE '%' + @Keyword + '%' OR
                         kh.CCCD LIKE '%' + @Keyword + '%' OR
                         kh.HoTen LIKE N'%' + @Keyword + N'%')
                    AND
                        (@TuNgay IS NULL OR CAST(hd.NgayLap AS DATE) >= @TuNgay)
                    AND
                        (@DenNgay IS NULL OR CAST(hd.NgayLap AS DATE) <= @DenNgay)
                    AND
                        (@TrangThai = N'Tất cả' OR hd.TrangThai = @TrangThai)
                    ORDER BY hd.MaHoaDon DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Keyword", string.IsNullOrWhiteSpace(keyword) ? "" : keyword.Trim());

                if (tuNgay.HasValue)
                    cmd.Parameters.AddWithValue("@TuNgay", tuNgay.Value.Date);
                else
                    cmd.Parameters.AddWithValue("@TuNgay", DBNull.Value);

                if (denNgay.HasValue)
                    cmd.Parameters.AddWithValue("@DenNgay", denNgay.Value.Date);
                else
                    cmd.Parameters.AddWithValue("@DenNgay", DBNull.Value);

                cmd.Parameters.AddWithValue("@TrangThai", string.IsNullOrWhiteSpace(trangThai) ? "Tất cả" : trangThai);

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    HoaDonDTO hd = new HoaDonDTO
                    {
                        MaHoaDon = Convert.ToInt32(rd["MaHoaDon"]),
                        SoHoaDon = rd["SoHoaDon"] == DBNull.Value ? "" : rd["SoHoaDon"].ToString(),
                        MaDatPhong = Convert.ToInt32(rd["MaDatPhong"]),

                        TienPhong = Convert.ToDecimal(rd["TienPhong"]),
                        TienDichVu = Convert.ToDecimal(rd["TienDichVu"]),
                        TongTien = Convert.ToDecimal(rd["TongTien"]),

                        PhuongThucThanhToan = rd["PhuongThucThanhToan"] == DBNull.Value ? "" : rd["PhuongThucThanhToan"].ToString(),
                        TienKhachDua = rd["TienKhachDua"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rd["TienKhachDua"]),
                        TienTraLai = rd["TienTraLai"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rd["TienTraLai"]),

                        MaNhanVienLap = rd["MaNhanVienLap"] == DBNull.Value ? (int?)null : Convert.ToInt32(rd["MaNhanVienLap"]),
                        TenNhanVienLap = rd["TenNhanVienLap"].ToString(),

                        TrangThai = rd["TrangThai"].ToString(),
                        NgayLap = Convert.ToDateTime(rd["NgayLap"]),
                        NgayThanhToan = rd["NgayThanhToan"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rd["NgayThanhToan"]),

                        SoPhong = rd["SoPhong"].ToString(),
                        HoTenKhachHang = rd["HoTenKhachHang"].ToString(),
                        CCCD = rd["CCCD"].ToString(),
                        SDT = rd["SDT"].ToString(),
                        TenLoaiPhong = rd["TenLoaiPhong"].ToString(),
                        SoDem = Convert.ToInt32(rd["SoDem"])
                    };

                    list.Add(hd);
                }
            }

            return list;
        }

        // =========================
        // 3. LẤY CHI TIẾT 1 HÓA ĐƠN THEO MaHoaDon
        // =========================
        public ChiTietHoaDonDTO GetChiTietHoaDonByMaHoaDon(int maHoaDon)
        {
            ChiTietHoaDonDTO result = null;

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT TOP 1
                        hd.MaHoaDon,
                        hd.SoHoaDon,
                        hd.NgayLap,
                        hd.NgayThanhToan,
                        hd.TrangThai,
                        hd.PhuongThucThanhToan,
                        hd.TienPhong,
                        hd.TienDichVu,
                        hd.TongTien,

                        kh.HoTen AS HoTenKhachHang,
                        p.SoPhong,
                        lp.TenLoaiPhong,
                        dp.ThoiGianNhan,
                        dp.NgayTraDuKien,
                        DATEDIFF(DAY, CAST(dp.ThoiGianNhan AS DATE), dp.NgayTraDuKien) AS SoDem,

                        ISNULL(nv.HoTen, N'') AS TenNhanVienLap
                    FROM HoaDon hd
                    INNER JOIN DatPhong dp ON hd.MaDatPhong = dp.MaDatPhong
                    INNER JOIN KhachHang kh ON dp.MaKhachHang = kh.MaKhachHang
                    INNER JOIN Phong p ON dp.MaPhong = p.MaPhong
                    INNER JOIN LoaiPhong lp ON p.MaLoaiPhong = lp.MaLoaiPhong
                    LEFT JOIN NhanVien nv ON hd.MaNhanVienLap = nv.MaNhanVien
                    WHERE hd.MaHoaDon = @MaHoaDon";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);

                SqlDataReader rd = cmd.ExecuteReader();

                if (rd.Read())
                {
                    result = new ChiTietHoaDonDTO
                    {
                        SoHoaDon = rd["SoHoaDon"] == DBNull.Value ? "" : rd["SoHoaDon"].ToString(),
                        NgayLapHoaDon = Convert.ToDateTime(rd["NgayLap"]),
                        HoTenKhachHang = rd["HoTenKhachHang"].ToString(),
                        SoPhong = rd["SoPhong"].ToString(),
                        TenLoaiPhong = rd["TenLoaiPhong"].ToString(),
                        ThoiGianNhan = Convert.ToDateTime(rd["ThoiGianNhan"]),
                        NgayTraDuKien = Convert.ToDateTime(rd["NgayTraDuKien"]),
                        SoDem = Convert.ToInt32(rd["SoDem"]),
                        TenNhanVienLap = rd["TenNhanVienLap"].ToString(),
                        TrangThai = rd["TrangThai"].ToString(),
                        NgayThanhToan = rd["NgayThanhToan"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rd["NgayThanhToan"]),
                        PhuongThucThanhToan = rd["PhuongThucThanhToan"] == DBNull.Value ? "" : rd["PhuongThucThanhToan"].ToString(),
                        TienPhong = Convert.ToDecimal(rd["TienPhong"]),
                        TienDichVu = Convert.ToDecimal(rd["TienDichVu"]),
                        TongTien = Convert.ToDecimal(rd["TongTien"]),
                        DanhSachDichVu = new List<ChiTietDichVuDTO>()
                    };
                }

                rd.Close();

                if (result != null)
                {
                    string queryDV = @"
                        SELECT
                            ctdv.MaCTDV,
                            dv.TenDichVu,
                            dv.LoaiDichVu,
                            ctdv.SoLuong,
                            dv.DonGia,
                            ctdv.ThanhTien
                        FROM HoaDon hd
                        INNER JOIN ChiTietDichVu ctdv ON hd.MaDatPhong = ctdv.MaDatPhong
                        INNER JOIN DichVu dv ON ctdv.MaDichVu = dv.MaDichVu
                        WHERE hd.MaHoaDon = @MaHoaDon
                        ORDER BY ctdv.MaCTDV";

                    SqlCommand cmdDV = new SqlCommand(queryDV, conn);
                    cmdDV.Parameters.AddWithValue("@MaHoaDon", maHoaDon);

                    SqlDataReader rdDV = cmdDV.ExecuteReader();

                    while (rdDV.Read())
                    {
                        result.DanhSachDichVu.Add(new ChiTietDichVuDTO
                        {
                            MaCTDV = Convert.ToInt32(rdDV["MaCTDV"]),
                            TenDichVu = rdDV["TenDichVu"].ToString(),
                            LoaiDichVu = rdDV["LoaiDichVu"].ToString(),
                            SoLuong = Convert.ToInt32(rdDV["SoLuong"]),
                            DonGia = Convert.ToDecimal(rdDV["DonGia"]),
                            ThanhTien = Convert.ToDecimal(rdDV["ThanhTien"])
                        });
                    }

                    rdDV.Close();
                }
            }

            return result;
        }

        // =========================
        // 4. THÊM HÓA ĐƠN
        // =========================
        public bool InsertHoaDon(HoaDonDTO hd)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = @"
                    INSERT INTO HoaDon
                    (
                        SoHoaDon,
                        MaDatPhong,
                        TienPhong,
                        TienDichVu,
                        PhuongThucThanhToan,
                        TienKhachDua,
                        TienTraLai,
                        MaNhanVienLap,
                        TrangThai,
                        NgayLap,
                        NgayThanhToan
                    )
                    VALUES
                    (
                        @SoHoaDon,
                        @MaDatPhong,
                        @TienPhong,
                        @TienDichVu,
                        @PhuongThucThanhToan,
                        @TienKhachDua,
                        @TienTraLai,
                        @MaNhanVienLap,
                        @TrangThai,
                        @NgayLap,
                        @NgayThanhToan
                    )";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SoHoaDon", (object)hd.SoHoaDon ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MaDatPhong", hd.MaDatPhong);
                cmd.Parameters.AddWithValue("@TienPhong", hd.TienPhong);
                cmd.Parameters.AddWithValue("@TienDichVu", hd.TienDichVu);
                cmd.Parameters.AddWithValue("@PhuongThucThanhToan", (object)hd.PhuongThucThanhToan ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TienKhachDua", (object)hd.TienKhachDua ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TienTraLai", (object)hd.TienTraLai ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MaNhanVienLap", (object)hd.MaNhanVienLap ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TrangThai", hd.TrangThai);
                cmd.Parameters.AddWithValue("@NgayLap", hd.NgayLap);
                cmd.Parameters.AddWithValue("@NgayThanhToan", (object)hd.NgayThanhToan ?? DBNull.Value);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // =========================
        // 5. KIỂM TRA ĐÃ CÓ HÓA ĐƠN CHƯA
        // =========================
        public bool ExistsHoaDonByMaDatPhong(int maDatPhong)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = "SELECT COUNT(*) FROM HoaDon WHERE MaDatPhong = @MaDatPhong";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaDatPhong", maDatPhong);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }
    }
}