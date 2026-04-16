using System;
using System.Windows;
using System.Windows.Controls;
using HotelManagement.BLL;
using HotelManagement.DTO;
using HotelManagement.GUI.Dialogs; // Đảm bảo có namespace này để gọi DichVuDialog

namespace HotelManagement.GUI.Views
{
    public partial class DichVuView : UserControl
    {
        private DichVuBLL bll = new DichVuBLL();

        public DichVuView()
        {
            InitializeComponent();
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                // Sau khi thêm GetAllDichVu() vào BLL, dòng này sẽ chạy bình thường
                var data = bll.GetAllDichVu();
                dgDichVu.ItemsSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải danh sách dịch vụ: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnThem_Click(object sender, RoutedEventArgs e)
        {
            // Đảm bảo tên Class Dialog của bạn là DichVuDialog
            ThemDichVuDialog dlg = new ThemDichVuDialog();
            if (dlg.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void BtnSua_Click(object sender, RoutedEventArgs e)
        {
            // Lấy object từ DataContext của dòng được nhấn nút
            if ((sender as Button).DataContext is DichVuDTO selected)
            {
                ThemDichVuDialog dlg = new ThemDichVuDialog(selected);
                if (dlg.ShowDialog() == true)
                {
                    LoadData();
                }
            }
        }

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).DataContext is DichVuDTO selected)
            {
                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa dịch vụ: {selected.TenDichVu}?\nLưu ý: Hành động này không thể hoàn tác.",
                                           "Xác nhận xóa",
                                           MessageBoxButton.YesNo,
                                           MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        if (bll.Xoa(selected.MaDichVu))
                        {
                            MessageBox.Show("Xóa dịch vụ thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show("Xóa thất bại. Dịch vụ có thể đang được sử dụng trong các hóa đơn.", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Stop);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi hệ thống khi xóa: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}