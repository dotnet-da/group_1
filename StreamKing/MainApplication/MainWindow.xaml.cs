using StreamKing.Data.Media;
using StreamKing.Login.ViewModels;
using StreamKing.MainApplication.ViewModels;
using System;
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
            UpdateDataContext();
        }

        public void SetSelectedMedia(Media? media)
        {
            _viewModel.SelectedMedia = media;
            UpdateDataContext();
            if(media is not null)
            {
                Console.WriteLine("New selected media: " + media.Title);
                Console.WriteLine("StreamingInfos: ");
                foreach (var streamInfo in media.StreamingInfos)
                {
                    Console.WriteLine("    "+streamInfo.Country + ": " + streamInfo.Name);
                }
                Console.WriteLine("Genres: ");
                foreach (var genre in media.Genres)
                {
                    Console.WriteLine(genre.Name);
                }
            }
            else
            {
                Console.WriteLine("Deleted selected media");
            }
        }

        public void UpdateMediaListView()
        {
            if (_viewModel.MainPage.GetType() == typeof(LandingPageViewModel))
            {
                //(_viewModel.MainPage as LandingPageViewModel).SetMediaList();
            }
        }

        public void UpdateDataContext()
        {
            DataContext = null;
            DataContext = _viewModel;
        }

        public void SetLandingPageView()
        {
            _viewModel.MainPage = new LandingPageViewModel { MediaList = App._mediaList};
            UpdateDataContext();
        }

        public void UpdateCurrentUser()
        {
            _viewModel.Account = App._currentUser;
            UpdateDataContext();
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
            UpdateDataContext();
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
            UpdateDataContext();
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
