namespace HotelManagement.DTO
{
    public class ChiTietDichVuDTO
    {
        public int MaCTDV { get; set; }
        public int MaDatPhong { get; set; }
        public int MaDichVu { get; set; }
        public int SoLuong { get; set; }
        public decimal ThanhTien { get; set; }

        // Thuộc tính hỗ trợ hiển thị
        public string TenDichVu { get; set; }
        public decimal DonGia { get; set; }
    }
}