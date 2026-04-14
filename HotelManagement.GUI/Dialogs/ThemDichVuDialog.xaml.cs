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

        public int MaDatPhong { get; set; }

        public ThemDichVuDialog()
        {
            InitializeComponent();
            Loaded += ThemDichVuDialog_Loaded;
            btnDong.Click += BtnDong_Click;
            btnLuu.Click += BtnLuu_Click;
            cbDichVu.SelectionChanged += CbDichVu_SelectionChanged;
        }

        private void ThemDichVuDialog_Loaded(object sender, RoutedEventArgs e)
        {
            txtMaDatPhong.Text = MaDatPhong.ToString();
            cbDichVu.ItemsSource = dichVuBLL.GetAllDichVu();
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