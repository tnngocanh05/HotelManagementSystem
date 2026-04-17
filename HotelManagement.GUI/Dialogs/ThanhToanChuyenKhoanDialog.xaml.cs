using HotelManagement.BLL;
using QRCoder;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace HotelManagement.GUI.Dialogs
{
    public partial class ThanhToanChuyenKhoanDialog : Window
    {
        private HoaDonBLL hoaDonBLL = new HoaDonBLL();

        public int MaDatPhong { get; set; }
        public decimal TongTien { get; set; }

        // Giữ phần mày đã thêm
        public int MaNhanVienLap { get; set; }

        private bool daThanhToanThanhCong = false;

        public ThanhToanChuyenKhoanDialog()
        {
            InitializeComponent();
            Loaded += ThanhToanChuyenKhoanDialog_Loaded;
            btnGiaLapThanhCong.Click += BtnGiaLapThanhCong_Click;
            btnHoanTat.Click += BtnHoanTat_Click;
            btnDong.Click += BtnDong_Click;
        }

        private void ThanhToanChuyenKhoanDialog_Loaded(object sender, RoutedEventArgs e)
        {
            txtMaDatPhong.Text = MaDatPhong.ToString();
            txtSoTien.Text = TongTien.ToString("N0") + " VNĐ";
            txtMaGiaoDich.Text = "VNPAY" + DateTime.Now.ToString("yyyyMMddHHmmss");
            txtTrangThai.Text = "Trạng thái: Chờ thanh toán";

            // Lồng lại phần tạo QR động
            TaoMaQR();
        }

        // Lồng thêm lại phần QR động
        private void TaoMaQR()
        {
            try
            {
                string duLieuQR =
                    "VNPAY DEMO" + Environment.NewLine +
                    "Ma giao dich: " + txtMaGiaoDich.Text + Environment.NewLine +
                    "Ma dat phong: " + MaDatPhong + Environment.NewLine +
                    "So tien: " + TongTien.ToString("N0") + " VND";

                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(duLieuQR, QRCodeGenerator.ECCLevel.Q);
                PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);

                byte[] qrBytes = qrCode.GetGraphic(20);

                using (MemoryStream ms = new MemoryStream(qrBytes))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = ms;
                    bitmap.EndInit();
                    bitmap.Freeze();

                    imgQR.Source = bitmap;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tạo mã QR: " + ex.Message,
                                "Thông báo lỗi",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        private void BtnGiaLapThanhCong_Click(object sender, RoutedEventArgs e)
        {
            daThanhToanThanhCong = true;
            txtTrangThai.Text = "Trạng thái: Đã thanh toán";
            btnHoanTat.IsEnabled = true;

            MessageBox.Show("Giả lập quét QR thành công.");
        }

        private void BtnHoanTat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!daThanhToanThanhCong)
                {
                    MessageBox.Show("Giao dịch chưa được xác nhận thành công.");
                    return;
                }

                string message;
                bool result = hoaDonBLL.ThanhToan(
                    MaDatPhong,
                    "Chuyển khoản",
                    null,
                    MaNhanVienLap,
                    out message);

                MessageBox.Show(message);

                if (result)
                {
                    DialogResult = true;
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void BtnDong_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}