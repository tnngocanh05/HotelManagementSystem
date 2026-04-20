using HotelManagement.BLL;
using HotelManagement.DTO;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace HotelManagement.GUI.admin
{
    public partial class PhongWindow : Page
    {
        private readonly PhongBLL phongBll = new PhongBLL();

        public PhongWindow()
        {
            InitializeComponent();
            Loaded += PhongWindow_Loaded;
        }

        private void PhongWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<PhongDTO> danhSachPhong = phongBll.GetAllRooms();
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
            ThemPhongWindow themPhongWindow = new ThemPhongWindow
            {
                Owner = Window.GetWindow(this)
            };

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

            SuaPhongWindow suaPhongWindow = new SuaPhongWindow(phong)
            {
                Owner = Window.GetWindow(this)
            };

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
                bool thanhCong = phongBll.DeleteRoom(phong.MaPhong);
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
