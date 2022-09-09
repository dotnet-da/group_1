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
using System.Windows.Shapes;
using StreamKing.Data.Accounts;
using StreamKing.Login;
using StreamKing.MainApplication.ViewModels;
using StreamKing.Web.Models;

namespace StreamKing.MainApplication.Views
{
    /// <summary>
    /// Interaktionslogik für UpdateAccount.xaml
    /// </summary>
    public partial class UpdateAccount : UserControl
    {
        public Account? User { get; set; }
        public UpdateAccountViewModel ViewModel { get; set; }

        public UpdateAccount() 
        {
            InitializeComponent();
        }

        private void CloseButton_Clicked(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Window.GetWindow(this)).SetAdminView();
        }

        private async void AddNewUser_Clicked(object sender, RoutedEventArgs e)
        {
            if(UsernameInput.Text.Length>0 && PasswordInput.Text.Length > 0)
            {
                RegisterRequest registerRequest = new RegisterRequest
                {
                    Username = UsernameInput.Text,
                    Password = PasswordInput.Text,
                    Email = EmailInput.Text,
                    FirstName = FirstNameInput.Text,
                    LastName = LastNameInput.Text,
                    Region = "US"
                };
                Mouse.OverrideCursor = Cursors.Wait;

                var status = await App.AddNewUser(registerRequest);

                Mouse.OverrideCursor = null;

                if (!status.Contains("ERROR"))
                {
                    MessageBox.Show(status + "\n New User Added");
                }
                else
                {
                    MessageBox.Show(status);
                }
            }
        }
    }
}
