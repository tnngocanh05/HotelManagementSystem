using HotelManagement.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Class_Library__.NET_Framework_; // 🔥 THÊM DÒNG NÀY

namespace HotelManagement.GUI.Views
{
    public partial class DsDatPhong : UserControl
    {
        List<DsDatPhongDTO> listAll; // 🔥 giữ toàn bộ data

        public DsDatPhong()
        {
            InitializeComponent();
            LoadData();
        }

        // ================= LOAD DATA =================

        public void LoadData()
        {
            // 🔥 SỬA CHỖ NÀY: lấy từ BLL thay vì hard-code
            listAll = DsDatPhongBLL.Instance.GetList();

            dgDatPhong.ItemsSource = listAll;
        }

        // ================= EVENT =================

        private void BtnHuy_Click(object sender, RoutedEventArgs e)
        {
            var dp = dgDatPhong.SelectedItem as DsDatPhongDTO;

            if (dp == null)
            {
                MessageBox.Show("Chọn dòng cần hủy!");
                return;
            }

            MessageBox.Show("Đã hủy đặt phòng!");
        }

        private void BtnXacNhan_Click(object sender, RoutedEventArgs e)
        {
            var dp = dgDatPhong.SelectedItem as DsDatPhongDTO;

            if (dp == null)
            {
                MessageBox.Show("Chọn dòng!");
                return;
            }

            MessageBox.Show("Xác nhận thành công!");
        }

        // ================= FILTER =================

        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearch.Text.ToLower();

            var trangThai = (cbTrangThai.SelectedItem as ComboBoxItem)?.Content.ToString();

            var fromDate = dpFrom.SelectedDate;
            var toDate = dpTo.SelectedDate;

            var result = listAll.Where(x =>
            {
                bool matchSearch =
                    string.IsNullOrWhiteSpace(keyword) ||
                    keyword == "tên, sdt, số phòng..." ||
                    x.MaDatPhong.ToString().Contains(keyword) ||
                    x.MaKhachHang.ToString().Contains(keyword) ||
                    x.MaPhong.ToString().Contains(keyword);

                bool matchTrangThai =
                    trangThai == "Tất cả" ||
                    string.IsNullOrEmpty(trangThai) ||
                    x.TrangThai == trangThai;

                bool matchDate = true;

                if (fromDate != null)
                    matchDate &= x.ThoiGianNhan.Date >= fromDate.Value.Date;

                if (toDate != null)
                    matchDate &= x.NgayTraDuKien.Date <= toDate.Value.Date;

                return matchSearch && matchTrangThai && matchDate;
            }).ToList();

            dgDatPhong.ItemsSource = result;
        }

        // ================= UX SEARCH =================

        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text == "Tên, SDT, số phòng...")
            {
                txtSearch.Text = "";
                txtSearch.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Tên, SDT, số phòng...";
                txtSearch.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }
    }
}