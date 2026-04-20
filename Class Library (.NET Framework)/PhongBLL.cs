using HotelManagement.DAL;
using HotelManagement.DTO;
using System;
using System.Collections.Generic;

namespace HotelManagement.BLL
{
    public class PhongBLL
    {
        private PhongDAL dal = new PhongDAL();

        public List<PhongDTO> GetAllRooms()
        {
            return dal.GetAllRooms();
        }

        public List<LoaiPhongDTO> GetAllRoomTypes()
        {
            return dal.GetAllRoomTypes();
        }

        public bool AddRoom(PhongDTO phong)
        {
            ValidateRoom(phong, false);
            return dal.AddRoom(phong);
        }

        public bool UpdateRoom(PhongDTO phong)
        {
            ValidateRoom(phong, true);
            return dal.UpdateRoom(phong);
        }

        public bool DeleteRoom(int maPhong)
        {
            if (maPhong <= 0)
            {
                throw new ArgumentException("Mã phòng không hợp lệ.");
            }

            return dal.DeleteRoom(maPhong);
        }

        public List<PhongDTO> GetAvailableRooms(DateTime thoiGianNhanMoi, DateTime thoiGianTraMoi)
        {
            return dal.GetAvailableRooms(thoiGianNhanMoi, thoiGianTraMoi);
        }

        private static void ValidateRoom(PhongDTO phong, bool isUpdate)
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

            if (string.IsNullOrWhiteSpace(phong.TinhTrang))
            {
                throw new ArgumentException("Trạng thái không được để trống.");
            }

            if (phong.MaLoaiPhong <= 0)
            {
                throw new ArgumentException("Loại phòng không hợp lệ.");
            }

            if (phong.Tang < 0)
            {
                throw new ArgumentException("Tầng không hợp lệ.");
            }
        }
    }
}
