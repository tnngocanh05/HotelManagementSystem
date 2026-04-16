using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using HotelManagement.BLL;
using System.ComponentModel;
using Microsoft.Win32;
// Namespace cho PDF
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Font;
using iText.IO.Font.Constants;

namespace HotelManagement.GUI.Views
{
    public partial class ThongKeView : UserControl, INotifyPropertyChanged
    {
        private ThongKeBLL bll = new ThongKeBLL();

        private SeriesCollection _revenueSeries;
        public SeriesCollection RevenueSeries { get => _revenueSeries; set { _revenueSeries = value; OnPropertyChanged("RevenueSeries"); } }

        private List<string> _labels;
        public List<string> Labels { get => _labels; set { _labels = value; OnPropertyChanged("Labels"); } }

        public SeriesCollection StatusSeries { get; set; }
        public SeriesCollection ServiceSeries { get; set; }

        public Func<double, string> YFormatter { get; set; }

        public ThongKeView()
        {
            InitializeComponent();

            // Cập nhật Formatter: Chia cho 1 triệu và thêm hậu tố 'tr' cho trục đứng
            YFormatter = value => (value / 1000000).ToString("N0") + "tr";

            DataContext = this;

            dpTuNgay.SelectedDate = DateTime.Now.AddDays(-10);
            dpDenNgay.SelectedDate = DateTime.Now;

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                txtDoanhThu.Text = bll.LayDoanhThuTong().ToString("N0") + " đ";

                DateTime tuNgay = dpTuNgay.SelectedDate ?? DateTime.Now.AddDays(-10);
                DateTime denNgay = dpDenNgay.SelectedDate ?? DateTime.Now;
                var doanhThuData = bll.LayDoanhThuTheoKhoang(tuNgay, denNgay);

                RevenueSeries = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "Doanh thu",
                        Values = new ChartValues<decimal>(doanhThuData.Values),
                        Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4776E6")),
                        DataLabels = true,
                        LabelPoint = point => point.Y.ToString("N0") // Hiển thị số đầy đủ trên đầu cột
                    }
                };
                Labels = doanhThuData.Keys.ToList();

                var roomData = bll.LayTiLePhong();
                StatusSeries = new SeriesCollection();
                foreach (var item in roomData)
                {
                    Brush color = Brushes.Gray;
                    if (item.Key == "Trống") color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#BEE9FF"));
                    else if (item.Key == "Đang ở") color = (Brush)new BrushConverter().ConvertFromString("#2ecc71");
                    else if (item.Key == "Dọn dẹp") color = Brushes.Gold;

                    StatusSeries.Add(new PieSeries { Title = item.Key, Values = new ChartValues<int> { item.Value }, Fill = color, DataLabels = true });
                }

                var serviceData = bll.LayThongKeDichVu();
                ServiceSeries = new SeriesCollection();
                foreach (var sv in serviceData)
                {
                    ServiceSeries.Add(new PieSeries { Title = sv.Key, Values = new ChartValues<int> { sv.Value }, DataLabels = true });
                }

                txtSoPhong.Text = roomData.Values.Sum().ToString();
                txtDichVu.Text = serviceData.Values.Sum().ToString();

                OnPropertyChanged("StatusSeries");
                OnPropertyChanged("ServiceSeries");
            }
            catch (Exception ex)
            {
                // Tránh crash ứng dụng khi dữ liệu null hoặc lỗi kết nối
                Console.WriteLine("Lỗi LoadData: " + ex.Message);
            }
        }

        private void BtnThongKe_Click(object sender, RoutedEventArgs e) => LoadData();

        private void BtnXuatPDF_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime tuNgay = dpTuNgay.SelectedDate ?? DateTime.Now.AddDays(-10);
                DateTime denNgay = dpDenNgay.SelectedDate ?? DateTime.Now;
                var doanhThuData = bll.LayDoanhThuTheoKhoang(tuNgay, denNgay);

                SaveFileDialog sfd = new SaveFileDialog { Filter = "PDF Files (*.pdf)|*.pdf", FileName = $"BaoCao_{DateTime.Now:yyyyMMdd}.pdf" };

                if (sfd.ShowDialog() == true)
                {
                    using (PdfWriter writer = new PdfWriter(sfd.FileName))
                    {
                        using (PdfDocument pdf = new PdfDocument(writer))
                        {
                            Document document = new Document(pdf);
                            PdfFont fontBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                            // Sử dụng tường minh iText.Layout.Properties để tránh lỗi Ambiguous reference
                            var centerAlign = iText.Layout.Properties.TextAlignment.CENTER;
                            var rightAlign = iText.Layout.Properties.TextAlignment.RIGHT;

                            document.Add(new iText.Layout.Element.Paragraph("BAO CAO DOANH THU")
                                .SetFont(fontBold).SetFontSize(18)
                                .SetTextAlignment(centerAlign));

                            document.Add(new iText.Layout.Element.Paragraph($"Thoi gian: {tuNgay:dd/MM/yyyy} - {denNgay:dd/MM/yyyy}")
                                .SetTextAlignment(centerAlign));

                            document.Add(new iText.Layout.Element.Paragraph("\n"));

                            // Tạo bảng với độ rộng cột đều nhau
                            Table table = new Table(iText.Layout.Properties.UnitValue.CreatePercentArray(new float[] { 50, 50 })).UseAllAvailableWidth();

                            table.AddHeaderCell(new Cell().Add(new iText.Layout.Element.Paragraph("Ngay").SetFont(fontBold)));
                            table.AddHeaderCell(new Cell().Add(new iText.Layout.Element.Paragraph("Doanh thu (VND)").SetFont(fontBold)));

                            decimal tong = 0;
                            foreach (var data in doanhThuData)
                            {
                                table.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph(data.Key)));
                                table.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph(data.Value.ToString("N0"))));
                                tong += data.Value;
                            }
                            document.Add(table);

                            document.Add(new iText.Layout.Element.Paragraph($"\nTONG CONG: {tong.ToString("N0")} VND")
                                .SetFont(fontBold).SetTextAlignment(rightAlign));

                            document.Close();
                        }
                    }
                    MessageBox.Show("Xuất PDF báo cáo thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất PDF: " + ex.Message, "Lỗi hệ thống", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}