using HotelManagement.DTO;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HotelManagement.GUI.Dialogs
{
    public partial class ThemNhanVienDialog : Window
    {
        private NhanVienDTO nv; // dùng cho sửa

        // ================= CONSTRUCTOR =================

        // 👉 THÊM
        public ThemNhanVienDialog()
        {
            InitializeComponent();
            LoadChucVu();
        }

        // 👉 SỬA
        public ThemNhanVienDialog(NhanVienDTO nv)
        {
            InitializeComponent();
            this.nv = nv;

            LoadChucVu();
            LoadData();
        }

        // ================= LOAD =================

        void LoadChucVu()
        {
            // ⚠ nếu chưa có ChucVuBUS thì tạm hardcode
            cbChucVu.Items.Clear();
            cbChucVu.Items.Add("1");
            cbChucVu.Items.Add("2");
            cbChucVu.Items.Add("3");
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
                cbChucVu.SelectedItem = nv.MaChucVu.ToString();
        }

        // ================= LƯU =================

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NhanVienDTO nvMoi = new NhanVienDTO();

                nvMoi.HoTen = txtHoTen.Text;
                nvMoi.NgaySinh = dpNgaySinh.SelectedDate ?? DateTime.Now;

                nvMoi.GioiTinh = (cbGioiTinh.SelectedItem as ComboBoxItem)
                                    ?.Content.ToString();

                nvMoi.SDT = txtSDT.Text;
                nvMoi.CCCD = txtCCCD.Text;
                nvMoi.Email = txtEmail.Text;

                nvMoi.MaChucVu = int.Parse(cbChucVu.SelectedItem.ToString());

                bool kq;

                // 👉 THÊM
                if (nv == null)
                {
                    kq = NhanVienBLL.Instance.ThemNhanVien(nvMoi);
                }
                // 👉 SỬA
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

        // ================= HỦY =================

        private void btnHuy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}