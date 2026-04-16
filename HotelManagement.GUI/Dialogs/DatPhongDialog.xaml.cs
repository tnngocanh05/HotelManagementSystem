using HotelManagement.BLL;
using HotelManagement.DTO;
using System;
using System.Windows;

namespace HotelManagement.GUI.Dialogs
{
    public partial class DatPhongDialog : Window
    {
        private DatPhongBLL datPhongBLL = new DatPhongBLL();

        public DatPhongDialog()
        {
            InitializeComponent();
            LoadDefaultData();
            btnDong.Click += BtnDong_Click;
            btnDatPhong.Click += BtnDatPhong_Click;
        }
        // --- BỔ SUNG: Constructor nhận 1 tham số PhongDTO để sửa lỗi ---
        public DatPhongDialog(PhongDTO phong) : this() // Gọi lại constructor mặc định ở trên
        {
            if (phong != null)
            {
                txtMaPhong.Text = phong.SoPhong; // Hiển thị Số phòng cho người dùng dễ nhìn
                txtLoaiPhong.Text = phong.TenLoaiPhong;

                // Lưu tag hoặc biến ẩn nếu bạn cần dùng MaPhong (ID) để lưu DB
                txtMaPhong.Tag = phong.MaPhong;
            }
        }

        public string MaPhong
        {
            get { return txtMaPhong.Text; }
            set { txtMaPhong.Text = value; }
        }

        public string LoaiPhong
        {
            get { return txtLoaiPhong.Text; }
            set { txtLoaiPhong.Text = value; }
        }

        private void LoadDefaultData()
        {
            dpNgayNhan.SelectedDate = DateTime.Today;
            dpNgayTra.SelectedDate = DateTime.Today.AddDays(1);

            cbGioNhan.Items.Add("08:00");
            cbGioNhan.Items.Add("10:00");
            cbGioNhan.Items.Add("12:00");
            cbGioNhan.Items.Add("14:00");
            cbGioNhan.Items.Add("16:00");
            cbGioNhan.Items.Add("18:00");

            cbGioNhan.SelectedIndex = 3;
        }

        private void BtnDong_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnDatPhong_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtHoTen.Text) ||
                    string.IsNullOrWhiteSpace(txtCCCD.Text) ||
                    string.IsNullOrWhiteSpace(txtSDT.Text) ||
                    string.IsNullOrWhiteSpace(txtSoNguoi.Text) ||
                    dpNgayNhan.SelectedDate == null ||
                    dpNgayTra.SelectedDate == null ||
                    cbGioNhan.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin đặt phòng!",
                                    "Thông báo",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                    return;
                }

                int soNguoi;
                if (!int.TryParse(txtSoNguoi.Text, out soNguoi))
                {
                    MessageBox.Show("Số người phải là số nguyên!",
                                    "Thông báo",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                    return;
                }

                bool result = datPhongBLL.DatPhongTrucTiep(
                    txtMaPhong.Text,
                    txtHoTen.Text,
                    txtCCCD.Text,
                    txtSDT.Text,
                    soNguoi,
                    dpNgayNhan.SelectedDate.Value,
                    cbGioNhan.SelectedItem.ToString(),
                    dpNgayTra.SelectedDate.Value
                );

                if (result)
                {
                    MessageBox.Show("Đặt phòng thành công!",
                                    "Thông báo",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);

                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Đặt phòng thất bại!",
                                    "Thông báo",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message,
                                "Thông báo lỗi",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }
    }
}