
using HotelManagement.DTO;
using HotelManagement.GUI.Dialogs; // 🔥 sửa namespace dialog
using System.Windows;
using System.Windows.Controls;

namespace HotelManagement.GUI.Views
{
    public partial class NhanVien : UserControl
    {
        public NhanVien()
        {
            InitializeComponent();
            LoadNhanVien();
        }

        // ================= LOAD =================

        private void LoadNhanVien()
        {
            dgNhanVien.ItemsSource = NhanVienBLL.Instance.GetListNhanVien();
        }

        // ================= THÊM =================

        private void btnThemNhanVien_Click(object sender, RoutedEventArgs e)
        {
            ThemNhanVienDialog f = new ThemNhanVienDialog();
            f.ShowDialog();
            LoadNhanVien();
        }

        // ================= SỬA =================

        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            var nv = (sender as FrameworkElement)?.DataContext as NhanVienDTO;
            if (nv == null) return;

            ThemNhanVienDialog f = new ThemNhanVienDialog(nv);
            f.ShowDialog();
            LoadNhanVien();
        }

        // ================= XÓA =================

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            var nv = (sender as FrameworkElement)?.DataContext as NhanVienDTO;
            if (nv == null) return;

            var result = MessageBox.Show(
                "Bạn có chắc muốn xóa?",
                "Xác nhận",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                bool kq = NhanVienBLL.Instance.DeleteNhanVien(nv.MaNV);

                if (kq)
                {
                    MessageBox.Show("Xóa thành công");
                    LoadNhanVien();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại");
                }
            }
        }
    }
}