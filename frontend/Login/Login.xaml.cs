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

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordInput.Password.Length > 0)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                LoginButton.IsEnabled = false;

                var status = await App.Login(UsernameInput.Text, PasswordInput.Password);

                Mouse.OverrideCursor = null;
                LoginButton.IsEnabled = true;

                UsernameInput.Text = "";
                PasswordInput.Password = "";
                MessageBox.Show("Login status: " + status);
            }
            else
            {
                MessageBox.Show("Password is empty!");
            }
        }
    }
}
