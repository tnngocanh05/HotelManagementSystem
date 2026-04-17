using HotelManagement.BLL;
using HotelManagement.DTO;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HotelManagement.GUI.Dialogs
{
    public partial class ThemDichVuDialog : Window
    {
        private DichVuDTO dv;

        public ThemDichVuDialog()
        {
            InitializeComponent();
        }

        public ThemDichVuDialog(DichVuDTO dvSua)
        {
            InitializeComponent();
            dv = dvSua;

            txtMa.Text = dv.MaDichVu.ToString(); // 🔥 HIỂN THỊ MÃ
            txtTen.Text = dv.TenDichVu;
            txtGia.Text = dv.DonGia.ToString();
        }


        private void btnHuy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            string loai = (cbLoai.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (dv == null)
            {
                DichVuDTO newDV = new DichVuDTO
                {
                    TenDichVu = txtTen.Text,
                    DonGia = decimal.Parse(txtGia.Text),
                    LoaiDichVu = loai
                };

                DichVuBLL.Instance.ThemDichVu(newDV);
            }
            else
            {
                dv.TenDichVu = txtTen.Text;
                dv.DonGia = decimal.Parse(txtGia.Text);
                dv.LoaiDichVu = loai;

                DichVuBLL.Instance.UpdateDichVu(dv);
            }

            this.Close();
        }
    }
}