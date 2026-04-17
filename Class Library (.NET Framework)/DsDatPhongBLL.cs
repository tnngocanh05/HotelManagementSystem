using System;
using System.Collections.Generic;
using HotelManagement.DAL;
using HotelManagement.DTO;

namespace Class_Library__.NET_Framework_
{
    public class DsDatPhongBLL
    {
        // 🔥 Singleton
        private static DsDatPhongBLL instance;
        public static DsDatPhongBLL Instance
        {
            get
            {
                if (instance == null)
                    instance = new DsDatPhongBLL();
                return instance;
            }
        }

        // 🔥 Lấy danh sách
        public List<DsDatPhongDTO> GetList()
        {
            return DsDatPhongDAL.Instance.GetAll();
        }

        // 🔥 Thêm
        public bool Add(DsDatPhongDTO dp)
        {
            return DsDatPhongDAL.Instance.Insert(dp);
        }

        // 🔥 Xoá
        public bool Delete(int id)
        {
            return DsDatPhongDAL.Instance.Delete(id);
        }
    }
}