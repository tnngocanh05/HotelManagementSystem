using System.Windows;
using System.Windows.Controls;
using HotelManagement.BLL;
using HotelManagement.DTO;

namespace HotelManagement.GUI.Views
{
    public partial class PhongView : UserControl
    {
        private PhongBLL bll = new PhongBLL();

        public PhongView()
        {
            InitializeComponent();
            LoadData();
        }

        public void LoadData() => dgPhong.ItemsSource = bll.GetAllRooms();

        private void BtnThem_Click(object sender, RoutedEventArgs e)
        {
            PhongDialog dlg = new PhongDialog();
            if (dlg.ShowDialog() == true) LoadData();
        }

        // --- SỬA THEO DÒNG ---
        private void BtnSua_Click(object sender, RoutedEventArgs e)
        {
            // Lấy đối tượng PhongDTO từ hàng chứa nút bấm
            Button btn = sender as Button;
            PhongDTO selected = btn.DataContext as PhongDTO;

            if (selected != null)
            {
                PhongDialog dlg = new PhongDialog(selected);
                if (dlg.ShowDialog() == true) LoadData();
            }
        }

        // --- XÓA THEO DÒNG ---
        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            PhongDTO selected = btn.DataContext as PhongDTO;

            if (selected != null)
            {
                var result = MessageBox.Show($"Xóa phòng {selected.SoPhong}?", "Xác nhận",
                                           MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    if (bll.Xoa(selected.MaPhong))
                    {
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa phòng đang có dữ liệu liên quan!");
                    }
                }
            }
        }
    }
}