using System.Windows;
using System.Windows.Controls;

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
            App._mainWindow.SetSelectedMedia(null);
        }

        private void AddToWatchlistButton_Clicked(object sender, RoutedEventArgs e)
        {
            App.AddSelectedMediaToWatchlist();
        }
    }
}
