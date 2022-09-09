using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using StreamKing.Data.Accounts;
using StreamKing.Data.Media;

namespace StreamKing.MainApplication.ViewModels
{
    public class UpdateAccountViewModel: MainPage
    {
        public Account? User { get; set; }

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


        }
        public UpdateAccountViewModel()
        { 
        }
    }
}
