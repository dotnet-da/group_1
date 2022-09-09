using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using StreamKing.Data.Accounts;
using StreamKing.Data.Media;
using StreamKing.Web.Models;

namespace StreamKing.MainApplication.ViewModels
{
    public class UpdateAccountViewModel: MainPage
    {
        public Account User { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? UserName { get; set; }

        public UpdateAccountViewModel(Account acc)
        {
            User = acc;
            FirstName = acc.FirstName;
            LastName = acc.LastName;
            Email = acc.Email;
            UserName = acc.Username;
            this.UpdateButtonCommand = new DelegateCommand(
                (o) => !String.IsNullOrEmpty(UserName) || !String.IsNullOrEmpty(FirstName),
                (o) => 
                {
                    var updateRequest = new UpdateRequest { Username = UserName, Email = Email, FirstName = FirstName, LastName = LastName };

                    App.AdminUpdateSelectedUser(User, updateRequest);
                }
            );



        }
        public UpdateAccountViewModel()
        { 
        }
        public DelegateCommand UpdateButtonCommand { get; set; }
    }
}
