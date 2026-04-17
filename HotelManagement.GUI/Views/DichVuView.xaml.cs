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
    /// Interaction logic for DichVuView.xaml
    /// </summary>
    public partial class DichVuView : UserControl
    {
        public DichVuView()
        {
            InitializeComponent();
            LoadData();
        }

        // ===== LOAD DATA =====
        void LoadData()
        {
            icDichVu.ItemsSource = DichVuBLL.Instance.GetListDichVu();
        }

        // ===== THÊM =====
        private void BtnThemDichVu_Click(object sender, RoutedEventArgs e)
        {
            ThemDichVuDialog f = new ThemDichVuDialog();
            f.ShowDialog();

            LoadData(); // 🔥 bắt buộc
        }

        // ===== SỬA =====
        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            DichVuDTO dv = btn.DataContext as DichVuDTO;

            ThemDichVuDialog f = new ThemDichVuDialog(dv);
            f.ShowDialog();

            LoadData();
        }

        // ===== XÓA =====
        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            DichVuDTO dv = btn.DataContext as DichVuDTO;

            DichVuBLL.Instance.DeleteDichVu(dv.MaDichVu);

            LoadData();
        }
    }
}
