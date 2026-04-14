using System.Windows;
using QuanLyKhachSanWeb.admin;

namespace QuanLyKhachSanWeb
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnDatPhong_Click(object sender, RoutedEventArgs e)
        {
            new DatPhongWindow().ShowDialog();
        }

        private void BtnKhachHang_Click(object sender, RoutedEventArgs e)
        {
            new KhachHangWindow().ShowDialog();
        }

        private void BtnNhanVien_Click(object sender, RoutedEventArgs e)
        {
            new NhanVienWindow().ShowDialog();
        }

        private void BtnPhong_Click(object sender, RoutedEventArgs e)
        {
            new PhongWindow().ShowDialog();
        }

        private void BtnHoaDon_Click(object sender, RoutedEventArgs e)
        {
            new HoaDonWindow().ShowDialog();
        }

        private void BtnThongKe_Click(object sender, RoutedEventArgs e)
        {
            new ThongKeWindow().ShowDialog();
        }

        private void BtnDangXuat_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }
    }
}
