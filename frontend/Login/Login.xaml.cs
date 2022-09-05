using frontend.Login.Models;
using frontend.Login.ViewModels;
using System.Windows;

namespace frontend.Login
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public static UserData Udata { get; set; }
        public Login()
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
