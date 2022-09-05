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
        public MainWindow()
        {
            InitializeComponent();
            UpdateHeader();
            App._mainWindow = this;
        }

        public void UpdateHeader()
        {
            string? Region;
            try
            {
                Region = App._currentUser.Region;
            }
            catch (System.Exception)
            {
                Region = null;
            }

            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (Region == "DE")
            {
                viewModel.ActiveRegionImage = "../Assets/Images/region_DE.jpg";
            }
            else if (Region == "FI")
            {
                viewModel.ActiveRegionImage = "../Assets/Images/region_FI.jpg";
            }
            else
            {
                viewModel.ActiveRegionImage = "../Assets/Images/region_US.jpg";
            }

            DataContext = viewModel;
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
            MainWindowViewModel viewModel = new MainWindowViewModel();
            if (button.Name.Contains("DE"))
            {
                viewModel.ActiveRegionImage = "../Assets/Images/region_DE.jpg";
            }
            else if (button.Name.Contains("FI"))
            {
                viewModel.ActiveRegionImage = "../Assets/Images/region_FI.jpg";
            }
            else if (button.Name.Contains("US"))
            {
                viewModel.ActiveRegionImage = "../Assets/Images/region_US.jpg";
            }

            if (RegionSwitch.Visibility == Visibility.Visible)
            {
                RegionSwitch.Visibility = Visibility.Collapsed;
            }

            DataContext = viewModel;
        }

        private void Logout_Clicked(object sender, RoutedEventArgs e)
        {
            App.Logout();
        }

        private void SettingsButton_Clicked(object sender, RoutedEventArgs e)
        {
            if (Menu.Visibility == Visibility.Visible)
            {
                Menu.Visibility = Visibility.Collapsed;
            }
            MessageBox.Show("SettingsButton_Clicked");
        }
    }
}
