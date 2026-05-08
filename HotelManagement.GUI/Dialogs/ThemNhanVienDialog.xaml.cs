using HotelManagement.DTO;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace HotelManagement.GUI.Dialogs
{
    public partial class ThemNhanVienDialog : Window
    {
        private NhanVienDTO nv;

        public ThemNhanVienDialog()
        {
            InitializeComponent();
            LoadChucVu();
        }

        public ThemNhanVienDialog(NhanVienDTO nv)
        {
            InitializeComponent();
            this.nv = nv;

            LoadChucVu();
            LoadData();
        }

        void LoadChucVu()
        {
            cbChucVu.Items.Clear();

            DataTable dt = NhanVienBLL.Instance.GetChucVu();

            foreach (DataRow row in dt.Rows)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = row["TenChucVu"].ToString();
                item.Tag = row["MaChucVu"];

                cbChucVu.Items.Add(item);
            }

            cbChucVu.SelectedIndex = 0;
        }

        void LoadData()
        {
            if (nv == null) return;

            txtHoTen.Text = nv.HoTen;
            dpNgaySinh.SelectedDate = nv.NgaySinh;

            if (nv.GioiTinh == "Nam")
                cbGioiTinh.SelectedIndex = 0;
            else
                cbGioiTinh.SelectedIndex = 1;

            txtSDT.Text = nv.SDT;
            txtCCCD.Text = nv.CCCD;
            txtEmail.Text = nv.Email;

            if (nv.MaChucVu != null)
            {
                foreach (ComboBoxItem item in cbChucVu.Items)
                {
                    if (item.Tag.ToString() == nv.MaChucVu.ToString())
                    {
                        cbChucVu.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NhanVienDTO nvMoi = new NhanVienDTO();

                nvMoi.HoTen = txtHoTen.Text;
                nvMoi.NgaySinh = dpNgaySinh.SelectedDate ?? DateTime.Now;
                nvMoi.GioiTinh = (cbGioiTinh.SelectedItem as ComboBoxItem)?.Content.ToString();
                nvMoi.SDT = txtSDT.Text;
                nvMoi.CCCD = txtCCCD.Text;
                nvMoi.Email = txtEmail.Text;

                ComboBoxItem item = cbChucVu.SelectedItem as ComboBoxItem;
                nvMoi.MaChucVu = Convert.ToInt32(item.Tag);

                bool kq;

                if (nv == null)
                {
                    kq = NhanVienBLL.Instance.ThemNhanVien(nvMoi);
                }
                else
                {
                    nvMoi.MaNV = nv.MaNV;
                    kq = NhanVienBLL.Instance.UpdateNhanVien(nvMoi);
                }

                MessageBox.Show(kq ? "Thành công" : "Thất bại");

                if (kq) this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnHuy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}