using System.Windows;
using System.Windows.Controls;
using HotelManagement.BLL;
using HotelManagement.DTO; // Đảm bảo có dòng này để dùng LoaiPhongDTO
using HotelManagement.GUI.Dialogs;

namespace HotelManagement.GUI.Views
{
    public partial class LoaiPhongView : UserControl
    {
        LoaiPhongBLL bll = new LoaiPhongBLL();

        public LoaiPhongView()
        {
            InitializeComponent();
            LoadData();
        }

        void LoadData()
        {
            dgv.ItemsSource = bll.LayTatCa();
        }

        private void BtnThem_Click(object sender, RoutedEventArgs e)
        {
            LoaiPhongDialog dlg = new LoaiPhongDialog();
            if (dlg.ShowDialog() == true)
            {
                LoadData(); // Load lại dữ liệu sau khi thêm thành công
            }
        }

        // ================= XỬ LÝ SỬA =================
        private void BtnSua_Click(object sender, RoutedEventArgs e)
        {
            // Lấy dòng đang được chọn từ nút bấm
            var button = sender as Button;
            var selectedLoaiPhong = button?.DataContext as LoaiPhongDTO;

            if (selectedLoaiPhong != null)
            {
                LoaiPhongDialog dlg = new LoaiPhongDialog();
                // Truyền dữ liệu sang Dialog để hiển thị lên các ô nhập
                dlg.SelectedLoaiPhong = selectedLoaiPhong;

                if (dlg.ShowDialog() == true)
                {
                    LoadData(); // Cập nhật lại bảng
                }
            }
        }

        // ================= XỬ LÝ XÓA =================
        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var item = button?.DataContext as LoaiPhongDTO;

            if (item != null)
            {
                // Gọi hàm bll.Xoa và truyền vào Mã (int)
                // Kết quả trả về là bool nên dùng được trong lệnh if
                if (bll.Xoa(item.MaLoaiPhong))
                {
                    MessageBox.Show("Xóa thành công!");
                    LoadData();
                }
            }
        }
    }
}