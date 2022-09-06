using StreamKing.Login.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamKing.Login.ViewModels
{
    internal class RegisterViewModel2
    {
        
        public UserData _userData;

        public RegisterViewModel2()
        {
            _userData = LoginWindow.Udata;
        }
    }
}
