using HotelManagement.BLL;
using HotelManagement.DTO;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace HotelManagement.GUI.Dialogs
{
    public partial class ChiTietHoaDonDialog : Window
    {
        private HoaDonBLL hoaDonBLL = new HoaDonBLL();

        public int MaHoaDon { get; set; }

        private ChiTietHoaDonDTO chiTietHoaDon;

        public ChiTietHoaDonDialog()
        {
            InitializeComponent();

            Loaded += ChiTietHoaDonDialog_Loaded;
            btnDong.Click += BtnDong_Click;
            btnXuatPDF.Click += BtnXuatPDF_Click;
            btnInHoaDon.Click += BtnInHoaDon_Click;
        }

        private void ChiTietHoaDonDialog_Loaded(object sender, RoutedEventArgs e)
        {
            LoadChiTietHoaDon();
        }

        private void LoadChiTietHoaDon()
        {
            chiTietHoaDon = hoaDonBLL.GetChiTietHoaDonByMaHoaDon(MaHoaDon);

            if (chiTietHoaDon == null)
            {
                MessageBox.Show("Không tìm thấy chi tiết hóa đơn.",
                                "Thông báo",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                Close();
                return;
            }

            txtSoHoaDon.Text = chiTietHoaDon.SoHoaDon ?? "";
            txtNgayLap.Text = chiTietHoaDon.NgayLapHoaDon.ToString("dd/MM/yyyy HH:mm");
            txtKhachHang.Text = chiTietHoaDon.HoTenKhachHang ?? "";
            txtSoPhong.Text = chiTietHoaDon.SoPhong ?? "";
            txtLoaiPhong.Text = chiTietHoaDon.TenLoaiPhong ?? "";
            txtSoDem.Text = chiTietHoaDon.SoDem.ToString();
            txtTrangThai.Text = chiTietHoaDon.TrangThai ?? "";
            txtNgayThanhToan.Text = chiTietHoaDon.NgayThanhToan.HasValue
                ? chiTietHoaDon.NgayThanhToan.Value.ToString("dd/MM/yyyy HH:mm")
                : "";
            txtNhanVienLap.Text = chiTietHoaDon.TenNhanVienLap ?? "";
            txtPhuongThuc.Text = chiTietHoaDon.PhuongThucThanhToan ?? "";

            txtTienPhong.Text = chiTietHoaDon.TienPhong.ToString("N0");
            txtTienDichVu.Text = chiTietHoaDon.TienDichVu.ToString("N0");
            txtTongTien.Text = chiTietHoaDon.TongTien.ToString("N0");

            dgDichVuHoaDon.ItemsSource = chiTietHoaDon.DanhSachDichVu;
        }

        private void BtnXuatPDF_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (chiTietHoaDon == null)
                {
                    MessageBox.Show("Không có dữ liệu hóa đơn để xuất PDF.",
                                    "Thông báo",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PDF file|*.pdf";
                saveFileDialog.FileName = (chiTietHoaDon.SoHoaDon ?? "HoaDon") + ".pdf";

                if (saveFileDialog.ShowDialog() == true)
                {
                    ExportPdf(saveFileDialog.FileName);

                    MessageBox.Show("Xuất PDF thành công!",
                                    "Thông báo",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất PDF: " + ex.Message,
                                "Thông báo lỗi",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        private void BtnInHoaDon_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (chiTietHoaDon == null)
                {
                    MessageBox.Show("Không có dữ liệu hóa đơn để in.",
                                    "Thông báo",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PDF file|*.pdf";
                saveFileDialog.FileName = (chiTietHoaDon.SoHoaDon ?? "HoaDon") + "_Print.pdf";

                if (saveFileDialog.ShowDialog() == true)
                {
                    ExportPdf(saveFileDialog.FileName);

                    Process.Start(new ProcessStartInfo(saveFileDialog.FileName)
                    {
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi in hóa đơn: " + ex.Message,
                                "Thông báo lỗi",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        private void ExportPdf(string filePath)
        {
            Document document = new Document(PageSize.A4, 36, 36, 36, 36);
            PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
            document.Open();

            string fontPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
                "arial.ttf"
            );

            BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            iTextSharp.text.Font titleFont = new iTextSharp.text.Font(baseFont, 18, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font normalFont = new iTextSharp.text.Font(baseFont, 11, iTextSharp.text.Font.NORMAL);
            iTextSharp.text.Font boldFont = new iTextSharp.text.Font(baseFont, 11, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font italicFont = new iTextSharp.text.Font(baseFont, 11, iTextSharp.text.Font.ITALIC);

            Paragraph hotelName = new Paragraph("KHÁCH SẠN QUẢN LÝ KHÁCH SẠN", boldFont);
            hotelName.Alignment = Element.ALIGN_CENTER;
            document.Add(hotelName);

            Paragraph title = new Paragraph("HÓA ĐƠN THANH TOÁN", titleFont);
            title.Alignment = Element.ALIGN_CENTER;
            title.SpacingAfter = 15;
            document.Add(title);

            document.Add(new Paragraph("Số hóa đơn: " + (chiTietHoaDon.SoHoaDon ?? ""), normalFont));
            document.Add(new Paragraph("Ngày lập hóa đơn: " + chiTietHoaDon.NgayLapHoaDon.ToString("dd/MM/yyyy HH:mm"), normalFont));
            document.Add(new Paragraph("Ngày thanh toán: " +
                (chiTietHoaDon.NgayThanhToan.HasValue
                    ? chiTietHoaDon.NgayThanhToan.Value.ToString("dd/MM/yyyy HH:mm")
                    : ""), normalFont));
            document.Add(new Paragraph("Nhân viên thanh toán: " + (chiTietHoaDon.TenNhanVienLap ?? ""), normalFont));
            document.Add(new Paragraph(" ", normalFont));

            document.Add(new Paragraph("Tên khách hàng: " + (chiTietHoaDon.HoTenKhachHang ?? ""), normalFont));
            document.Add(new Paragraph("Mã phòng: " + (chiTietHoaDon.SoPhong ?? ""), normalFont));
            document.Add(new Paragraph("Loại phòng: " + (chiTietHoaDon.TenLoaiPhong ?? ""), normalFont));
            document.Add(new Paragraph("Số đêm ở: " + chiTietHoaDon.SoDem, normalFont));
            document.Add(new Paragraph("Phương thức thanh toán: " + (chiTietHoaDon.PhuongThucThanhToan ?? ""), normalFont));
            document.Add(new Paragraph(" ", normalFont));

            Paragraph serviceTitle = new Paragraph("BẢNG DỊCH VỤ", boldFont);
            serviceTitle.SpacingAfter = 10;
            document.Add(serviceTitle);

            PdfPTable table = new PdfPTable(5);
            table.WidthPercentage = 100;
            table.SetWidths(new float[] { 3f, 2f, 1.5f, 1.5f, 2f });

            AddCell(table, "Tên dịch vụ", boldFont);
            AddCell(table, "Loại DV", boldFont);
            AddCell(table, "Số lượng", boldFont);
            AddCell(table, "Đơn giá", boldFont);
            AddCell(table, "Thành tiền", boldFont);

            if (chiTietHoaDon.DanhSachDichVu != null && chiTietHoaDon.DanhSachDichVu.Count > 0)
            {
                foreach (ChiTietDichVuDTO dv in chiTietHoaDon.DanhSachDichVu)
                {
                    AddCell(table, dv.TenDichVu ?? "", normalFont);
                    AddCell(table, dv.SoLuong.ToString(), normalFont);
                    AddCell(table, dv.DonGia.ToString("N0"), normalFont);
                    AddCell(table, dv.ThanhTien.ToString("N0"), normalFont);
                }
            }
            else
            {
                PdfPCell noDataCell = new PdfPCell(new Phrase("Không có dịch vụ", normalFont));
                noDataCell.Colspan = 5;
                noDataCell.HorizontalAlignment = Element.ALIGN_CENTER;
                noDataCell.Padding = 8;
                table.AddCell(noDataCell);
            }

            document.Add(table);
            document.Add(new Paragraph(" ", normalFont));

            document.Add(new Paragraph("Tiền phòng: " + chiTietHoaDon.TienPhong.ToString("N0") + " VNĐ", boldFont));
            document.Add(new Paragraph("Tiền dịch vụ: " + chiTietHoaDon.TienDichVu.ToString("N0") + " VNĐ", boldFont));
            document.Add(new Paragraph("TỔNG TIỀN: " + chiTietHoaDon.TongTien.ToString("N0") + " VNĐ", boldFont));
            document.Add(new Paragraph(" ", normalFont));

            Paragraph thanks = new Paragraph("Xin cảm ơn quý khách đã sử dụng dịch vụ của chúng tôi!", italicFont);
            thanks.Alignment = Element.ALIGN_CENTER;
            thanks.SpacingBefore = 20;
            document.Add(thanks);

            document.Close();
        }

        private void AddCell(PdfPTable table, string text, iTextSharp.text.Font font)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Padding = 6;
            table.AddCell(cell);
        }

        private void BtnDong_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}