using frontend.Login.Models;
using frontend.Login.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace frontend.Login.Views
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
            UsernameInput.Text = Login.Udata.firstName;
            UserLastnameInput.Text = Login.Udata.lastName;
            if (Login.Udata.region == null)
            {
                region = "US";
                rb_us.IsChecked = true;
            }
            else if (Login.Udata.region == "FI")
            {
                region = "FI";
                rb_fi.IsChecked = true;
            }
            else if (Login.Udata.region == "DE")
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
            Login loginWindow = (Login)Window.GetWindow(this);
            
            // Clear the model
            Login.ClearUserModel();
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
            Login login = (Login)Window.GetWindow(this);
            Login.Udata.firstName = UsernameInput.Text;
            Login.Udata.lastName = UserLastnameInput.Text;
            Login.Udata.region = region;
            login.SetRegisterView2();
        }

        private void radioUS_Click(object sender, RoutedEventArgs e)
        {
            region = "US";
            Login.Udata.region = region;
        }

        private void radioDE_Click(object sender, RoutedEventArgs e)
        {
            region = "DE";
            Login.Udata.region = region;
        }
        private void radioFI_Click(object sender, RoutedEventArgs e)
        {
            region = "FI";
            Login.Udata.region = region;
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

                Login.Udata.firstName = UsernameInput.Text;
                Login.Udata.lastName = UserLastnameInput.Text;
                

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
