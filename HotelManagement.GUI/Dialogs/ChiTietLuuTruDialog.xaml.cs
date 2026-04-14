using System.Windows;
using HotelManagement.BLL;
using HotelManagement.DTO;

namespace HotelManagement.GUI.Dialogs
{
    public partial class ChiTietLuuTruDialog : Window
    {
        private DatPhongBLL datPhongBLL = new DatPhongBLL();
        private ChiTietDichVuBLL chiTietDichVuBLL = new ChiTietDichVuBLL();

        public string SoPhong { get; set; }

        private int maDatPhongHienTai = 0;

        public ChiTietLuuTruDialog()
        {
            InitializeComponent();

            btnDong.Click += BtnDong_Click;
            btnThemDichVu.Click += BtnThemDichVu_Click;
            btnThanhToan.Click += BtnThanhToan_Click;

            Loaded += ChiTietLuuTruDialog_Loaded;
        }

        // Load khi mở popup
        private void ChiTietLuuTruDialog_Loaded(object sender, RoutedEventArgs e)
        {
            LoadThongTin();
        }

        // Load toàn bộ thông tin lưu trú + dịch vụ
        private void LoadThongTin()
        {
            if (string.IsNullOrWhiteSpace(SoPhong))
                return;

            ChiTietLuuTruDTO data = datPhongBLL.GetChiTietLuuTruBySoPhong(SoPhong);

            if (data == null)
            {
                MessageBox.Show("Không tìm thấy thông tin lưu trú của phòng này.",
                                "Thông báo",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                this.Close();
                return;
            }

            maDatPhongHienTai = data.MaDatPhong;

            txtMaDatPhong.Text = data.MaDatPhong.ToString();
            txtSoPhong.Text = data.SoPhong;
            txtKhachHang.Text = data.HoTen;
            txtCCCD.Text = data.CCCD;
            txtSDT.Text = data.SDT;
            txtLoaiPhong.Text = data.TenLoaiPhong;
            txtThoiGianNhan.Text = data.ThoiGianNhan.ToString("dd/MM/yyyy HH:mm");
            txtNgayTraDuKien.Text = data.NgayTraDuKien.ToString("dd/MM/yyyy");

            dgDichVu.ItemsSource = chiTietDichVuBLL.GetByMaDatPhong(maDatPhongHienTai);
        }

        // Nút thêm dịch vụ
        private void BtnThemDichVu_Click(object sender, RoutedEventArgs e)
        {
            if (maDatPhongHienTai <= 0)
                return;

            ThemDichVuDialog dlg = new ThemDichVuDialog();
            dlg.MaDatPhong = maDatPhongHienTai;

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                dgDichVu.ItemsSource = chiTietDichVuBLL.GetByMaDatPhong(maDatPhongHienTai);
            }
        }

        // Nút thanh toán
        private void BtnThanhToan_Click(object sender, RoutedEventArgs e)
        {
            if (maDatPhongHienTai <= 0)
                return;

            ThanhToanDialog dlg = new ThanhToanDialog();
            dlg.MaDatPhong = maDatPhongHienTai;

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                MessageBox.Show("Đã thanh toán xong. Phòng sẽ chuyển sang trạng thái dọn dẹp.",
                                "Thông báo",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);

                this.DialogResult = true;
                this.Close();
            }
        }

        // Nút đóng
        private void BtnDong_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}