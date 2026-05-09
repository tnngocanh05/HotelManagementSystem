using HotelManagement.BLL;
using System;
using System.Windows;

namespace HotelManagement.GUI.Dialogs
{
    public partial class KiemTraPhongDialog : Window
    {
        private DenBuBLL denBuBLL = new DenBuBLL();

        public int MaDatPhong { get; set; }

        public KiemTraPhongDialog()
        {
            InitializeComponent();

            Loaded += KiemTraPhongDialog_Loaded;

            btnLuu.Click += BtnLuu_Click;
            btnDong.Click += BtnDong_Click;
        }

        private void KiemTraPhongDialog_Loaded(object sender, RoutedEventArgs e)
        {
            txtMaDatPhong.Text = MaDatPhong.ToString();
        }

        private void BtnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string noiDung = txtNoiDung.Text.Trim();

                decimal soTien;

                if (!decimal.TryParse(txtSoTien.Text.Trim(), out soTien))
                {
                    MessageBox.Show("Số tiền không hợp lệ.");
                    return;
                }

                bool result = denBuBLL.ThemDenBu(
                    MaDatPhong,
                    noiDung,
                    soTien);

                if (result)
                {
                    MessageBox.Show("Lưu đền bù thành công.");

                    DialogResult = true;

                    Close();
                }
                else
                {
                    MessageBox.Show("Không thể lưu đền bù.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void BtnDong_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}