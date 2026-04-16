using HotelManagement.BLL;
using System;
using System.Windows;

namespace HotelManagement.GUI.Dialogs
{
    public partial class ThanhToanChuyenKhoanDialog : Window
    {
        private HoaDonBLL hoaDonBLL = new HoaDonBLL();

        public int MaDatPhong { get; set; }
        public decimal TongTien { get; set; }

        // Lồng thêm
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
            txtTrangThai.Text = "Chưa thanh toán";
        }

        private void BtnGiaLapThanhCong_Click(object sender, RoutedEventArgs e)
        {
            daThanhToanThanhCong = true;
            txtTrangThai.Text = "Đã thanh toán";
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