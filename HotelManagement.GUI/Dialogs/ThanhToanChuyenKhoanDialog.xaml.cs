using HotelManagement.BLL;
using QRCoder;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HotelManagement.GUI.Dialogs
{
    public partial class ThanhToanChuyenKhoanDialog : Window
    {
        private HoaDonBLL hoaDonBLL = new HoaDonBLL();

        public int MaDatPhong { get; set; }
        public decimal TongTien { get; set; }

        private string maGiaoDich = "";
        private bool daThanhToanThanhCong = false;

        public ThanhToanChuyenKhoanDialog()
        {
            InitializeComponent();

            Loaded += ThanhToanChuyenKhoanDialog_Loaded;
            btnDong.Click += BtnDong_Click;
            btnGiaLapThanhCong.Click += BtnGiaLapThanhCong_Click;
            btnHoanTat.Click += BtnHoanTat_Click;
        }

        private void ThanhToanChuyenKhoanDialog_Loaded(object sender, RoutedEventArgs e)
        {
            maGiaoDich = "VNPAY" + DateTime.Now.ToString("yyyyMMddHHmmss");

            txtMaGiaoDich.Text = maGiaoDich;
            txtMaDatPhong.Text = MaDatPhong.ToString();
            txtSoTien.Text = TongTien.ToString("N0") + " VNĐ";

            string qrContent = $"VNPAY|{maGiaoDich}|{MaDatPhong}|{TongTien}";
            imgQR.Source = GenerateQRCode(qrContent);
        }

        private BitmapImage GenerateQRCode(string text)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);

            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrBytes = qrCode.GetGraphic(20);

            using (MemoryStream ms = new MemoryStream(qrBytes))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = ms;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();
                return bitmap;
            }
        }

        private void BtnGiaLapThanhCong_Click(object sender, RoutedEventArgs e)
        {
            daThanhToanThanhCong = true;

            txtTrangThai.Text = "Trạng thái: Đã thanh toán";
            txtTrangThai.Foreground = Brushes.Green;

            btnHoanTat.IsEnabled = true;

            MessageBox.Show("Giả lập giao dịch VNPAY thành công!",
                            "Thông báo",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }

        private void BtnHoanTat_Click(object sender, RoutedEventArgs e)
        {
            if (!daThanhToanThanhCong)
            {
                MessageBox.Show("Giao dịch chưa được xác nhận thành công.",
                                "Thông báo",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            string message;
            bool result = hoaDonBLL.ThanhToan(MaDatPhong, "Chuyển khoản", null, out message);

            MessageBox.Show(message);

            if (result)
            {
                this.DialogResult = true;
                this.Close();
            }
        }

        private void BtnDong_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}