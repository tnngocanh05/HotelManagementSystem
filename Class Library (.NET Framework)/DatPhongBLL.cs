using HotelManagement.DAL;
using HotelManagement.DTO;
using System;

namespace HotelManagement.BLL
{
    public class DatPhongBLL
    {
        private KhachHangDAL khachHangDAL = new KhachHangDAL();
        private DatPhongDAL datPhongDAL = new DatPhongDAL();
        private HoaDonDAL hoaDonDAL = new HoaDonDAL();

        public bool DatPhongTrucTiep(
            string soPhong,
            string hoTen,
            string cccd,
            string sdt,
            int soNguoi,
            DateTime ngayNhan,
            string gioNhan,
            DateTime ngayTraDuKien)
        {
            if (string.IsNullOrWhiteSpace(soPhong) ||
                string.IsNullOrWhiteSpace(hoTen) ||
                string.IsNullOrWhiteSpace(cccd) ||
                string.IsNullOrWhiteSpace(sdt) ||
                string.IsNullOrWhiteSpace(gioNhan) ||
                soNguoi <= 0)
            {
                return false;
            }

            KhachHangDTO khach = khachHangDAL.GetByCCCD(cccd);

            int maKhachHang;

            if (khach == null)
            {
                KhachHangDTO khachMoi = new KhachHangDTO
                {
                    HoTen = hoTen,
                    CCCD = cccd,
                    SDT = sdt
                };

                maKhachHang = khachHangDAL.InsertAndReturnId(khachMoi);
            }
            else
            {
                maKhachHang = khach.MaKhachHang;
            }

            int maPhong = datPhongDAL.GetMaPhongBySoPhong(soPhong);

            if (maPhong <= 0)
            {
                return false;
            }

            DateTime thoiGianNhan = DateTime.Parse(
                ngayNhan.ToString("yyyy-MM-dd") + " " + gioNhan);

            DatPhongDTO datPhong = new DatPhongDTO
            {
                MaKhachHang = maKhachHang,
                MaPhong = maPhong,
                ThoiGianDat = DateTime.Now,
                ThoiGianNhan = thoiGianNhan,
                NgayTraDuKien = ngayTraDuKien.Date,
                SoNguoi = soNguoi,
                TrangThai = "Đã đặt"
            };

            // SỬA: InsertDatPhong giờ trả về MaDatPhong
            int maDatPhong = datPhongDAL.InsertDatPhong(datPhong);

            if (maDatPhong > 0)
            {
                // Lồng thêm: tạo hóa đơn ngay khi đặt phòng
                bool taoHoaDon = hoaDonDAL.TaoHoaDonKhiDatPhong(maDatPhong);

                if (!taoHoaDon)
                {
                    return false;
                }

                datPhongDAL.UpdateTinhTrangPhong(maPhong, "Đang ở");
                return true;
            }

            return false;
        }

        public ChiTietLuuTruDTO GetChiTietLuuTruBySoPhong(string soPhong)
        {
            return datPhongDAL.GetChiTietLuuTruBySoPhong(soPhong);
        }
    }
}