using HotelManagement.DAL;
using HotelManagement.DTO;
using System;
using System.Collections.Generic;

namespace HotelManagement.BLL
{
    public class PhongBLL
    {
        private PhongDAL dal = new PhongDAL();

        // ĐỔI TÊN TỪ LayTatCa THÀNH GetAllRooms ĐỂ HẾT LỖI GẠCH ĐỎ
        public List<PhongDTO> GetAllRooms()
        {
            try
            {
                return dal.GetAllRooms();
            }
            catch (Exception)
            {
                return new List<PhongDTO>();
            }
        }

        // Giữ nguyên để phục vụ chức năng tìm phòng trống
        public List<PhongDTO> GetAvailableRooms(DateTime thoiGianNhanMoi, DateTime thoiGianTraMoi)
        {
            try
            {
                return dal.GetAvailableRooms(thoiGianNhanMoi, thoiGianTraMoi);
            }
            catch (Exception)
            {
                return new List<PhongDTO>();
            }
        }

        public bool Xoa(int maPhong)
        {
            try { return dal.Xoa(maPhong); }
            catch { return false; }
        }

        public bool Them(PhongDTO p)
        {
            try { return dal.Them(p); }
            catch { return false; }
        }

        public bool Sua(PhongDTO p)
        {
            try { return dal.Sua(p); }
            catch { return false; }
        }
    }
}