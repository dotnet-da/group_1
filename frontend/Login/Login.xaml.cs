using System.Windows;
using System.Windows.Input;

namespace frontend.Login
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        public static bool isLoggingIn = false;

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordInput.Password.Length > 0 && !isLoggingIn)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                isLoggingIn = true;

                var username = UsernameInput.Text;
                var password = PasswordInput.Password;

                UsernameInput.Clear();
                PasswordInput.Clear();

                var status = await App.Login(username, password);

                Mouse.OverrideCursor = null;

                MessageBox.Show("Login status: " + status);
                isLoggingIn = false;
            }
            else
            {
                MessageBox.Show("Password is empty!");
            }
        }
    }
}
