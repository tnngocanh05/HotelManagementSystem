using System;
using System.Collections.Generic;

namespace HotelManagement.DTO
{
    public class ChiTietHoaDonDTO
    {
        public string SoHoaDon { get; set; }
        public DateTime NgayLapHoaDon { get; set; }

        public string HoTenKhachHang { get; set; }
        public string SoPhong { get; set; }
        public string TenLoaiPhong { get; set; }

        public DateTime ThoiGianNhan { get; set; }
        public DateTime NgayTraDuKien { get; set; }
        public int SoDem { get; set; }

        public string TenNhanVienLap { get; set; }

        public string TrangThai { get; set; }
        public DateTime? NgayThanhToan { get; set; }
        public string PhuongThucThanhToan { get; set; }

        public decimal TienPhong { get; set; }
        public decimal TienDichVu { get; set; }
        public decimal TongTien { get; set; }

        public List<ChiTietDichVuDTO> DanhSachDichVu { get; set; }
    }
}