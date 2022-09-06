using StreamKing.Login.Models;
using StreamKing.Login.ViewModels;
using System.Windows;

namespace StreamKing.Login
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public static UserData Udata { get; set; }
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
            Udata = new UserData();
        }

        public void SetRegisterView()
        {
            DataContext = new RegisterViewModel1();
        }

        public void SetRegisterView2()
        {
            DataContext = new RegisterViewModel2();
        }

        public void SetLoginView()
        {
            //Height = 480;
            DataContext = new LoginViewModel();
        }

        // clear the UserData Model
        public static void ClearUserModel()
        {
            Udata.region = "";
            Udata.firstName = "";
            Udata.lastName = "";
            Udata.userName = "";
            Udata.password = "";
        }

        
    }
}
