using System;
using System.Windows;
using HotelManagement.DTO;
using HotelManagement.BLL;

namespace HotelManagement.GUI.Dialogs
{
    public partial class LoaiPhongDialog : Window
    {
        private LoaiPhongBLL bll = new LoaiPhongBLL();

        // Property này nhận dữ liệu từ LoaiPhongView truyền sang khi nhấn "Sửa"
        public LoaiPhongDTO SelectedLoaiPhong { get; set; }

        public LoaiPhongDialog()
        {
            InitializeComponent();
            this.Loaded += LoaiPhongDialog_Loaded;
        }

        private void LoaiPhongDialog_Loaded(object sender, RoutedEventArgs e)
        {
            // Nếu có dữ liệu truyền sang => Chế độ Sửa
            if (SelectedLoaiPhong != null)
            {
                this.Title = "Cập nhật loại phòng";
                txtTenLoaiPhong.Text = SelectedLoaiPhong.TenLoaiPhong;
                txtSoNguoiToiDa.Text = SelectedLoaiPhong.SoNguoiToiDa.ToString();
                txtGiaTien.Text = SelectedLoaiPhong.GiaTien.ToString();
            }
            else
            {
                this.Title = "Thêm loại phòng mới";
            }
        }

        private void BtnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1. Lấy dữ liệu từ giao diện
                string ten = txtTenLoaiPhong.Text.Trim();
                int soNguoi = int.Parse(txtSoNguoiToiDa.Text.Trim());
                decimal gia = decimal.Parse(txtGiaTien.Text.Trim());

                if (string.IsNullOrEmpty(ten))
                {
                    MessageBox.Show("Vui lòng nhập tên loại phòng!");
                    return;
                }

                if (SelectedLoaiPhong == null)
                {
                    // --- CHẾ ĐỘ THÊM MỚI ---
                    LoaiPhongDTO moi = new LoaiPhongDTO
                    {
                        TenLoaiPhong = ten,
                        SoNguoiToiDa = soNguoi,
                        GiaTien = gia
                    };

                    // Bạn cần đảm bảo đã viết hàm bll.Them(moi)
                    if (bll.Them(moi))
                    {
                        MessageBox.Show("Thêm mới thành công!");
                        this.DialogResult = true;
                    }
                }
                else
                {
                    // --- CHẾ ĐỘ SỬA ---
                    // Cập nhật thông tin mới vào đối tượng đang có
                    SelectedLoaiPhong.TenLoaiPhong = ten;
                    SelectedLoaiPhong.SoNguoiToiDa = soNguoi;
                    SelectedLoaiPhong.GiaTien = gia;

                    // Gọi xuống BLL để thực thi lệnh UPDATE trong SQL
                    if (bll.Sua(SelectedLoaiPhong))
                    {
                        MessageBox.Show("Cập nhật thành công!");
                        this.DialogResult = true; // Trả về true để màn hình danh sách LoadData() lại
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật thất bại!");
                    }
                }
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Vui lòng kiểm tra lại định dạng số người và giá tiền!");
            }
        }

        private void BtnHuy_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}