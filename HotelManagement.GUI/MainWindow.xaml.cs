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
            btnLoaiPhong.Click += BtnLoaiPhong_Click;
            btnPhong.Click += BtnPhong_Click;
            btnDichVu.Click += BtnDichVu_Click;
            btnThongKe.Click += BtnThongKe_Click;
            MainContent.Content = new TrangChuView();
        }

        private void BtnTrangChu_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new TrangChuView();
        }
        private void BtnLoaiPhong_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new LoaiPhongView();
        }
        private void BtnPhong_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new PhongView();
        }
        private void BtnDichVu_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new DichVuView();
        }


        private void BtnHoaDon_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new HoaDonView();
        }
        private void BtnThongKe_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ThongKeView();
        }
    }
}