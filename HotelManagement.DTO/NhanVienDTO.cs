using System;

namespace HotelManagement.DTO
{
    public class NhanVienDTO
    {
        public int MaNV { get; set; }
        public string HoTen { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string SDT { get; set; }
        public string CCCD { get; set; }
        public string Email { get; set; }
        public int? MaChucVu { get; set; }
        public string TenChucVu { get; set; }
    }
}