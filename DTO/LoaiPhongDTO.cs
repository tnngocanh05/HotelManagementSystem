namespace QuanLyKhachSanWeb.DTO
{
    public class LoaiPhongDTO
    {
        public int MaLoaiPhong { get; set; }

        public string TenLoaiPhong { get; set; }

        public int SoNguoiToiDa { get; set; }

        public decimal GiaTien { get; set; }

        public string HienThi => $"{TenLoaiPhong} - {SoNguoiToiDa} người - {GiaTien:N0} VNĐ";
    }
}
