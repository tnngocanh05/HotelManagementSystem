using System.Windows;

namespace HotelManagement.GUI.admin
{
    public partial class LoginWindow : Window
    {
        private const string ValidEmail = "admin@gmail.com";
        private const string ValidPassword = "123456789";
        private bool isPasswordVisible;

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnDangNhap_Click(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Text.Trim() == ValidEmail && GetCurrentPassword() == ValidPassword)
            {
                MainWindow mainWindow = new MainWindow();
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
            isPasswordVisible = !isPasswordVisible;

            if (isPasswordVisible)
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
            if (!isPasswordVisible)
            {
                txtVisiblePassword.Text = pwdPassword.Password;
            }
        }

        private void TxtVisiblePassword_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (isPasswordVisible)
            {
                pwdPassword.Password = txtVisiblePassword.Text;
            }
        }

        private string GetCurrentPassword()
        {
            return isPasswordVisible ? txtVisiblePassword.Text : pwdPassword.Password;
        }
    }
}
