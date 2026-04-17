using HotelManagement.BLL;
using HotelManagement.DTO;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HotelManagement.GUI.Dialogs
{
    /// <summary>
    /// Interaction logic for ThemKhachHangDialog.xaml
    /// </summary>
    public partial class ThemKhachHangDialog : Window
    {
        private KhachHangDTO kh;

        // ===== THÊM =====
        public ThemKhachHangDialog()
        {
            InitializeComponent();
        }

        // ===== SỬA =====
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
            // THÊM
            if (kh == null)
            {
                KhachHangDTO newKH = new KhachHangDTO
                {
                    HoTen = txtHoTen.Text,
                    CCCD = txtCCCD.Text,
                    SDT = txtSDT.Text
                };

                KhachHangBLL.Instance.ThemKhachHang(newKH);
            }
            // SỬA
            else
            {
                kh.HoTen = txtHoTen.Text;
                kh.CCCD = txtCCCD.Text;
                kh.SDT = txtSDT.Text;

                KhachHangBLL.Instance.UpdateKhachHang(kh);
            }

            this.Close();
        }

        private void btnHuy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
