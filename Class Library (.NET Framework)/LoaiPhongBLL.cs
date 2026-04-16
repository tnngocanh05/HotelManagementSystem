using System.Collections.Generic;
using HotelManagement.DAL;
using HotelManagement.DTO;

namespace HotelManagement.BLL
{
    public class LoaiPhongBLL
    {
        private LoaiPhongDAL dal = new LoaiPhongDAL();

        public List<LoaiPhongDTO> LayTatCa() => dal.LayDanhSach();

        public bool Them(LoaiPhongDTO lp)
        {
            // Kiểm tra logic cơ bản
            if (string.IsNullOrEmpty(lp.TenLoaiPhong) || lp.GiaTien < 0)
            {
                return false;
            }

            // Trả về kết quả từ DAL (true/false)
            return dal.Them(lp);
        }

        public bool Sua(LoaiPhongDTO lp)
        {
            return dal.SuaLoaiPhong(lp);
        }

        public bool Xoa(int maLoai)
        {
            return dal.XoaLoaiPhong(maLoai);
        }
    }
}