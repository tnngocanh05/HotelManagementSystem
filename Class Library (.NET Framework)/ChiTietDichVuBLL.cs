using HotelManagement.DAL;
using HotelManagement.DTO;
using System.Collections.Generic;
using System.Linq;

namespace HotelManagement.BLL
{
    public class ChiTietDichVuBLL
    {
        private static ChiTietDichVuBLL instance;
        public static ChiTietDichVuBLL Instance
        {
            get
            {
                if (instance == null)
                    instance = new ChiTietDichVuBLL();
                return instance;
            }
        }

        public List<ChiTietDichVuDTO> GetList()
        {
            return ChiTietDichVuDAL.Instance.GetAll();
        }

        public List<ChiTietDichVuDTO> GetByMaDatPhong(int maDP)
        {
            return ChiTietDichVuDAL.Instance.GetByMaDatPhong(maDP);
        }

        // 🔥 THÊM (AUTO TÍNH TIỀN)
        public bool Them(ChiTietDichVuDTO ct)
        {
            var dv = DichVuDAL.Instance.GetAll()
                        .FirstOrDefault(x => x.MaDichVu == ct.MaDichVu);

            if (dv == null) return false;

            // 👉 TÍNH TIỀN TẠI ĐÂY
            ct.ThanhTien = ct.SoLuong * dv.DonGia;

            return ChiTietDichVuDAL.Instance.Insert(ct);
        }

        public bool Sua(ChiTietDichVuDTO ct)
        {
            var dv = DichVuDAL.Instance.GetAll()
                        .FirstOrDefault(x => x.MaDichVu == ct.MaDichVu);

            if (dv == null) return false;

            ct.ThanhTien = ct.SoLuong * dv.DonGia;

            return ChiTietDichVuDAL.Instance.Update(ct);
        }

        public bool Xoa(int ma)
        {
            return ChiTietDichVuDAL.Instance.Delete(ma);
        }
    }
}