using StreamKing.Data.Media;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace StreamKing.MainApplication.ViewModels
{
    public class LandingPageViewModel : MainPage, INotifyPropertyChanged
    {
        private ObservableCollection<Media> _mediaList;
        public ObservableCollection<Media> MediaList
        {
            get
            {
                return _mediaList;
            }
            set
            {
                if (value != _mediaList)
                {
                    _mediaList = value;
                    OnPropertyChanged("MediaList");
                }
            }
        }

        public Watchlist Watchlist
        {
            get { return (Watchlist)GetValue(WatchlistProperty); }
            set { SetValue(WatchlistProperty, value); }
        }
        public static readonly DependencyProperty WatchlistProperty =
               DependencyProperty.Register("Watchlist", typeof(Watchlist), typeof(LandingPageViewModel), new PropertyMetadata());

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
