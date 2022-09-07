using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StreamKing.Login.Views
{

    public partial class RegisterView1 : UserControl
    {
        //public RegisterViewModel1 _viewModel;
        string? region = "US";

        public static int _inputMaxLength = 40;
        public static int _inputMinLength = 0;
        public static bool _isSigningUp = false;

        public RegisterView1()
        {
            InitializeComponent();
            UsernameInput.Text = LoginWindow.Udata.firstName;
            UserLastnameInput.Text = LoginWindow.Udata.lastName;
            if (LoginWindow.Udata.region == null)
            {
                region = "US";
                rb_us.IsChecked = true;
            }
            else if (LoginWindow.Udata.region == "FI")
            {
                region = "FI";
                rb_fi.IsChecked = true;
            }
            else if (LoginWindow.Udata.region == "DE")
            {
                region = "DE";
                rb_de.IsChecked = true;
            }
            else
            {
                region = "US";
                rb_us.IsChecked = true;
            }

        }

        private void BackToLoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = (LoginWindow)Window.GetWindow(this);

            // Clear the model
            LoginWindow.ClearUserModel();
            loginWindow.SetLoginView();
        }

        private void UsernameInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            checkValidInputs();
        }

        private void UserLastnameInput_TextChanged(object sender, RoutedEventArgs e)
        {
            checkValidInputs();
        }

        private void NextButton_Clicked(object sender, RoutedEventArgs e)
        {
            LoginWindow login = (LoginWindow)Window.GetWindow(this);
            LoginWindow.Udata.firstName = UsernameInput.Text;
            LoginWindow.Udata.lastName = UserLastnameInput.Text;
            LoginWindow.Udata.region = region;
            login.SetRegisterView2();
        }

        private void radioUS_Click(object sender, RoutedEventArgs e)
        {
            region = "US";
            LoginWindow.Udata.region = region;
        }

        private void radioDE_Click(object sender, RoutedEventArgs e)
        {
            region = "DE";
            LoginWindow.Udata.region = region;
        }
        private void radioFI_Click(object sender, RoutedEventArgs e)
        {
            region = "FI";
            LoginWindow.Udata.region = region;
        }

        public void checkValidInputs()
        {

            if (UserLastnameInput.Text.Length <= _inputMaxLength
                && UserLastnameInput.Text.Length > _inputMinLength
                && UsernameInput.Text.Length > _inputMinLength
                && UsernameInput.Text.Length <= _inputMaxLength)
            {
                SignUpInfo.Visibility = Visibility.Collapsed;

                UserLastnameInputLabel.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ffffff");
                UsernameInputLabel.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ffffff");

                LoginWindow.Udata.firstName = UsernameInput.Text;
                LoginWindow.Udata.lastName = UserLastnameInput.Text;


                NextButton.IsEnabled = true;
            }
            else
            {
                SignUpInfo.Visibility = Visibility.Visible;
                string signUpInfo = "";
                SignUpInfo.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ff0000");

                UserLastnameInputLabel.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ffffff");
                UsernameInputLabel.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ffffff");

                if (UsernameInput.Text.Length > _inputMaxLength)
                {
                    signUpInfo = "Your First Name is not allowed to be longer than " + _inputMaxLength + " characters.";
                    UsernameInputLabel.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ff0000");
                }
                else if (UsernameInput.Text.Length <= _inputMinLength)
                {
                    signUpInfo = "Your First Name is not allowed to be shorter than " + (_inputMinLength + 1) + " characters.";
                    UsernameInputLabel.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ff0000");
                }
                else if (UserLastnameInput.Text.Length <= _inputMinLength)
                {
                    signUpInfo = "Your Last Name is not allowed to be shorter than " + (_inputMinLength + 1) + " characters.";
                    UserLastnameInputLabel.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ff0000");
                }
                else if (UserLastnameInput.Text.Length > _inputMaxLength)
                {
                    signUpInfo = "Your Last Name is not allowed to be longer than " + _inputMaxLength + " characters.";
                    UserLastnameInputLabel.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ff0000");
                }


                SignUpInfo.Text = signUpInfo;
                NextButton.IsEnabled = false;
            }
        }
    }
}
