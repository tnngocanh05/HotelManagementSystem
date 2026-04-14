using System.Windows;

namespace QuanLyKhachSanWeb.admin
{
    public partial class LoginWindow : Window
    {
        private const string ValidEmail = "admin@gmail.com";
        private const string ValidPassword = "123456789";
        private bool _isPasswordVisible;

        public LoginWindow()
        {
            InitializeComponent();
            txtEmail.Text = ValidEmail;
        }

        private void BtnDangNhap_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = GetCurrentPassword();

            if (email == ValidEmail && password == ValidPassword)
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
                return;
            }

            MessageBox.Show("Sai email hoặc mật khẩu", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void BtnThoat_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnTogglePassword_Click(object sender, RoutedEventArgs e)
        {
            _isPasswordVisible = !_isPasswordVisible;

            if (_isPasswordVisible)
            {
                txtVisiblePassword.Text = pwdPassword.Password;
                txtVisiblePassword.Visibility = Visibility.Visible;
                pwdPassword.Visibility = Visibility.Collapsed;
                btnTogglePassword.Content = "Ẩn";
                txtVisiblePassword.Focus();
                txtVisiblePassword.SelectionStart = txtVisiblePassword.Text.Length;
            }
            else
            {
                pwdPassword.Password = txtVisiblePassword.Text;
                pwdPassword.Visibility = Visibility.Visible;
                txtVisiblePassword.Visibility = Visibility.Collapsed;
                btnTogglePassword.Content = "Hiện";
                pwdPassword.Focus();
            }
        }

        private void PwdPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!_isPasswordVisible)
            {
                txtVisiblePassword.Text = pwdPassword.Password;
            }
        }

        private void TxtVisiblePassword_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (_isPasswordVisible)
            {
                pwdPassword.Password = txtVisiblePassword.Text;
            }
        }

        private string GetCurrentPassword()
        {
            return _isPasswordVisible ? txtVisiblePassword.Text : pwdPassword.Password;
        }
    }
}
