using System;
using System.Collections.Generic;
using HotelManagement.DAL;
using HotelManagement.DTO;

namespace HotelManagement.BLL
{
public class NhanVienBLL
{
    private static NhanVienBLL instance;
    public static NhanVienBLL Instance
    {
        get
        {
            if (instance == null)
                instance = new NhanVienBLL();
            return instance;
        }
    }

    public List<NhanVienDTO> GetListNhanVien()
    {
        return NhanVienDAL.Instance.GetAll();
    }

    public bool ThemNhanVien(NhanVienDTO nv)
    {
        return NhanVienDAL.Instance.Insert(nv);
    }

    public bool UpdateNhanVien(NhanVienDTO nv)
    {
        return NhanVienDAL.Instance.Update(nv);
    }

    public bool DeleteNhanVien(int maNV)
    {
        return NhanVienDAL.Instance.Delete(maNV);
    }

    public DataTable GetChucVu()
    {
        return NhanVienDAL.Instance.GetChucVu();
    }
}