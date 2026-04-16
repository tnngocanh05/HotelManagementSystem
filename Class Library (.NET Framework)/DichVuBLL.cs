using HotelManagement.DAL;
using HotelManagement.DTO;
using System.Collections.Generic;

public class DichVuBLL
{
    private DichVuDAL dal = new DichVuDAL();

    // Giữ nguyên hàm cũ của bạn
    public List<DichVuDTO> LayTatCa() => dal.LayDanhSach();

    // BỔ SUNG THÊM: Để file DichVuView.xaml.cs gọi không bị lỗi
    public List<DichVuDTO> GetAllDichVu() => LayTatCa();

    public bool Them(DichVuDTO dv) => dal.Them(dv);
    public bool Sua(DichVuDTO dv) => dal.Sua(dv);
    public bool Xoa(int ma) => dal.Xoa(ma);
}