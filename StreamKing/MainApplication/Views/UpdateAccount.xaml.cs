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
            //ViewModel = (UpdateAccountViewModel)this.DataContext;
            //if (ViewModel is not null)
            //{

            //    User = ViewModel.User;
            //    MessageBox.Show("Username from if: " + User.FirstName);
            //    // (DataContext as UpdateAccountViewModel).User;
            //}
            //else
            //{
            //    User = ViewModel.User;
            //    MessageBox.Show("Username from else: " + User.FirstName);
            //}
           

        }
      
        //private void UpdateButton_Clicked(object sender, RoutedEventArgs e)
        //{
        //    var updateRequest = new UpdateRequest {Username = UsernameInput.Text, Email = EmailInput.Text, FirstName = FirstNameInput.Text, LastName = LastNameInput.Text };
           
        //    App.AdminUpdateSelectedUser(User, updateRequest);

        //}
    }
}
