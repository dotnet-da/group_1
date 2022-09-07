using StreamKing.Data.Media;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace StreamKing.MainApplication.ViewModels
{
    public class LandingPageViewModel : MainPage, INotifyPropertyChanged
    {
        public LandingPageViewModel()
        {
            MediaList = new ObservableCollection<Media>(App._mediaList);
        }

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

        public ObservableCollection<Media> _mediaWatchlist;
        public ObservableCollection<Media> MediaWatchlist
        {
            get
            {
                return _mediaWatchlist;
            }
            set
            {
                if (value != _mediaWatchlist)
                {
                    _mediaWatchlist = value;
                    OnPropertyChanged("MediaWatchlist");
                }
            }
        }

        public Watchlist Watchlist
        {
            get { return (Watchlist)GetValue(WatchlistProperty); }
            set
            {
                SetValue(WatchlistProperty, value);
                ObservableCollection<Media> result = new ObservableCollection<Media>();
                if (Watchlist != null)
                {
                    foreach (var movie in Watchlist.MovieList)
                    {
                        result.Add(movie.Movie);
                    }
                    foreach (var series in Watchlist.SeriesList)
                    {
                        result.Add(series.Series);
                    }
                }
                _mediaWatchlist = result;
            }
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
