using HotelManagement.DAL;
using HotelManagement.DTO;
using System.Collections.Generic;

namespace HotelManagement.BLL
{
    public class DichVuBLL
    {
        private static DichVuBLL instance;
        public static DichVuBLL Instance
        {
            get
            {
                if (instance == null)
                    instance = new DichVuBLL();
                return instance;
            }
        }

        // ===== LẤY DANH SÁCH =====
        public List<DichVuDTO> GetListDichVu()
        {
            return DichVuDAL.Instance.GetAll();
        }

        // ===== THÊM =====
        public bool ThemDichVu(DichVuDTO dv)
        {
            if (dv == null) return false;

            if (string.IsNullOrWhiteSpace(dv.TenDichVu))
                return false;

            if (dv.DonGia <= 0)
                return false;

            if (string.IsNullOrWhiteSpace(dv.LoaiDichVu))
                return false;

            return DichVuDAL.Instance.Insert(dv);
        }

        // ===== SỬA =====
        public bool UpdateDichVu(DichVuDTO dv)
        {
            if (dv == null) return false;

            if (dv.MaDichVu <= 0)
                return false;

            return DichVuDAL.Instance.Update(dv);
        }

        // ===== XÓA =====
        public bool DeleteDichVu(int maDV)
        {
            if (maDV <= 0)
                return false;

            return DichVuDAL.Instance.Delete(maDV);
        }

        // ===== SEARCH =====
        public List<DichVuDTO> SearchDichVu(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return GetListDichVu();

            return DichVuDAL.Instance.Search(keyword);
        }
    }
}