using HotelManagement.DAL;
using HotelManagement.DTO;
using System.Collections.Generic;

namespace HotelManagement.BLL
{
    public class KhachHangBLL
    {
        private static KhachHangBLL instance;
        public static KhachHangBLL Instance
        {
            get
            {
                if (instance == null)
                    instance = new KhachHangBLL();
                return instance;
            }
        }

        public List<KhachHangDTO> GetListKhachHang()
        {
            return KhachHangDAL.Instance.GetAll();
        }

        public bool ThemKhachHang(KhachHangDTO kh)
        {
            return KhachHangDAL.Instance.Insert(kh);
        }

        public bool UpdateKhachHang(KhachHangDTO kh)
        {
            return KhachHangDAL.Instance.Update(kh);
        }

        public bool DeleteKhachHang(int maKH)
        {
            return KhachHangDAL.Instance.Delete(maKH);
        }

        public List<KhachHangDTO> SearchKhachHang(string keyword)
        {
            return KhachHangDAL.Instance.Search(keyword);
        }
    }
}
