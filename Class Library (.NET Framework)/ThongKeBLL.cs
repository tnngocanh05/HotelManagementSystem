using HotelManagement.DAL;
using System;
using System.Collections.Generic;

namespace HotelManagement.BLL
{
    public class ThongKeBLL
    {
        private ThongKeDAL dal = new ThongKeDAL();

        public decimal LayDoanhThuTong() => dal.LayTongDoanhThu();

        public Dictionary<string, decimal> LayDoanhThuTheoKhoang(DateTime tu, DateTime den)
            => dal.LayDoanhThuTheoKhoang(tu, den);

        public Dictionary<string, int> LayTiLePhong() => dal.LayTinhTrangPhong();

        public Dictionary<string, int> LayThongKeDichVu() => dal.LayThongKeDichVu();
    }
}