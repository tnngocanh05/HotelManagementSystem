using System.Data.SqlClient;
using System.Windows;

namespace HotelManagement.GUI.Login
{
    public partial class LoginWindow : Window
    {
        string connectionString =
            @"Server=DESKTOP-8MRPC2J\SQLEXPRESS;Database=QuanLyKhachSan;Trusted_Connection=True;";

        public LoginWindow()
        {
            InitializeComponent();

            btnDangNhap.Click += BtnDangNhap_Click;
        }

        private void BtnDangNhap_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();

            string password;

            if (chkShowPassword.IsChecked == true)
                password = txtShowPassword.Text.Trim();
            else
                password = txtPassword.Password.Trim();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"
                    SELECT MaNhanVien, HoTen, MaChucVu
                    FROM NhanVien
                    WHERE TenDangNhap = @TenDangNhap
                    AND MatKhau = @MatKhau";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@TenDangNhap", username);
                cmd.Parameters.AddWithValue("@MatKhau", password);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    int maChucVu = (int)reader["MaChucVu"];

                    MainWindow main = new MainWindow();

                    // QUẢN LÝ
                    if (maChucVu == 2)
                    {
                        main.Show();
                    }

                    // LỄ TÂN
                    else if (maChucVu == 1)
                    {
                        main.btnNhanVien.Visibility = Visibility.Collapsed;
                        main.btnThongKe.Visibility = Visibility.Collapsed;

                        main.Show();
                    }

                    // NHÂN VIÊN
                    else if (maChucVu == 3)
                    {
                        main.btnNhanVien.Visibility = Visibility.Collapsed;
                        main.btnThongKe.Visibility = Visibility.Collapsed;
                        main.btnHoaDon.Visibility = Visibility.Collapsed;
                        main.btnKhachHang.Visibility = Visibility.Collapsed;
                        main.btnDanhSachDatPhong.Visibility = Visibility.Collapsed;
                        main.btnDichVu.Visibility = Visibility.Collapsed;

                        main.Show();
                    }

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Sai tài khoản hoặc mật khẩu");
                }
            }
        }

        private void chkShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            txtShowPassword.Text = txtPassword.Password;
            txtShowPassword.Visibility = Visibility.Visible;
            txtPassword.Visibility = Visibility.Collapsed;
        }

        private void chkShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            txtPassword.Password = txtShowPassword.Text;
            txtPassword.Visibility = Visibility.Visible;
            txtShowPassword.Visibility = Visibility.Collapsed;
        }
    }
}