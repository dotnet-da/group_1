using StreamKing.Login.Models;

namespace StreamKing.Login.ViewModels
{
    public class RegisterViewModel1 : ViewModelBase
    {
        public UserData _userData;
        public string _firstName;
        public string _lastName;
        public string _region;

        public string firstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
                onPropertyChanged(nameof(firstName));
            }
        }

        public string lastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
                onPropertyChanged(nameof(lastName));
            }
        }

        public string region
        {
            get
            {
                return _region;
            }
            set
            {
                _region = value;
                onPropertyChanged(nameof(region));
            }
        }

        // default Constructor
        public RegisterViewModel1()
        {
            _userData = LoginWindow.Udata;
        }
    }
}
