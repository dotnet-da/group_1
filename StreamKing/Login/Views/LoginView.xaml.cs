using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StreamKing.Login.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
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

                LoginWindow loginWindow = (LoginWindow)Window.GetWindow(this);

                var status = await App.Login(username, password);
                Console.WriteLine("Login status: " + status);

                isLoggingIn = false;

                if (status == "Success")
                {
                    loginWindow.Close();
                }
                else
                {
                    MessageBox.Show("Something went wrong: " + status);
                    Mouse.OverrideCursor = null;
                }
            }
            else
            {
                MessageBox.Show("Password is empty!");
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = (LoginWindow)Window.GetWindow(this);
            loginWindow.SetRegisterView();
        }
    }
}
