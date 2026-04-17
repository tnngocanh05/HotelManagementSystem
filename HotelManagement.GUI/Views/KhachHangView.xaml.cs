using ClosedXML.Excel;
using HotelManagement.BLL;
using HotelManagement.DTO;
using HotelManagement.GUI.Dialogs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace HotelManagement.GUI.Views
{
    /// <summary>
    /// Interaction logic for KhachHangView.xaml
    /// </summary>
    public partial class KhachHangView : UserControl
    {
        public KhachHangView()
        {
            InitializeComponent();
            LoadData();
        }

        // ===== LOAD DATA =====
        void LoadData()
        {
            dgKhachHang.ItemsSource = KhachHangBLL.Instance.GetListKhachHang();
        }

        // ===== THÊM =====
        private void btnThemKhachHang_Click(object sender, RoutedEventArgs e)
        {
            ThemKhachHangDialog f = new ThemKhachHangDialog();
            f.ShowDialog();

            LoadData(); // 🔥 cập nhật lại bảng
        }

        // ===== SỬA =====
        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            KhachHangDTO kh = btn.DataContext as KhachHangDTO;

            if (kh == null) return;

            ThemKhachHangDialog f = new ThemKhachHangDialog(kh);
            f.ShowDialog();

            LoadData();
        }

        // ===== XÓA =====
        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            KhachHangDTO kh = btn.DataContext as KhachHangDTO;

            if (kh == null) return;

            var result = MessageBox.Show("Bạn có chắc muốn xóa khách hàng này?",
                                        "Xác nhận",
                                        MessageBoxButton.YesNo,
                                        MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                KhachHangBLL.Instance.DeleteKhachHang(kh.MaKhachHang);
                LoadData();
            }
        }
    }
}
