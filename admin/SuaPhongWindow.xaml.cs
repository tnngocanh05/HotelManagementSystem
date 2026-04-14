using System;
using System.Windows;
using QuanLyKhachSanWeb.BLL;
using QuanLyKhachSanWeb.DTO;

namespace QuanLyKhachSanWeb.admin
{
    public partial class SuaPhongWindow : Window
    {
        private readonly PhongBLL _phongBll = new PhongBLL();
        private readonly PhongDTO _phongHienTai;

        public SuaPhongWindow(PhongDTO phong)
        {
            _phongHienTai = phong ?? throw new ArgumentNullException(nameof(phong));

            InitializeComponent();
            Loaded += SuaPhongWindow_Loaded;
        }

        private void SuaPhongWindow_Loaded(object sender, RoutedEventArgs e)
        {
            cboTinhTrang.ItemsSource = new[]
            {
                "Trống",
                "Đang ở",
                "Dọn dẹp",
                "Bảo trì"
            };

            try
            {
                cboLoaiPhong.ItemsSource = _phongBll.LayDanhSachLoaiPhong();
                txtSoPhong.Text = _phongHienTai.SoPhong;
                txtTang.Text = _phongHienTai.Tang.ToString();
                cboTinhTrang.SelectedItem = _phongHienTai.TinhTrang;
                cboLoaiPhong.SelectedValue = _phongHienTai.MaLoaiPhong;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tải thông tin phòng.\n{ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void BtnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var phong = new PhongDTO
                {
                    MaPhong = _phongHienTai.MaPhong,
                    SoPhong = txtSoPhong.Text.Trim(),
                    TinhTrang = cboTinhTrang.SelectedItem?.ToString(),
                    Tang = ParseTang(),
                    MaLoaiPhong = Convert.ToInt32(cboLoaiPhong.SelectedValue)
                };

                bool thanhCong = _phongBll.SuaPhong(phong);

                if (thanhCong)
                {
                    MessageBox.Show("Cập nhật phòng thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy phòng để cập nhật.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể cập nhật phòng.\n{ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int ParseTang()
        {
            if (!int.TryParse(txtTang.Text.Trim(), out int tang))
            {
                throw new ArgumentException("Tầng phải là số nguyên.");
            }

            return tang;
        }

        private void BtnHuy_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
