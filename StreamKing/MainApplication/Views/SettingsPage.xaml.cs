using StreamKing.Data.Accounts;
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
    }
}
