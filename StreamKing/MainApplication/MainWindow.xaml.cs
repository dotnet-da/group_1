using StreamKing.Login.ViewModels;
using StreamKing.MainApplication.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace StreamKing.MainApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindowViewModel _viewModel { get; set; } = new MainWindowViewModel();
        public MainWindow()
        {
            DataContext = null;
            DataContext = _viewModel;

            InitializeComponent();
            UpdateHeader();
            App._mainWindow = this;
        }

        public void SetSettingsView()
        {
            _viewModel.MainPage = new SettingsPageViewModel();
            DataContext = null;
            DataContext = _viewModel;
        }

        public void SetLandingPageView()
        {
            _viewModel.MainPage = new LandingPageViewModel { MediaList = App._mediaList};
            DataContext = null;
            DataContext = _viewModel;
        }

        public void UpdateCurrentUser()
        {
            _viewModel.Account = App._currentUser;
            DataContext = null;
            DataContext = _viewModel;
        }

        public void UpdateHeader()
        {
            string? Region;

            if(App._currentUser is not null)
            {
                Region = App._currentUser.Region;
            }
            else
            {
                Region = null;
            }


            if (Region == "DE")
            {
                _viewModel.ActiveRegionImage = "../Assets/Images/region_DE.jpg";
            }
            else if (Region == "FI")
            {
                _viewModel.ActiveRegionImage = "../Assets/Images/region_FI.jpg";
            }
            else
            {
                _viewModel.ActiveRegionImage = "../Assets/Images/region_US.jpg";
            }
            DataContext = null;
            DataContext = _viewModel;
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            var addButton = sender as FrameworkElement;
            if (addButton != null)
            {
                addButton.ContextMenu.IsOpen = true;
            }
        }

        private void HeaderProfileButton_Click(object sender, RoutedEventArgs e)
        {
            if (Menu.Visibility == Visibility.Visible)
            {
                Menu.Visibility = Visibility.Collapsed;
            }
            else if (Menu.Visibility == Visibility.Collapsed)
            {
                if (RegionSwitch.Visibility == Visibility.Visible)
                {
                    RegionSwitch.Visibility = Visibility.Collapsed;
                }
                Menu.Visibility = Visibility.Visible;
            }
        }

        private void HeaderRegionButton_Click(object sender, RoutedEventArgs e)
        {
            if (RegionSwitch.Visibility == Visibility.Visible)
            {
                RegionSwitch.Visibility = Visibility.Collapsed;
            }
            else if (RegionSwitch.Visibility == Visibility.Collapsed)
            {
                if (Menu.Visibility == Visibility.Visible)
                {
                    Menu.Visibility = Visibility.Collapsed;
                }
                RegionSwitch.Visibility = Visibility.Visible;
            }
        }

        private void RegionSwitch_Clicked(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (button.Name.Contains("DE"))
            {
                App.SwitchRegion("DE");
                _viewModel.ActiveRegionImage = "../Assets/Images/region_DE.jpg";
            }
            else if (button.Name.Contains("FI"))
            {
                App.SwitchRegion("FI");
                _viewModel.ActiveRegionImage = "../Assets/Images/region_FI.jpg";
            }
            else if (button.Name.Contains("US"))
            {
                App.SwitchRegion("US");
                _viewModel.ActiveRegionImage = "../Assets/Images/region_US.jpg";
            }

            if (RegionSwitch.Visibility == Visibility.Visible)
            {
                RegionSwitch.Visibility = Visibility.Collapsed;
            }
            DataContext = null;
            DataContext = _viewModel;
        }

        private void Logout_Clicked(object sender, RoutedEventArgs e)
        {
            _viewModel = new MainWindowViewModel();
            App.Logout();
        }

        private void SettingsButton_Clicked(object sender, RoutedEventArgs e)
        {
            if (Menu.Visibility == Visibility.Visible)
            {
                Menu.Visibility = Visibility.Collapsed;
            }
            SetSettingsView();
        }
    }
}
