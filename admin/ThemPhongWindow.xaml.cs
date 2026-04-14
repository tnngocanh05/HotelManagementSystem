using System;
using System.Windows;
using QuanLyKhachSanWeb.BLL;
using QuanLyKhachSanWeb.DTO;

namespace QuanLyKhachSanWeb.admin
{
    public partial class ThemPhongWindow : Window
    {
        private readonly PhongBLL _phongBll = new PhongBLL();

        public ThemPhongWindow()
        {
            InitializeComponent();
            Loaded += ThemPhongWindow_Loaded;
        }

        private void ThemPhongWindow_Loaded(object sender, RoutedEventArgs e)
        {
            cboTinhTrang.ItemsSource = new[]
            {
                "Trống",
                "Đang ở",
                "Dọn dẹp",
                "Bảo trì"
            };

            cboTinhTrang.SelectedIndex = 0;

            try
            {
                cboLoaiPhong.ItemsSource = _phongBll.LayDanhSachLoaiPhong();
                if (cboLoaiPhong.Items.Count > 0)
                {
                    cboLoaiPhong.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tải loại phòng.\n{ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void BtnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var phong = new PhongDTO
                {
                    SoPhong = txtSoPhong.Text.Trim(),
                    TinhTrang = cboTinhTrang.SelectedItem?.ToString(),
                    Tang = ParseTang(),
                    MaLoaiPhong = Convert.ToInt32(cboLoaiPhong.SelectedValue)
                };

                bool thanhCong = _phongBll.ThemPhong(phong);

                if (thanhCong)
                {
                    MessageBox.Show("Thêm phòng thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("Thêm phòng không thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể thêm phòng.\n{ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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
