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

        public List<PhongDTO> GetAvailableRooms(DateTime thoiGianNhanMoi, DateTime thoiGianTraMoi)
        {
            return dal.GetAvailableRooms(thoiGianNhanMoi, thoiGianTraMoi);
        }
    }
}