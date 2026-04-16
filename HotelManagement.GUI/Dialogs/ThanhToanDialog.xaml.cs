using HotelManagement.BLL;
using System;
using System.Windows;

namespace HotelManagement.GUI.Dialogs
{
    public partial class ThanhToanDialog : Window
    {
        private HoaDonBLL hoaDonBLL = new HoaDonBLL();
        private NhanVienBLL nhanVienBLL = new NhanVienBLL();

        public int MaDatPhong { get; set; }

        private decimal tongTien = 0;

        public ThanhToanDialog()
        {
            InitializeComponent();
            Loaded += ThanhToanDialog_Loaded;
            btnDong.Click += BtnDong_Click;
            btnTinhTienThua.Click += BtnTinhTienThua_Click;
            btnXacNhan.Click += BtnXacNhan_Click;
            cbPhuongThuc.SelectionChanged += CbPhuongThuc_SelectionChanged;
        }

        private void ThanhToanDialog_Loaded(object sender, RoutedEventArgs e)
        {
            txtMaDatPhong.Text = MaDatPhong.ToString();

            decimal tienPhong = hoaDonBLL.TinhTienPhong(MaDatPhong);
            decimal tienDichVu = hoaDonBLL.TinhTienDichVu(MaDatPhong);
            tongTien = tienPhong + tienDichVu;

            txtTienPhong.Text = tienPhong.ToString("N0");
            txtTienDichVu.Text = tienDichVu.ToString("N0");
            txtTongTien.Text = tongTien.ToString("N0");

            cbPhuongThuc.Items.Clear();
            cbPhuongThuc.Items.Add("Tiền mặt");
            cbPhuongThuc.Items.Add("Chuyển khoản");
            cbPhuongThuc.SelectedIndex = 0;

            // Lồng thêm
            LoadNhanVien();
        }

        // Lồng thêm
        private void LoadNhanVien()
        {
            cbNhanVien.ItemsSource = nhanVienBLL.GetAllNhanVien();
            cbNhanVien.DisplayMemberPath = "HoTen";
            cbNhanVien.SelectedValuePath = "MaNhanVien";

            if (cbNhanVien.Items.Count > 0)
                cbNhanVien.SelectedIndex = 0;
        }

        private void CbPhuongThuc_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            bool laTienMat = cbPhuongThuc.SelectedItem != null &&
                             cbPhuongThuc.SelectedItem.ToString() == "Tiền mặt";

            lblTienKhachDua.Visibility = laTienMat ? Visibility.Visible : Visibility.Collapsed;
            txtTienKhachDua.Visibility = laTienMat ? Visibility.Visible : Visibility.Collapsed;
            btnTinhTienThua.Visibility = laTienMat ? Visibility.Visible : Visibility.Collapsed;
        }

        private void BtnTinhTienThua_Click(object sender, RoutedEventArgs e)
        {
            decimal tienKhachDua;
            if (!decimal.TryParse(txtTienKhachDua.Text, out tienKhachDua))
            {
                MessageBox.Show("Tiền khách đưa không hợp lệ.");
                return;
            }

            if (tienKhachDua < tongTien)
            {
                MessageBox.Show("Tiền khách đưa không đủ.");
                return;
            }

            decimal tienThua = tienKhachDua - tongTien;
            MessageBox.Show("Tiền trả lại khách: " + tienThua.ToString("N0") + " VNĐ");
        }

        private void BtnXacNhan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbPhuongThuc.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn phương thức thanh toán.");
                    return;
                }

                if (cbNhanVien.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn nhân viên thanh toán.");
                    return;
                }

                int maNhanVienLap = Convert.ToInt32(cbNhanVien.SelectedValue);
                string phuongThuc = cbPhuongThuc.SelectedItem.ToString();

                if (phuongThuc == "Tiền mặt")
                {
                    decimal? tienKhachDua = null;
                    decimal value;

                    if (!decimal.TryParse(txtTienKhachDua.Text, out value))
                    {
                        MessageBox.Show("Tiền khách đưa không hợp lệ.");
                        return;
                    }

                    tienKhachDua = value;

                    string message;
                    bool result = hoaDonBLL.ThanhToan(
                        MaDatPhong,
                        phuongThuc,
                        tienKhachDua,
                        maNhanVienLap,
                        out message);

                    MessageBox.Show(message);

                    if (result)
                    {
                        DialogResult = true;
                        Close();
                    }
                }
                else if (phuongThuc == "Chuyển khoản")
                {
                    ThanhToanChuyenKhoanDialog dlg = new ThanhToanChuyenKhoanDialog
                    {
                        MaDatPhong = MaDatPhong,
                        TongTien = tongTien,
                        MaNhanVienLap = maNhanVienLap
                    };

                    bool? result = dlg.ShowDialog();

                    if (result == true)
                    {
                        DialogResult = true;
                        Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void BtnDong_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}