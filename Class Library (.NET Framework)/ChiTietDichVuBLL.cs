using HotelManagement.DAL;
using HotelManagement.DTO;
using System.Collections.Generic;

namespace HotelManagement.BLL
{
    public class ChiTietDichVuBLL
    {
        private ChiTietDichVuDAL dal = new ChiTietDichVuDAL();

        public List<ChiTietDichVuDTO> GetByMaDatPhong(int maDatPhong)
        {
            return dal.GetByMaDatPhong(maDatPhong);
        }

        public bool InsertChiTietDichVu(ChiTietDichVuDTO dto)
        {
            if (dto.SoLuong <= 0)
                return false;
            return dal.InsertChiTietDichVu(dto);
        }
    }
}