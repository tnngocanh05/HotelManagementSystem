using HotelManagement.BLL;
using HotelManagement.DTO;
using System.Windows;

namespace HotelManagement.GUI.Dialogs
{
    public partial class ThemKhachHangDialog : Window
    {
        private KhachHangDTO kh;
        private KhachHangBLL khachHangBLL = new KhachHangBLL();

        // THÊM
        public ThemKhachHangDialog()
        {
            InitializeComponent();
        }

        // SỬA
        public ThemKhachHangDialog(KhachHangDTO khach)
        {
            InitializeComponent();

            kh = khach;

            txtHoTen.Text = kh.HoTen;
            txtCCCD.Text = kh.CCCD;
            txtSDT.Text = kh.SDT;
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            if (kh == null)
            {
                KhachHangDTO newKH = new KhachHangDTO
                {
                    HoTen = txtHoTen.Text,
                    CCCD = txtCCCD.Text,
                    SDT = txtSDT.Text
                };

                khachHangBLL.ThemKhachHang(newKH);
            }
            else
            {
                kh.HoTen = txtHoTen.Text;
                kh.CCCD = txtCCCD.Text;
                kh.SDT = txtSDT.Text;

                khachHangBLL.UpdateKhachHang(kh);
            }

            this.Close();
        }

        private void btnHuy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}