using HotelManagement.DAL;
using HotelManagement.DTO;
using System;
using System.Collections.Generic;

namespace HotelManagement.BLL
{
    public class HoaDonBLL
    {
        private HoaDonDAL hoaDonDAL = new HoaDonDAL();
        private DatPhongDAL datPhongDAL = new DatPhongDAL();
        private PhongDAL phongDAL = new PhongDAL();

        // =========================
        // 1. LẤY TOÀN BỘ DANH SÁCH HÓA ĐƠN
        // =========================
        public List<HoaDonDTO> GetAllHoaDon()
        {
            return hoaDonDAL.GetAllHoaDon();
        }

        // =========================
        // 2. TÌM KIẾM + LỌC HÓA ĐƠN
        // =========================
        public List<HoaDonDTO> SearchHoaDon(string keyword, DateTime? tuNgay, DateTime? denNgay, string trangThai)
        {
            return hoaDonDAL.SearchHoaDon(keyword, tuNgay, denNgay, trangThai);
        }

        // =========================
        // 3. LẤY CHI TIẾT 1 HÓA ĐƠN
        // =========================
        public ChiTietHoaDonDTO GetChiTietHoaDonByMaHoaDon(int maHoaDon)
        {
            return hoaDonDAL.GetChiTietHoaDonByMaHoaDon(maHoaDon);
        }

        // =========================
        // 4. KIỂM TRA ĐÃ CÓ HÓA ĐƠN CHƯA
        // =========================
        public bool ExistsHoaDonByMaDatPhong(int maDatPhong)
        {
            return hoaDonDAL.ExistsHoaDonByMaDatPhong(maDatPhong);
        }

        // =========================
        // 5. TẠO SỐ HÓA ĐƠN TẠM
        // Giữ lại để tương thích nếu nơi khác đang gọi
        // =========================
        public string TaoSoHoaDonTam(int maHoaDonTam)
        {
            return "HD" + maHoaDonTam.ToString("000");
        }

        // =========================
        // 6. TÍNH TIỀN PHÒNG
        // =========================
        public decimal TinhTienPhong(int maDatPhong)
        {
            DatPhongDTO dp = datPhongDAL.GetDatPhongById(maDatPhong);
            if (dp == null) return 0;

            decimal giaPhong = phongDAL.GetGiaPhongByMaPhong(dp.MaPhong);

            int soDem = (dp.NgayTraDuKien.Date - dp.ThoiGianNhan.Date).Days;
            if (soDem <= 0) soDem = 1;

            return giaPhong * soDem;
        }

        // =========================
        // 7. TÍNH TIỀN DỊCH VỤ
        // =========================
        public decimal TinhTienDichVu(int maDatPhong)
        {
            return datPhongDAL.GetTongTienDichVu(maDatPhong);
        }

        // =========================
        // 8. TÍNH TỔNG TIỀN
        // =========================
        public decimal TinhTongTien(int maDatPhong)
        {
            return TinhTienPhong(maDatPhong) + TinhTienDichVu(maDatPhong);
        }

        // =========================
        // 9. THANH TOÁN
        // - KHÔNG tạo hóa đơn mới nữa
        // - Chỉ cập nhật hóa đơn đã có sẵn từ lúc đặt phòng
        // - Update DatPhong = Đã trả
        // - Update Phong = Dọn dẹp
        // =========================
        public bool ThanhToan(int maDatPhong, string phuongThuc, decimal? tienKhachDua, out string message)
        {
            return ThanhToan(maDatPhong, phuongThuc, tienKhachDua, null, out message);
        }

        // =========================
        // 10. THANH TOÁN CÓ NHÂN VIÊN
        // =========================
        public bool ThanhToan(int maDatPhong, string phuongThuc, decimal? tienKhachDua, int? maNhanVienLap, out string message)
        {
            message = "";

            // Vì đặt phòng xong là đã có hóa đơn "Chưa thanh toán"
            if (!hoaDonDAL.ExistsHoaDonByMaDatPhong(maDatPhong))
            {
                message = "Không tìm thấy hóa đơn của đặt phòng này.";
                return false;
            }

            DatPhongDTO dp = datPhongDAL.GetDatPhongById(maDatPhong);
            if (dp == null)
            {
                message = "Không tìm thấy đặt phòng.";
                return false;
            }

            decimal tienPhong = TinhTienPhong(maDatPhong);
            decimal tienDichVu = TinhTienDichVu(maDatPhong);
            decimal tongTien = tienPhong + tienDichVu;

            decimal? tienTraLai = null;

            if (phuongThuc == "Tiền mặt")
            {
                if (!tienKhachDua.HasValue)
                {
                    message = "Vui lòng nhập tiền khách đưa.";
                    return false;
                }

                if (tienKhachDua.Value < tongTien)
                {
                    message = "Tiền khách đưa không đủ.";
                    return false;
                }

                tienTraLai = tienKhachDua.Value - tongTien;
            }

            bool updateHoaDon = hoaDonDAL.CapNhatThanhToanHoaDon(
                maDatPhong,
                tienPhong,
                tienDichVu,
                phuongThuc,
                tienKhachDua,
                tienTraLai,
                maNhanVienLap);

            if (!updateHoaDon)
            {
                message = "Không thể cập nhật thanh toán hóa đơn.";
                return false;
            }

            bool updateDatPhong = datPhongDAL.UpdateTrangThaiDatPhong(maDatPhong, "Đã trả");
            bool updatePhong = datPhongDAL.UpdateTinhTrangPhong(dp.MaPhong, "Dọn dẹp");

            if (!updateDatPhong || !updatePhong)
            {
                message = "Thanh toán thành công nhưng cập nhật trạng thái chưa hoàn tất.";
                return false;
            }

            message = "Thanh toán thành công.";
            return true;
        }
    }
}