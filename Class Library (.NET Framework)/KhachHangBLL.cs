using HotelManagement.DAL;
using HotelManagement.DTO;
using System.Collections.Generic;

namespace HotelManagement.BLL
{
    public class KhachHangBLL
    {
        private KhachHangDAL khachHangDAL = new KhachHangDAL();

        public List<KhachHangDTO> GetListKhachHang()
        {
            return khachHangDAL.GetAll();
        }

        public bool ThemKhachHang(KhachHangDTO kh)
        {
            return khachHangDAL.Insert(kh);
        }

        public bool UpdateKhachHang(KhachHangDTO kh)
        {
            return khachHangDAL.Update(kh);
        }

        public bool DeleteKhachHang(int maKH)
        {
            return khachHangDAL.Delete(maKH);
        }

        public List<KhachHangDTO> SearchKhachHang(string keyword)
        {
            return khachHangDAL.Search(keyword);
        }
    }
}