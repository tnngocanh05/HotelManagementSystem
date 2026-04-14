using System;
using System.Collections.Generic;
using QuanLyKhachSanWeb.DAL;
using QuanLyKhachSanWeb.DTO;

namespace QuanLyKhachSanWeb.BLL
{
    public class PhongBLL
    {
        private readonly PhongDAL _phongDal = new PhongDAL();

        public List<PhongDTO> LayDanhSachPhong()
        {
            return _phongDal.LayDanhSachPhong();
        }

        public List<LoaiPhongDTO> LayDanhSachLoaiPhong()
        {
            return _phongDal.LayDanhSachLoaiPhong();
        }

        public bool ThemPhong(PhongDTO phong)
        {
            KiemTraDuLieuPhong(phong, false);
            return _phongDal.ThemPhong(phong);
        }

        public bool SuaPhong(PhongDTO phong)
        {
            KiemTraDuLieuPhong(phong, true);
            return _phongDal.SuaPhong(phong);
        }

        public bool XoaPhong(int maPhong)
        {
            if (maPhong <= 0)
            {
                throw new ArgumentException("Mã phòng không hợp lệ.");
            }

            return _phongDal.XoaPhong(maPhong);
        }

        private static void KiemTraDuLieuPhong(PhongDTO phong, bool isUpdate)
        {
            if (phong == null)
            {
                throw new ArgumentNullException(nameof(phong));
            }

            if (isUpdate && phong.MaPhong <= 0)
            {
                throw new ArgumentException("Mã phòng không hợp lệ.");
            }

            if (string.IsNullOrWhiteSpace(phong.SoPhong))
            {
                throw new ArgumentException("Số phòng không được để trống.");
            }

            if (phong.SoPhong.Length > 10)
            {
                throw new ArgumentException("Số phòng tối đa 10 ký tự.");
            }

            if (string.IsNullOrWhiteSpace(phong.TinhTrang))
            {
                throw new ArgumentException("Trạng thái không được để trống.");
            }

            if (phong.Tang < 0)
            {
                throw new ArgumentException("Tầng không hợp lệ.");
            }

            if (phong.MaLoaiPhong <= 0)
            {
                throw new ArgumentException("Loại phòng không hợp lệ.");
            }
        }
    }
}
