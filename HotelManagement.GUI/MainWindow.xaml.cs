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

            MainContent.Content = new TrangChuView();
        }

        private void BtnTrangChu_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new TrangChuView();
        }

        private void BtnHoaDon_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new HoaDonView();
        }
    }
}