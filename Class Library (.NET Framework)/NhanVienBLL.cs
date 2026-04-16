using System;
using System.Collections.Generic;
using HotelManagement.DAL;
using HotelManagement.DTO;

namespace HotelManagement.BLL
{
    public class NhanVienBLL
    {
        private NhanVienDAL dal = new NhanVienDAL();

        // Lấy toàn bộ danh sách nhân viên
        public List<NhanVienDTO> LayTatCa()
        {
            return dal.LayDanhSach();
        }

        // Thêm nhân viên mới
        public bool Them(NhanVienDTO nv)
        {
            // Bạn có thể thêm các logic kiểm tra ở đây trước khi gọi DAL
            // Ví dụ: Kiểm tra tên không được để trống
            if (string.IsNullOrWhiteSpace(nv.HoTen)) return false;

            return dal.Them(nv);
        }

        // Sửa thông tin nhân viên
        public bool Sua(NhanVienDTO nv)
        {
            return dal.Sua(nv);
        }

        // Xóa nhân viên theo Mã
        public bool Xoa(int ma)
        {
            return dal.Xoa(ma);
        }
    }
}