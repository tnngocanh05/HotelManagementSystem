using System;
using System.Windows;
using System.Windows.Controls;
using HotelManagement.BLL;
using HotelManagement.DTO;

namespace HotelManagement.GUI.Views
{
    public partial class NhanVienDialog : Window
    {
        private NhanVienBLL bll = new NhanVienBLL();
        private NhanVienDTO _selected = null;

        public NhanVienDialog(NhanVienDTO nv = null)
        {
            InitializeComponent();
            _selected = nv;

            if (_selected != null)
            {
                this.Title = "Cập nhật nhân viên";
                txtHoTen.Text = _selected.HoTen;
                dpNgaySinh.SelectedDate = _selected.NgaySinh;
                txtSDT.Text = _selected.SDT;
                txtCCCD.Text = _selected.CCCD;
                txtEmail.Text = _selected.Email;
                txtMaChucVu.Text = _selected.MaChucVu.ToString();

                // Chọn giới tính trong ComboBox
                foreach (ComboBoxItem item in cbGioiTinh.Items)
                {
                    if (item.Content.ToString() == _selected.GioiTinh)
                    {
                        cbGioiTinh.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void BtnLuu_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra rỗng
            if (string.IsNullOrWhiteSpace(txtHoTen.Text) || dpNgaySinh.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng nhập Họ tên và chọn Ngày sinh!");
                return;
            }

            // Gán dữ liệu
            NhanVienDTO nv = _selected ?? new NhanVienDTO();
            nv.HoTen = txtHoTen.Text;
            nv.NgaySinh = dpNgaySinh.SelectedDate.Value;
            nv.GioiTinh = (cbGioiTinh.SelectedItem as ComboBoxItem).Content.ToString();
            nv.SDT = txtSDT.Text;
            nv.CCCD = txtCCCD.Text;
            nv.Email = txtEmail.Text;

            if (int.TryParse(txtMaChucVu.Text, out int maCV))
                nv.MaChucVu = maCV;
            else
                nv.MaChucVu = 1; // Giá trị mặc định nếu nhập sai

            // Gọi BLL
            bool success = (_selected == null) ? bll.Them(nv) : bll.Sua(nv);

            if (success)
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Thao tác thất bại!");
            }
        }

        private void BtnHuy_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}