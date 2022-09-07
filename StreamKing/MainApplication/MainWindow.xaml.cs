using StreamKing.Data.Media;
using StreamKing.MainApplication.ViewModels;
using System;
using System.Collections.ObjectModel;
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
            SetWatchlist();
            UpdateHeader();
            App._mainWindow = this;
        }

        public void SetSettingsView()
        {
            _viewModel.MainPage = new SettingsPageViewModel();
            UpdateDataContext();
        }

        public void SetWatchlist()
        {
            if (_viewModel.MainPage is not null && App.Watchlist is not null)
            {
                ((LandingPageViewModel)_viewModel.MainPage).Watchlist = App.Watchlist;
                Console.WriteLine("Updated Watchlist: " + ((LandingPageViewModel)_viewModel.MainPage).Watchlist.Name);
                Console.WriteLine("   SeriesList: ");
                foreach (var entry in ((LandingPageViewModel)_viewModel.MainPage).Watchlist.SeriesList)
                {
                    Console.WriteLine("   " + entry.Tag + ": " + entry.Series.Title);
                }
                Console.WriteLine("   MovieList: " + ((LandingPageViewModel)_viewModel.MainPage).Watchlist.Name);

                foreach (var entry in ((LandingPageViewModel)_viewModel.MainPage).Watchlist.MovieList)
                {
                    Console.WriteLine("   " + entry.Tag + ": " + entry.Movie.Title);
                }
                UpdateDataContext();
            }
        }

        public Media? GetSelectedMedia()
        {
            if (_viewModel.SelectedMedia is not null)
            {
                return _viewModel.SelectedMedia;
            }
            return null;
        }

        public WatchEntry? GetSelectedWatchEntry()
        {
            if (_viewModel.SelectedMovieEntry is not null)
            {
                return _viewModel.SelectedMovieEntry;
            }
            if (_viewModel.SelectedSeriesEntry is not null)
            {
                return _viewModel.SelectedSeriesEntry;
            }
            return null;
        }

        public void SetMedialist()
        {
            if (_viewModel.MainPage is not null && App._mediaList is not null)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    ((LandingPageViewModel)_viewModel.MainPage).MediaList = new ObservableCollection<Media>(App._mediaList);
                    Console.WriteLine("Updated MediaList: " + ((LandingPageViewModel)_viewModel.MainPage).MediaList.Count);
                });
            }
        }

        public void SetSelectedMedia(Media? media)
        {
            _viewModel.SelectedMedia = media;
            UpdateDataContext();
            if (media is not null)
            {
                Console.WriteLine("New selected media: " + media.Title);
                if (media.StreamingInfos is not null)
                {
                    Console.WriteLine("StreamingInfos: ");
                    foreach (var streamInfo in media.StreamingInfos)
                    {
                        Console.WriteLine("    " + streamInfo.Country + ": " + streamInfo.Name);
                    }
                }
                if (media.Genres is not null)
                {
                    Console.WriteLine("Genres: ");
                    foreach (var genre in media.Genres)
                    {
                        Console.WriteLine(genre.Name);
                    }
                }
            }
            else
            {
                Console.WriteLine("Deleted selected media");
            }
        }

        public void SetSelectedMovieEntry(MovieEntry? media)
        {
            _viewModel.SelectedMovieEntry = media;
            UpdateDataContext();
            if (media is not null)
            {
                Console.WriteLine("New selected media: " + media.Movie.Title);
                if (media.Movie.StreamingInfos is not null)
                {
                    Console.WriteLine("StreamingInfos: ");
                    foreach (var streamInfo in media.Movie.StreamingInfos)
                    {
                        Console.WriteLine("    " + streamInfo.Country + ": " + streamInfo.Name);
                    }
                }
                if (media.Movie.Genres is not null)
                {
                    Console.WriteLine("Genres: ");
                    foreach (var genre in media.Movie.Genres)
                    {
                        Console.WriteLine(genre.Name);
                    }
                }
            }
            else
            {
                Console.WriteLine("Deleted selected movieentry");
            }
        }

        public void SetSelectedSeriesEntry(SeriesEntry? media)
        {
            _viewModel.SelectedSeriesEntry = media;
            UpdateDataContext();
            if (media is not null)
            {
                Console.WriteLine("New selected media: " + media.Series.Title);
                if (media.Series.StreamingInfos is not null)
                {
                    Console.WriteLine("StreamingInfos: ");
                    foreach (var streamInfo in media.Series.StreamingInfos)
                    {
                        Console.WriteLine("    " + streamInfo.Country + ": " + streamInfo.Name);
                    }
                }
                if (media.Series.Genres is not null)
                {
                    Console.WriteLine("Genres: ");
                    foreach (var genre in media.Series.Genres)
                    {
                        Console.WriteLine(genre.Name);
                    }
                }
            }
            else
            {
                Console.WriteLine("Deleted selected seriesentry");
            }
        }
        public void UpdateDataContext()
        {
            DataContext = null;
            DataContext = _viewModel;
        }

        public void SetLandingPageView()
        {
            Console.WriteLine(App._mediaList.Count);
            _viewModel.MainPage = new LandingPageViewModel { MediaList = new ObservableCollection<Media>(App._mediaList), Watchlist = App.Watchlist };
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

            if (App._currentUser is not null)
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
