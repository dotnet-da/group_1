using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;
using StreamKing.Login.Models;
using StreamKing.Web.Models;

namespace StreamKing.Login.Views
{
    /// <summary>
    /// Interaction logic for RegisterView2.xaml
    /// </summary>
    public partial class RegisterView2 : UserControl
    {
        //public UserData userData;
        public static int _usernameMinLength = 4;
        public static int _passwordMinLength = 8;
        public static int _inputMaxLength = 40;
        int num = 0;
        string verifyString = "";

        public static bool _isSigningUp = false;

        public RegisterView2()
        {
            InitializeComponent();
            LoadCaptcha();
            UsernameInput.Text = LoginWindow.Udata.userName;
            //userData = Login.userData;
        }

        private void BackToLoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = (LoginWindow)Window.GetWindow(this);

            // Clear the model
            LoginWindow.ClearUserModel();
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

        private void InputCaptcha_TextChanged(object sender, TextChangedEventArgs e)
        {
            checkValidInputs();
        }

        private void GenerateCaptcha_Click(object sender, RoutedEventArgs e)
        {
            LoadCaptcha();
            checkValidInputs();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow registerView1 = (LoginWindow)Window.GetWindow(this);
            registerView1.SetRegisterView();
        }
        public void checkValidInputs()
        {
            verifyString = num.ToString();

            if (RepeatPasswordInput.Password == PasswordInput.Password
                && PasswordInput.Password.Length >= _passwordMinLength
                && PasswordInput.Password.Length <= _inputMaxLength
                && UsernameInput.Text.Length > _usernameMinLength
                && UsernameInput.Text.Length <= _inputMaxLength
                && verifyString.Equals(InputCaptcha.Text.ToString()))
            {
                SignUpInfo.Visibility = Visibility.Collapsed;

                PasswordInputLabel.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ffffff");
                UsernameInputLabel.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ffffff");
                RepeatPasswordInputLabel.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ffffff");
                VerifyCaptchaTextBlock.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ffffff");

                setModelParams(UsernameInput.Text);

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
                else if (UsernameInput.Text.Length > _inputMaxLength)
                {
                    signUpInfo = "Please choose a username with at most " + _inputMaxLength + " characters.";
                    UsernameInputLabel.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ff0000");
                }
                else if (PasswordInput.Password.Length < _passwordMinLength)
                {
                    signUpInfo = "Password needs to be at least " + _passwordMinLength + " characters long.";
                    PasswordInputLabel.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ff0000");
                }
                else if (PasswordInput.Password.Length > _inputMaxLength)
                {
                    signUpInfo = "Password cannot contain more than " + _inputMaxLength + " characters.";
                    PasswordInputLabel.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ff0000");
                }
                else if (RepeatPasswordInput.Password != PasswordInput.Password)
                {
                    signUpInfo = "Password and Repeat Password are not the same.";
                    RepeatPasswordInputLabel.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ff0000");
                }
                else if (!verifyString.Equals(InputCaptcha.Text.ToString()))
                {
                    signUpInfo = "Captcha and Verify Captcha are not the same.";
                    VerifyCaptchaTextBlock.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ff0000");
                }

                SignUpInfo.Text = signUpInfo;
                SignUpButton.IsEnabled = false;
            }
        }

        private void setModelParams(string userName)
        {
            LoginWindow.Udata.userName = userName;
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

                RegisterRequest registerRequest = new RegisterRequest
                {
                    Username = username,
                    Password = password,
                    Region = LoginWindow.Udata.region,
                    FirstName = LoginWindow.Udata.firstName,
                    LastName = LoginWindow.Udata.lastName,
                };

                var status = await App.Register(registerRequest);

                Mouse.OverrideCursor = null;

               

                if (!status.Contains("ERROR"))
                {
                    MessageBox.Show(status + "\n You can now Login");
                    UsernameInput.Clear();
                    PasswordInput.Clear();
                    RepeatPasswordInput.Clear();

                    BackToLoginButton_Click(null, null);
                }
                else 
                {
                    MessageBox.Show(status);
                }
                _isSigningUp = false;
                SignUpButton.IsEnabled = !_isSigningUp;
            }
            else
            {
                MessageBox.Show("Already signing up. Don't be so impatient!");
            }
        }

        private void LoadCaptcha()
        {
            Random random = new Random();
            num = random.Next(1000000, 10000000);
            var image = new Bitmap((int)this.CaptchaImage.Width, (int)this.CaptchaImage.Height);
            var font = new Font("Comic", 25, System.Drawing.FontStyle.Bold, GraphicsUnit.Pixel);
            var graphics = Graphics.FromImage(image);
            graphics.DrawString(num.ToString(), font, System.Drawing.Brushes.Red, new System.Drawing.Point(90, 2));

            drawImage(image);


        }

        // Convert bitmap to System.Windows.Controls.Image
        private void drawImage(System.Drawing.Bitmap bitmap)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();
            CaptchaImage.Source = bi;
        }
    }
}
