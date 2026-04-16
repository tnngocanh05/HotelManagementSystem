using System;
using System.Windows;
using System.Windows.Controls;
using HotelManagement.BLL;
using HotelManagement.DTO;

namespace HotelManagement.GUI.Views
{
    public partial class PhongDialog : Window
    {
        private PhongBLL phongBll = new PhongBLL();
        private LoaiPhongBLL loaiPhongBll = new LoaiPhongBLL();
        private PhongDTO _selectedPhong = null;

        // Constructor dùng cho cả Thêm (không truyền p) và Sửa (truyền p)
        public PhongDialog(PhongDTO p = null)
        {
            InitializeComponent();
            LoadLoaiPhong();

            _selectedPhong = p;

            if (_selectedPhong != null)
            {
                this.Title = "Cập nhật thông tin phòng";
                txtSoPhong.Text = _selectedPhong.SoPhong;
                txtTang.Text = _selectedPhong.Tang.ToString();
                cbLoaiPhong.SelectedValue = _selectedPhong.MaLoaiPhong;

                // Chọn đúng Item trong ComboBox Tình trạng
                foreach (ComboBoxItem item in cbTinhTrang.Items)
                {
                    if (item.Content.ToString() == _selectedPhong.TinhTrang)
                    {
                        cbTinhTrang.SelectedItem = item;
                        break;
                    }
                }
            }
            else
            {
                this.Title = "Thêm phòng mới";
                cbTinhTrang.SelectedIndex = 0; // Mặc định chọn mục đầu tiên
            }
        }

        private void LoadLoaiPhong()
        {
            cbLoaiPhong.ItemsSource = loaiPhongBll.LayTatCa();
        }

        private void BtnLuu_Click(object sender, RoutedEventArgs e)
        {
            // 1. Kiểm tra dữ liệu đầu vào cơ bản
            if (string.IsNullOrWhiteSpace(txtSoPhong.Text) || cbLoaiPhong.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Số phòng và chọn Loại phòng!");
                return;
            }

            // 2. Kiểm tra định dạng số cho Tầng
            if (!int.TryParse(txtTang.Text, out int tang))
            {
                MessageBox.Show("Số tầng phải là chữ số!");
                return;
            }

            // 3. Gán dữ liệu vào DTO
            // Nếu _selectedPhong null thì tạo mới (Thêm), nếu không thì dùng lại (Sửa)
            PhongDTO p = _selectedPhong ?? new PhongDTO();
            p.SoPhong = txtSoPhong.Text;
            p.Tang = tang;
            p.MaLoaiPhong = (int)cbLoaiPhong.SelectedValue;
            p.TinhTrang = (cbTinhTrang.SelectedItem as ComboBoxItem).Content.ToString();

            // 4. Gọi BLL thực hiện lưu
            try
            {
                bool result;
                if (_selectedPhong == null)
                {
                    result = phongBll.Them(p);
                }
                else
                {
                    result = phongBll.Sua(p);
                }

                if (result)
                {
                    MessageBox.Show("Lưu dữ liệu thành công!");
                    this.DialogResult = true; // Đóng dialog và báo về View để LoadData
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Lưu dữ liệu thất bại. Vui lòng kiểm tra lại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message);
            }
        }

        private void BtnHuy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}