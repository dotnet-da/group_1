using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using StreamKing.Data.Accounts;
using StreamKing.Data.Media;

namespace StreamKing.MainApplication.ViewModels
{
    public class AdminViewModel: MainPage
    {
        public List<Account> allUsers { get; set; }

        //wie kann AdminView aus selectedUser zugreifen?
        public Account? selectedUser { get; set; }
        public AdminViewModel()
        {
            allUsers = new List<Account>(App._allUsers);
        }

    }
}
