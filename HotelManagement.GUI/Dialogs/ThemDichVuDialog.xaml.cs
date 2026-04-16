using HotelManagement.BLL;
using HotelManagement.DTO;
using System;
using System.Linq;
using System.Windows;

namespace HotelManagement.GUI.Dialogs
{
    public partial class ThemDichVuDialog : Window
    {
        private DichVuBLL dichVuBLL = new DichVuBLL();
        private ChiTietDichVuBLL chiTietDichVuBLL = new ChiTietDichVuBLL();

        // BỔ SUNG: Biến để kiểm tra xem đang ở chế độ Sửa hay Thêm
        private DichVuDTO _selectedDichVu;

        public int MaDatPhong { get; set; }

        public ThemDichVuDialog()
        {
            InitializeComponent();
            Loaded += ThemDichVuDialog_Loaded;
            btnDong.Click += BtnDong_Click;
            btnLuu.Click += BtnLuu_Click;
            cbDichVu.SelectionChanged += CbDichVu_SelectionChanged;
        }

        // BỔ SUNG: Constructor mới nhận 1 đối số để sửa lỗi Build
        public ThemDichVuDialog(DichVuDTO selected) : this()
        {
            _selectedDichVu = selected;

            // Hiển thị thông tin lên giao diện nếu cần sửa
            if (_selectedDichVu != null)
            {
                this.Title = "Chỉnh sửa thông tin dịch vụ";
                // Đoạn này tùy thuộc vào các x:Name bạn đặt trong XAML
                // Ví dụ: cbDichVu.SelectedItem = _selectedDichVu;
            }
        }

        private void ThemDichVuDialog_Loaded(object sender, RoutedEventArgs e)
        {
            txtMaDatPhong.Text = MaDatPhong.ToString();
            cbDichVu.ItemsSource = dichVuBLL.GetAllDichVu();

            // BỔ SUNG: Nếu là chế độ sửa, chọn sẵn dịch vụ trong ComboBox
            if (_selectedDichVu != null)
            {
                cbDichVu.Text = _selectedDichVu.TenDichVu;
            }
        }

        private void CbDichVu_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            DichVuDTO dv = cbDichVu.SelectedItem as DichVuDTO;
            if (dv != null)
            {
                txtDonGia.Text = dv.DonGia.ToString("N0");
            }
        }

        private void BtnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbDichVu.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn dịch vụ!");
                    return;
                }

                int soLuong;
                if (!int.TryParse(txtSoLuong.Text, out soLuong) || soLuong <= 0)
                {
                    MessageBox.Show("Số lượng phải lớn hơn 0!");
                    return;
                }

                DichVuDTO dv = cbDichVu.SelectedItem as DichVuDTO;

                // KIỂM TRA: Nếu _selectedDichVu == null thì mới thực hiện Insert vào phòng
                // Còn nếu không null, bạn có thể bổ sung logic Update danh mục dịch vụ tại đây
                if (_selectedDichVu == null)
                {
                    ChiTietDichVuDTO dto = new ChiTietDichVuDTO
                    {
                        MaDatPhong = MaDatPhong,
                        MaDichVu = dv.MaDichVu,
                        SoLuong = soLuong,
                        ThanhTien = dv.DonGia * soLuong
                    };

                    bool result = chiTietDichVuBLL.InsertChiTietDichVu(dto);

                    if (result)
                    {
                        MessageBox.Show("Thêm dịch vụ thành công!");
                        this.DialogResult = true;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Thêm dịch vụ thất bại!");
                    }
                }
                else
                {
                    // Logic dành cho nút Sửa (nếu bạn muốn cập nhật trực tiếp từ dialog này)
                    _selectedDichVu.TenDichVu = dv.TenDichVu; // Ví dụ
                    if (dichVuBLL.Sua(_selectedDichVu))
                    {
                        MessageBox.Show("Cập nhật danh mục dịch vụ thành công!");
                        this.DialogResult = true;
                        this.Close();
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
            this.Close();
        }
    }
}