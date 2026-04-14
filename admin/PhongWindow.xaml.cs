using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using QuanLyKhachSanWeb.BLL;
using QuanLyKhachSanWeb.DTO;

namespace QuanLyKhachSanWeb.admin
{
    public partial class PhongWindow : Window
    {
        private readonly PhongBLL _phongBll = new PhongBLL();

        public PhongWindow()
        {
            InitializeComponent();
            ContentRendered += PhongWindow_ContentRendered;
        }

        private void PhongWindow_ContentRendered(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(LoadData), DispatcherPriority.Background);
        }

        private void LoadData()
        {
            try
            {
                List<PhongDTO> danhSachPhong = _phongBll.LayDanhSachPhong();
                dgPhong.ItemsSource = null;
                dgPhong.ItemsSource = danhSachPhong;
            }
            catch (Exception ex)
            {
                dgPhong.ItemsSource = null;
                MessageBox.Show($"Không thể tải danh sách phòng.\n{ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnThemPhong_Click(object sender, RoutedEventArgs e)
        {
            var themPhongWindow = new ThemPhongWindow();
            bool? ketQua = themPhongWindow.ShowDialog();

            if (ketQua == true)
            {
                LoadData();
            }
        }

        private void BtnSua_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button) || !(button.Tag is PhongDTO phong))
            {
                return;
            }

            var suaPhongWindow = new SuaPhongWindow(phong);
            bool? ketQua = suaPhongWindow.ShowDialog();

            if (ketQua == true)
            {
                LoadData();
            }
        }

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button) || !(button.Tag is PhongDTO phong))
            {
                return;
            }

            MessageBoxResult xacNhan = MessageBox.Show(
                $"Bạn có chắc muốn xóa phòng {phong.SoPhong}?",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (xacNhan != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                bool thanhCong = _phongBll.XoaPhong(phong.MaPhong);
                if (thanhCong)
                {
                    MessageBox.Show("Xóa phòng thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy phòng để xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Xóa phòng thất bại.\n{ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
