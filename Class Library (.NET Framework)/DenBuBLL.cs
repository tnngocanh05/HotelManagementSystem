using HotelManagement.DAL;
using HotelManagement.DTO;

namespace HotelManagement.BLL
{
    public class DenBuBLL
    {
        private DenBuDAL denBuDAL = new DenBuDAL();

        public bool ThemDenBu(int maDatPhong,
                              string noiDung,
                              decimal soTien)
        {
            if (string.IsNullOrWhiteSpace(noiDung))
                return false;

            if (soTien <= 0)
                return false;

            DenBuDTO denBu = new DenBuDTO
            {
                MaDatPhong = maDatPhong,
                NoiDung = noiDung,
                SoTien = soTien
            };

            return denBuDAL.ThemDenBu(denBu);
        }

        public decimal TinhTienDenBu(int maDatPhong)
        {
            return denBuDAL.TinhTienDenBu(maDatPhong);
        }
    }
}