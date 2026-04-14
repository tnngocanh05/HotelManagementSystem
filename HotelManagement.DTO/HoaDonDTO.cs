using System;

namespace HotelManagement.DTO
{
    public class HoaDonDTO
    {
        public int MaHoaDon { get; set; }
        public string SoHoaDon { get; set; }
        public int MaDatPhong { get; set; }

        public decimal TienPhong { get; set; }
        public decimal TienDichVu { get; set; }
        public decimal TongTien { get; set; }

        public string PhuongThucThanhToan { get; set; }
        public decimal? TienKhachDua { get; set; }
        public decimal? TienTraLai { get; set; }

        public int? MaNhanVienLap { get; set; }
        public string TenNhanVienLap { get; set; }

        public string TrangThai { get; set; }
        public DateTime NgayLap { get; set; }
        public DateTime? NgayThanhToan { get; set; }

        // Dữ liệu hiển thị list
        public string SoPhong { get; set; }
        public string HoTenKhachHang { get; set; }
        public string CCCD { get; set; }
        public string SDT { get; set; }
        public string TenLoaiPhong { get; set; }
        public int SoDem { get; set; }
    }
}