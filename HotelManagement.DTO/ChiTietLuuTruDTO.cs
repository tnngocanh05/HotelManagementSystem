using System;

namespace HotelManagement.DTO
{
    public class ChiTietLuuTruDTO
    {
        public int MaDatPhong { get; set; }
        public int MaKhachHang { get; set; }
        public int MaPhong { get; set; }

        public string SoPhong { get; set; }
        public string TenLoaiPhong { get; set; }

        public string HoTen { get; set; }
        public string CCCD { get; set; }
        public string SDT { get; set; }

        public DateTime ThoiGianNhan { get; set; }
        public DateTime NgayTraDuKien { get; set; }
        public string TrangThai { get; set; }
    }
}