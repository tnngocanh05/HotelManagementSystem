using System;
using System.Windows;
using HotelManagement.BLL;
using HotelManagement.DTO;

namespace HotelManagement.GUI.Views
{
    public partial class ChiTietDichVuDialog : Window
    {
        private int _maDatPhong;
        private ChiTietDichVuBLL bll = new ChiTietDichVuBLL();
        private DichVuBLL dvBll = new DichVuBLL(); // Giả sử bạn đã có lớp quản lý danh mục dịch vụ

        public ChiTietDichVuDialog(int maDP)
        {
            InitializeComponent();
            _maDatPhong = maDP;
            LoadDanhSachDichVu();
        }

        private void LoadDanhSachDichVu()
        {
            try
            {
                // Load các dịch vụ sẵn có trong khách sạn (Coca, Mì tôm, Giặt là...)
                cbDichVu.ItemsSource = dvBll.LayTatCa();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dịch vụ: " + ex.Message);
            }
        }

        private void BtnXacNhan_Click(object sender, RoutedEventArgs e)
        {
            // 1. Kiểm tra lựa chọn
            if (cbDichVu.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn một dịch vụ!");
                return;
            }

            // 2. Kiểm tra số lượng
            if (!int.TryParse(txtSoLuong.Text, out int soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng phải là số nguyên dương!");
                return;
            }

            try
            {
                // 3. Tạo DTO và lưu
                ChiTietDichVuDTO ct = new ChiTietDichVuDTO
                {
                    MaDatPhong = _maDatPhong,
                    MaDichVu = (int)cbDichVu.SelectedValue,
                    SoLuong = soLuong
                };

                if (bll.InsertChiTietDichVu(ct))
                {
                    MessageBox.Show("Đã thêm dịch vụ thành công!");
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Không thể thêm dịch vụ. Vui lòng kiểm tra lại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void BtnHuy_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}