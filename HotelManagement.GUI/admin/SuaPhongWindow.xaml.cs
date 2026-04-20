using HotelManagement.BLL;
using HotelManagement.DTO;
using System;
using System.Windows;

namespace HotelManagement.GUI.admin
{
    public partial class SuaPhongWindow : Window
    {
        private readonly PhongBLL phongBll = new PhongBLL();
        private readonly PhongDTO phongHienTai;

        public SuaPhongWindow(PhongDTO phong)
        {
            phongHienTai = phong ?? throw new ArgumentNullException(nameof(phong));

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
                cboLoaiPhong.ItemsSource = phongBll.GetAllRoomTypes();
                txtSoPhong.Text = phongHienTai.SoPhong;
                txtTang.Text = phongHienTai.Tang.ToString();
                cboTinhTrang.SelectedItem = phongHienTai.TinhTrang;
                cboLoaiPhong.SelectedValue = phongHienTai.MaLoaiPhong;
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
                PhongDTO phong = new PhongDTO
                {
                    MaPhong = phongHienTai.MaPhong,
                    SoPhong = txtSoPhong.Text.Trim(),
                    TinhTrang = cboTinhTrang.SelectedItem?.ToString(),
                    Tang = ParseTang(),
                    MaLoaiPhong = Convert.ToInt32(cboLoaiPhong.SelectedValue)
                };

                bool thanhCong = phongBll.UpdateRoom(phong);
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
