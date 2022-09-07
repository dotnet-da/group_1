using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StreamKing.MainApplication.Views
{
    /// <summary>
    /// Interaction logic for DetailedMediaTemplateView.xaml
    /// </summary>
    public partial class DetailedMediaTemplateView : UserControl
    {
        public DetailedMediaTemplateView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (App._mainWindow is not null)
            {
                App._mainWindow.SetSelectedMedia(null);
                App._mainWindow.SetSelectedMovieEntry(null);
                App._mainWindow.SetSelectedSeriesEntry(null);
            }
        }

        private async void AddToWatchlistButton_Clicked(object sender, RoutedEventArgs e)
        {
            AddToWatchlistButton.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            var status = await App.AddSelectedMediaToWatchlist();

            AddToWatchlistButton.IsEnabled = true;
            Mouse.OverrideCursor = null;
            if (App._mainWindow is not null)
            {
                App._mainWindow.SetSelectedMedia(null);
            }
        }
        private async void RemoveFromWatchlistButton_Clicked(object sender, RoutedEventArgs e)
        {
            AddToWatchlistButton.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            var status = await App.RemoveSelectedWatchEntryFromWatchlist();

            AddToWatchlistButton.IsEnabled = true;
            Mouse.OverrideCursor = null;
            if (App._mainWindow is not null)
            {
                App._mainWindow.SetSelectedMovieEntry(null);
                App._mainWindow.SetSelectedSeriesEntry(null);
            }
        }
    }
}
