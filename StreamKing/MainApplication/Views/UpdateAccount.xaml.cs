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
using StreamKing.Web.Models;

namespace StreamKing.MainApplication.Views
{
    /// <summary>
    /// Interaktionslogik für UpdateAccount.xaml
    /// </summary>
    public partial class UpdateAccount : UserControl
    {
        public Account User { get; set; }
        public UpdateAccount(Account user)
        {
            InitializeComponent();
            User = user;
            UsernameInput.Text = user.Username;
            EmailInput.Text = user.Email;
            FirstNameInput.Text = user.FirstName;
            LastNameInput.Text = user.LastName;
            
        }
        private void UpdateButton_Clicked(object sender, RoutedEventArgs e)
        {
            User.Username = UsernameInput.Text;
            User.Email = EmailInput.Text;
            User.FirstName = FirstNameInput.Text;
            User.LastName = LastNameInput.Text;

            var updateRequest = new UpdateRequest {Username = UsernameInput.Text, Email = EmailInput.Text, FirstName = FirstNameInput.Text, LastName = LastNameInput.Text };
           
            App.AdminUpdateSelectedUser(User, updateRequest);

        }
    }
}
