using HotelManagement.DAL;
using HotelManagement.DTO;
using System.Collections.Generic;

namespace HotelManagement.BLL
{
    public class DichVuBLL
    {
        private DichVuDAL dal = new DichVuDAL();

        public List<DichVuDTO> GetAllDichVu()
        {
            return dal.GetAllDichVu();
        }
    }
}