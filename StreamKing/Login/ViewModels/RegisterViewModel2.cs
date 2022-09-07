using StreamKing.Login.Models;

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
