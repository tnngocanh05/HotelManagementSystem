using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HotelManagement.BLL;
using HotelManagement.DTO;
using HotelManagement.GUI.Dialogs;

namespace HotelManagement.GUI.Views
{
    public partial class TrangChuView : UserControl
    {
        private PhongBLL phongBLL = new PhongBLL();

        // Biến lưu trạng thái đang tìm phòng theo thời gian
        private bool dangTimTheoThoiGian = false;
        private DateTime? thoiGianNhanDangTim = null;
        private DateTime? thoiGianTraDangTim = null;

        public TrangChuView()
        {
            InitializeComponent();
            LoadFilterData();
            LoadRoomMap();
        }

        private void LoadFilterData()
        {
            dpNgayNhan.SelectedDate = DateTime.Today;

            cbGioNhan.Items.Clear();
            cbGioNhan.Items.Add("08:00");
            cbGioNhan.Items.Add("10:00");
            cbGioNhan.Items.Add("12:00");
            cbGioNhan.Items.Add("14:00");
            cbGioNhan.Items.Add("16:00");
            cbGioNhan.Items.Add("18:00");
            cbGioNhan.SelectedIndex = 3;

            cbSoDem.Items.Clear();
            cbSoDem.Items.Add("1");
            cbSoDem.Items.Add("2");
            cbSoDem.Items.Add("3");
            cbSoDem.Items.Add("4");
            cbSoDem.Items.Add("5");
            cbSoDem.SelectedIndex = 0;

            cbLoaiPhong.Items.Clear();
            cbLoaiPhong.Items.Add("Tất cả");
            cbLoaiPhong.Items.Add("Phòng đơn");
            cbLoaiPhong.Items.Add("Phòng đôi");
            cbLoaiPhong.Items.Add("VIP");
            cbLoaiPhong.Items.Add("Luxury");
            cbLoaiPhong.SelectedIndex = 0;

            cbTang.Items.Clear();
            cbTang.Items.Add("Tất cả");
            for (int i = 1; i <= 5; i++)
            {
                cbTang.Items.Add($"Tầng {i}");
            }
            cbTang.SelectedIndex = 0;

            btnTimPhong.Click -= BtnTimPhong_Click;
            btnTimPhong.Click += BtnTimPhong_Click;
        }

        private void BtnTimPhong_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dpNgayNhan.SelectedDate == null)
                {
                    MessageBox.Show("Vui lòng chọn ngày nhận!",
                                    "Thông báo",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                    return;
                }

                if (cbGioNhan.SelectedItem == null || cbSoDem.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn đầy đủ giờ nhận và số đêm!",
                                    "Thông báo",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                    return;
                }

                DateTime ngayNhan = dpNgayNhan.SelectedDate.Value;
                string gioNhan = cbGioNhan.SelectedItem.ToString();
                int soDem = int.Parse(cbSoDem.SelectedItem.ToString());

                DateTime thoiGianNhanMoi = DateTime.Parse(
                    ngayNhan.ToString("yyyy-MM-dd") + " " + gioNhan);

                DateTime thoiGianTraMoi = ngayNhan.Date.AddDays(soDem).AddHours(12);

                dangTimTheoThoiGian = true;
                thoiGianNhanDangTim = thoiGianNhanMoi;
                thoiGianTraDangTim = thoiGianTraMoi;

                LoadAvailableRooms(thoiGianNhanMoi, thoiGianTraMoi);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xử lý thời gian tìm phòng: " + ex.Message,
                                "Thông báo lỗi",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        private void LoadRoomMap()
        {
            dangTimTheoThoiGian = false;
            thoiGianNhanDangTim = null;
            thoiGianTraDangTim = null;

            RoomContainer.Children.Clear();

            var danhSachPhong = phongBLL.GetAllRooms();

            string searchRoom = txtSearchRoom.Text.Trim().ToLower();
            string loaiPhong = cbLoaiPhong.SelectedItem?.ToString() ?? "Tất cả";
            string tangText = cbTang.SelectedItem?.ToString() ?? "Tất cả";

            if (!string.IsNullOrEmpty(searchRoom))
            {
                danhSachPhong = danhSachPhong
                    .Where(p => p.SoPhong.ToLower().Contains(searchRoom))
                    .ToList();
            }

            if (loaiPhong != "Tất cả")
            {
                danhSachPhong = danhSachPhong
                    .Where(p => p.TenLoaiPhong == loaiPhong)
                    .ToList();
            }

            if (tangText != "Tất cả")
            {
                int tang = int.Parse(tangText.Replace("Tầng ", ""));
                danhSachPhong = danhSachPhong
                    .Where(p => p.Tang == tang)
                    .ToList();
            }

            HienThiDanhSachPhong(danhSachPhong);
        }

        private void LoadAvailableRooms(DateTime thoiGianNhanMoi, DateTime thoiGianTraMoi)
        {
            RoomContainer.Children.Clear();

            var danhSachPhong = phongBLL.GetAvailableRooms(thoiGianNhanMoi, thoiGianTraMoi);

            string searchRoom = txtSearchRoom.Text.Trim().ToLower();
            string loaiPhong = cbLoaiPhong.SelectedItem?.ToString() ?? "Tất cả";
            string tangText = cbTang.SelectedItem?.ToString() ?? "Tất cả";

            if (!string.IsNullOrEmpty(searchRoom))
            {
                danhSachPhong = danhSachPhong
                    .Where(p => p.SoPhong.ToLower().Contains(searchRoom))
                    .ToList();
            }

            if (loaiPhong != "Tất cả")
            {
                danhSachPhong = danhSachPhong
                    .Where(p => p.TenLoaiPhong == loaiPhong)
                    .ToList();
            }

            if (tangText != "Tất cả")
            {
                int tang = int.Parse(tangText.Replace("Tầng ", ""));
                danhSachPhong = danhSachPhong
                    .Where(p => p.Tang == tang)
                    .ToList();
            }

            HienThiDanhSachPhong(danhSachPhong);
        }

        private void HienThiDanhSachPhong(System.Collections.Generic.List<PhongDTO> danhSachPhong)
        {
            var dsTheoTang = danhSachPhong
                .GroupBy(p => p.Tang)
                .OrderBy(g => g.Key);

            foreach (var tangGroup in dsTheoTang)
            {
                TextBlock txtFloor = new TextBlock
                {
                    Text = $"Tầng {tangGroup.Key}",
                    FontSize = 24,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 10, 0, 10),
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0F4C5C"))
                };

                WrapPanel wrap = new WrapPanel
                {
                    Margin = new Thickness(0, 0, 0, 20)
                };

                foreach (PhongDTO phong in tangGroup.OrderBy(p => p.SoPhong))
                {
                    Border roomCard = CreateRoomCard(phong);
                    wrap.Children.Add(roomCard);
                }

                RoomContainer.Children.Add(txtFloor);
                RoomContainer.Children.Add(wrap);
            }
        }

        private Border CreateRoomCard(PhongDTO phong)
        {
            Border card = new Border
            {
                Width = 180,
                Height = 120,
                CornerRadius = new CornerRadius(10),
                Margin = new Thickness(8),
                Background = GetBackgroundByStatus(phong.TinhTrang),
                Cursor = System.Windows.Input.Cursors.Hand
            };

            StackPanel stack = new StackPanel
            {
                Margin = new Thickness(12)
            };

            stack.Children.Add(new TextBlock
            {
                Text = phong.SoPhong,
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.White
            });

            stack.Children.Add(new TextBlock
            {
                Text = phong.TenLoaiPhong,
                FontSize = 14,
                Margin = new Thickness(0, 8, 0, 5),
                Foreground = Brushes.White
            });

            stack.Children.Add(new TextBlock
            {
                Text = phong.TinhTrang,
                FontSize = 18,
                FontWeight = FontWeights.SemiBold,
                Foreground = Brushes.White
            });

            card.Child = stack;

            card.MouseLeftButtonUp += (s, e) =>
            {
                // Nếu đang ở chế độ đã tìm phòng theo thời gian,
                // thì các phòng đang hiển thị đều là phòng có thể đặt trong khoảng đó
                if (dangTimTheoThoiGian)
                {
                    DatPhongDialog dlg = new DatPhongDialog();

                    dlg.MaPhong = phong.SoPhong;
                    dlg.LoaiPhong = phong.TenLoaiPhong;

                    bool? result = dlg.ShowDialog();

                    if (result == true)
                    {
                        if (thoiGianNhanDangTim.HasValue && thoiGianTraDangTim.HasValue)
                        {
                            LoadAvailableRooms(thoiGianNhanDangTim.Value, thoiGianTraDangTim.Value);
                        }
                        else
                        {
                            LoadRoomMap();
                        }
                    }
                }
                else
                {
                    // Chế độ xem mặc định
                    if (phong.TinhTrang == "Trống")
                    {
                        DatPhongDialog dlg = new DatPhongDialog();

                        dlg.MaPhong = phong.SoPhong;
                        dlg.LoaiPhong = phong.TenLoaiPhong;

                        bool? result = dlg.ShowDialog();

                        if (result == true)
                        {
                            LoadRoomMap();
                        }
                    }
                    else if (phong.TinhTrang == "Đang ở")
                    {
                        ChiTietLuuTruDialog dlg = new ChiTietLuuTruDialog();
                        dlg.SoPhong = phong.SoPhong;
                        dlg.ShowDialog();

                        LoadRoomMap();
                    }
                    else
                    {
                        MessageBox.Show(
                            $"Phòng {phong.SoPhong} hiện đang ở trạng thái: {phong.TinhTrang}\nKhông thể thao tác phòng này.",
                            "Thông báo",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                    }
                }
            };

            return card;
        }

        private Brush GetBackgroundByStatus(string tinhTrang)
        {
            switch (tinhTrang)
            {
                case "Trống":
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#BEE9FF"));
                case "Đang ở":
                case "Đã đặt":
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4CAF50"));
                case "Bảo trì":
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9E9E9E"));
                case "Dọn dẹp":
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F4C542"));
                default:
                    return Brushes.LightGray;
            }
        }
    }
}