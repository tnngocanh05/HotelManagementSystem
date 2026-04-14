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
    public partial class HoaDonView : UserControl
    {
        private HoaDonBLL hoaDonBLL = new HoaDonBLL();

        public HoaDonView()
        {
            InitializeComponent();

            Loaded += HoaDonView_Loaded;

            btnTimKiem.Click += BtnTimKiem_Click;
            btnLamMoi.Click += BtnLamMoi_Click;
            btnXemChiTiet.Click += BtnXemChiTiet_Click;
            btnXuatExcel.Click += BtnXuatExcel_Click;
            btnXuatPDF.Click += BtnXuatPDF_Click;
        }

        private void HoaDonView_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTrangThai();
            LoadDanhSachHoaDon();
        }

        // =========================
        // LOAD COMBOBOX TRẠNG THÁI
        // =========================
        private void LoadTrangThai()
        {
            cbTrangThai.Items.Clear();
            cbTrangThai.Items.Add("Tất cả");
            cbTrangThai.Items.Add("Chưa thanh toán");
            cbTrangThai.Items.Add("Đã thanh toán");
            cbTrangThai.SelectedIndex = 0;
        }

        // =========================
        // LOAD TOÀN BỘ HÓA ĐƠN
        // =========================
        private void LoadDanhSachHoaDon()
        {
            try
            {
                List<HoaDonDTO> ds = hoaDonBLL.GetAllHoaDon();
                dgHoaDon.ItemsSource = ds;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load danh sách hóa đơn: " + ex.Message,
                                "Thông báo lỗi",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        // =========================
        // TÌM KIẾM / LỌC
        // =========================
        private void BtnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string keyword = txtKeyword.Text.Trim();
                DateTime? tuNgay = dpTuNgay.SelectedDate;
                DateTime? denNgay = dpDenNgay.SelectedDate;
                string trangThai = cbTrangThai.SelectedItem == null
                    ? "Tất cả"
                    : cbTrangThai.SelectedItem.ToString();

                if (tuNgay.HasValue && denNgay.HasValue && tuNgay.Value.Date > denNgay.Value.Date)
                {
                    MessageBox.Show("Từ ngày không được lớn hơn đến ngày.",
                                    "Thông báo",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                    return;
                }

                List<HoaDonDTO> ds = hoaDonBLL.SearchHoaDon(keyword, tuNgay, denNgay, trangThai);
                dgHoaDon.ItemsSource = ds;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm hóa đơn: " + ex.Message,
                                "Thông báo lỗi",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        // =========================
        // LÀM MỚI
        // =========================
        private void BtnLamMoi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtKeyword.Clear();
                dpTuNgay.SelectedDate = null;
                dpDenNgay.SelectedDate = null;
                cbTrangThai.SelectedIndex = 0;

                LoadDanhSachHoaDon();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi làm mới dữ liệu: " + ex.Message,
                                "Thông báo lỗi",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        // =========================
        // XEM CHI TIẾT
        // =========================
        private void BtnXemChiTiet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HoaDonDTO selected = dgHoaDon.SelectedItem as HoaDonDTO;

                if (selected == null)
                {
                    MessageBox.Show("Vui lòng chọn một hóa đơn để xem chi tiết.",
                                    "Thông báo",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                    return;
                }

                ChiTietHoaDonDialog dlg = new ChiTietHoaDonDialog();
                dlg.MaHoaDon = selected.MaHoaDon;
                dlg.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xem chi tiết hóa đơn: " + ex.Message,
                                "Thông báo lỗi",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        // =========================
        // XUẤT EXCEL
        // =========================
        private void BtnXuatExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<HoaDonDTO> ds = dgHoaDon.ItemsSource as List<HoaDonDTO>;

                if (ds == null || ds.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất Excel.",
                                    "Thông báo",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Workbook|*.xlsx",
                    FileName = "DanhSachHoaDon.xlsx"
                };

                if (saveFileDialog.ShowDialog() != true)
                    return;

                using (XLWorkbook wb = new XLWorkbook())
                {
                    var ws = wb.Worksheets.Add("HoaDon");

                    // Tiêu đề lớn
                    ws.Cell(1, 1).Value = "DANH SÁCH HÓA ĐƠN";
                    ws.Range(1, 1, 1, 12).Merge();
                    ws.Cell(1, 1).Style.Font.Bold = true;
                    ws.Cell(1, 1).Style.Font.FontSize = 16;
                    ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // Thông tin lọc
                    ws.Cell(2, 1).Value = "Từ ngày:";
                    ws.Cell(2, 2).Value = dpTuNgay.SelectedDate.HasValue
                        ? dpTuNgay.SelectedDate.Value.ToString("dd/MM/yyyy")
                        : "";

                    ws.Cell(2, 3).Value = "Đến ngày:";
                    ws.Cell(2, 4).Value = dpDenNgay.SelectedDate.HasValue
                        ? dpDenNgay.SelectedDate.Value.ToString("dd/MM/yyyy")
                        : "";

                    ws.Cell(2, 5).Value = "Trạng thái:";
                    ws.Cell(2, 6).Value = cbTrangThai.SelectedItem == null ? "" : cbTrangThai.SelectedItem.ToString();

                    ws.Cell(2, 7).Value = "Từ khóa:";
                    ws.Cell(2, 8).Value = txtKeyword.Text.Trim();

                    // Header bảng
                    int row = 4;
                    ws.Cell(row, 1).Value = "STT";
                    ws.Cell(row, 2).Value = "Mã hóa đơn";
                    ws.Cell(row, 3).Value = "Mã phòng";
                    ws.Cell(row, 4).Value = "Khách hàng";
                    ws.Cell(row, 5).Value = "CCCD";
                    ws.Cell(row, 6).Value = "SĐT";
                    ws.Cell(row, 7).Value = "Loại phòng";
                    ws.Cell(row, 8).Value = "Số đêm";
                    ws.Cell(row, 9).Value = "Ngày lập";
                    ws.Cell(row, 10).Value = "Ngày thanh toán";
                    ws.Cell(row, 11).Value = "Trạng thái";
                    ws.Cell(row, 12).Value = "Tổng tiền";

                    var headerRange = ws.Range(row, 1, row, 12);
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
                    headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                    // Dữ liệu
                    int stt = 1;
                    row++;

                    foreach (var hd in ds)
                    {
                        ws.Cell(row, 1).Value = stt++;
                        ws.Cell(row, 2).Value = hd.SoHoaDon;
                        ws.Cell(row, 3).Value = hd.SoPhong;
                        ws.Cell(row, 4).Value = hd.HoTenKhachHang;
                        ws.Cell(row, 5).Value = hd.CCCD;
                        ws.Cell(row, 6).Value = hd.SDT;
                        ws.Cell(row, 7).Value = hd.TenLoaiPhong;
                        ws.Cell(row, 8).Value = hd.SoDem;
                        ws.Cell(row, 9).Value = hd.NgayLap.ToString("dd/MM/yyyy HH:mm");
                        ws.Cell(row, 10).Value = hd.NgayThanhToan.HasValue
                            ? hd.NgayThanhToan.Value.ToString("dd/MM/yyyy HH:mm")
                            : "";
                        ws.Cell(row, 11).Value = hd.TrangThai;
                        ws.Cell(row, 12).Value = hd.TongTien;

                        ws.Row(row).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        ws.Row(row).Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                        row++;
                    }

                    // Format cột tiền
                    ws.Column(12).Style.NumberFormat.Format = "#,##0";

                    // Auto fit
                    ws.Columns().AdjustToContents();

                    // Lưu file
                    wb.SaveAs(saveFileDialog.FileName);
                }

                MessageBox.Show("Xuất Excel thành công!",
                                "Thông báo",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất Excel: " + ex.Message,
                                "Thông báo lỗi",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        // =========================
        // XUẤT PDF
        // =========================
        private void BtnXuatPDF_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HoaDonDTO selected = dgHoaDon.SelectedItem as HoaDonDTO;

                if (selected == null)
                {
                    MessageBox.Show("Vui lòng chọn một hóa đơn để xuất PDF.",
                                    "Thông báo",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                    return;
                }

                ChiTietHoaDonDialog dlg = new ChiTietHoaDonDialog();
                dlg.MaHoaDon = selected.MaHoaDon;
                dlg.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thao tác PDF hóa đơn: " + ex.Message,
                                "Thông báo lỗi",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }
    }
}