using StreamKing.Data.Accounts;
using StreamKing.Web.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace StreamKing.MainApplication.Views
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : UserControl
    {
        public SettingsPage()
        {
            InitializeComponent();
            SetValues();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Window.GetWindow(this)).SetLandingPageView();
        }

        public void SetValues()
        {
            Account user = App._currentUser;

            if (user is not null)
            {
                UsernameInput.Text = user.Username;
                IdInput.Text = user.Id.ToString();

                EmailInput.Text = user.Email;
                TypeInput.Text = Enum.GetName(typeof(AccountType), user.Type);

                StatusInput.Text = Enum.GetName(typeof(AccountStatus), user.Status);
                FailedLoginsInput.Text = user.FailedLogins.ToString();

                FirstNameInput.Text = user.FirstName;
                LastNameInput.Text = user.LastName;

                BirthdayInput.Text = user.Birthday.ToShortDateString();
                CreatedInput.Text = user.Created.ToShortDateString();

                RegionInput.Text = user.Region;
            }
        }

        private void DeleteButton_Clicked(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete your Account?", "Attention", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                App.DeleteCurrentUser();
            }
        }

        private void UpdateButton_Clicked(object sender, RoutedEventArgs e)
        {
            var updateRequest = new UpdateRequest { Email = EmailInput.Text, FirstName = FirstNameInput.Text, LastName = LastNameInput.Text };
            App.UpdateCurrentuser(updateRequest);
        }
    }
}
