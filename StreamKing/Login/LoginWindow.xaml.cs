using StreamKing.Login.ViewModels;
using System.Windows;

namespace StreamKing.Login
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
        }

        public void SetRegisterView()
        {
            Height = 640;
            DataContext = new RegisterViewModel();
        }

        public void SetLoginView()
        {
            Height = 480;
            DataContext = new LoginViewModel();
        }
    }
}
