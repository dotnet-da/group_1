using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace frontend.Login.Views
{
    /// <summary>
    /// Interaction logic for RegisterView.xaml
    /// </summary>
    public partial class RegisterView : UserControl
    {

        public static int _usernameMinLength = 4;
        public static int _passwordMinLength = 8;
        public static int _passwordMaxLength = 40;

        public static bool _isSigningUp = false;

        public RegisterView()
        {
            InitializeComponent();
        }

        private void BackToLoginButton_Click(object sender, RoutedEventArgs e)
        {
            Login loginWindow = (Login)Window.GetWindow(this);
            loginWindow.SetLoginView();
        }

        private void PasswordInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            checkValidInputs();
        }

        private void RepeatPasswordInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            checkValidInputs();
        }

        private void UsernameInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            checkValidInputs();
        }

        public void checkValidInputs()
        {

            if (RepeatPasswordInput.Password == PasswordInput.Password
    && RepeatPasswordInput.Password.Length >= _passwordMinLength
    && RepeatPasswordInput.Password.Length <= _passwordMaxLength
    && PasswordInput.Password.Length >= _passwordMinLength
    && PasswordInput.Password.Length <= _passwordMaxLength
    && UsernameInput.Text.Length > _usernameMinLength)
            {
                SignUpInfo.Visibility = Visibility.Collapsed;

                PasswordInputLabel.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ffffff");
                UsernameInputLabel.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ffffff");
                RepeatPasswordInputLabel.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ffffff");

                SignUpButton.IsEnabled = true;
            }
            else
            {
                SignUpInfo.Visibility = Visibility.Visible;
                string signUpInfo = "";
                SignUpInfo.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ff0000");

                PasswordInputLabel.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ffffff");
                UsernameInputLabel.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ffffff");
                RepeatPasswordInputLabel.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ffffff");

                if (UsernameInput.Text.Length < _usernameMinLength)
                {
                    signUpInfo = "Please choose a username with at least " + _usernameMinLength + " characters.";
                    UsernameInputLabel.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ff0000");
                }
                else if (PasswordInput.Password.Length < _passwordMinLength)
                {
                    signUpInfo = "Password needs to be at least " + _passwordMinLength + " characters long.";
                    PasswordInputLabel.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ff0000");
                }
                else if (PasswordInput.Password.Length > _passwordMaxLength)
                {
                    signUpInfo = "Password cannot contain more than " + _passwordMaxLength + " characters.";
                    PasswordInputLabel.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ff0000");
                }
                else if (RepeatPasswordInput.Password != PasswordInput.Password)
                {
                    signUpInfo = "Password and Repeat Password are not the same.";
                    RepeatPasswordInputLabel.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ff0000");
                }

                SignUpInfo.Text = signUpInfo;
                SignUpButton.IsEnabled = false;
            }
        }

        private async void SignUpButton_Click(object sender, RoutedEventArgs e)
        {

            if (PasswordInput.Password == RepeatPasswordInput.Password && !_isSigningUp)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                _isSigningUp = true;
                SignUpButton.IsEnabled = !_isSigningUp;

                var username = UsernameInput.Text;
                var password = PasswordInput.Password;

                var status = await App.Register(username, password);

                Mouse.OverrideCursor = null;

                MessageBox.Show("Register status: " + status);

                if (!status.Contains("ERROR"))
                {
                    UsernameInput.Clear();
                    PasswordInput.Clear();
                    RepeatPasswordInput.Clear();

                    BackToLoginButton_Click(null, null);
                }
                _isSigningUp = false;
                SignUpButton.IsEnabled = !_isSigningUp;
            }
            else
            {
                MessageBox.Show("Already signing up. Don't be so impatient!");
            }
        }
    }
}
