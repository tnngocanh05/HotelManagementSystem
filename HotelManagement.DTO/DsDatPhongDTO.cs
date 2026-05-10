using System;

namespace HotelManagement.DTO
{
    public class DsDatPhongDTO
    {
        public int MaDatPhong { get; set; }
        public int MaKhachHang { get; set; }
        public int MaPhong { get; set; }
        public DateTime ThoiGianDat { get; set; }
        public DateTime ThoiGianNhan { get; set; }
        public DateTime NgayTraDuKien { get; set; }
        public int SoNguoi { get; set; }
        public string TrangThai { get; set; }
    }
}