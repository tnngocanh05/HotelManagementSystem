using HotelManagement.GUI.Pages;
using System.Windows;
using HotelManagement.GUI.Views;

namespace HotelManagement.GUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            btnTrangChu.Click += BtnTrangChu_Click;
            btnHoaDon.Click += BtnHoaDon_Click;
            btnPhong.Click += BtnPhong_Click;
            btnKhachHang.Click += BtnKhachHang_Click;
            btnNhanVien.Click += BtnNhanVien_Click;
            btnThongKe.Click += BtnThongKe_Click;
            btnLoaiPhong.Click += BtnLoaiPhong_Click;
            btnDichVu.Click += BtnDichVu_Click;
            btnDanhSachDatPhong.Click += BtnDanhSachDatPhong_Click;

            MainFrame.Navigate(new TrangChuPage());
        }

        private void BtnTrangChu_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new TrangChuPage());
        }

        private void BtnHoaDon_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new HoaDonPage());
        }

        private void BtnPhong_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new admin.PhongWindow());
        }

        private void BtnKhachHang_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PlaceholderPage("Khách Hàng"));
        }

        private void BtnNhanVien_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PlaceholderPage("Nhân Viên"));
        }

        private void BtnThongKe_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PlaceholderPage("Thống Kê"));
        }

        private void BtnLoaiPhong_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PlaceholderPage("Loại Phòng"));
        }

        private void BtnDichVu_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PlaceholderPage("Dịch Vụ"));
        }

        private void BtnDanhSachDatPhong_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PlaceholderPage("Danh Sách Đặt Phòng"));
        }
    }
}
