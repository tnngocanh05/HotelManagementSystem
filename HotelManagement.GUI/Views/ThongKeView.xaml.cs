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
                        LabelPoint = point => point.Y.ToString("N0")
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
                Console.WriteLine("Lỗi LoadData: " + ex.Message);
            }
        }

        private void BtnThongKe_Click(object sender, RoutedEventArgs e) => LoadData();

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}