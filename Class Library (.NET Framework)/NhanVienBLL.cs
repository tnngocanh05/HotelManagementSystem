using HotelManagement.DAL;
using HotelManagement.DTO;
using System.Collections.Generic;

public class NhanVienBLL
{
    private NhanVienDAL nhanVienDAL = new NhanVienDAL();

    public List<NhanVienDTO> GetAllNhanVien()
    {
        return nhanVienDAL.GetAllNhanVien();
    }
}