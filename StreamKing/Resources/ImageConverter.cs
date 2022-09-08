using StreamKing.Data.Media;
using System;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace StreamKing.Resources
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            string s = "";
            if (value.GetType() == typeof(Media) || value.GetType() == typeof(Movie) || value.GetType() == typeof(Series))
            {
                s = (value as Media).BackdropURL;

            }
            else if (value.GetType() == typeof(MovieEntry))
            {
                s = (value as MovieEntry).Movie.BackdropURL;

            }
            else if (value.GetType() == typeof(SeriesEntry))
            {
                s = (value as SeriesEntry).Series.BackdropURL;
            }

            return new BitmapImage(new Uri("https://image.tmdb.org/t/p/w500" + s, UriKind.Absolute));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BigImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            string s = "";
            if (value.GetType() == typeof(Media) || value.GetType() == typeof(Movie) || value.GetType() == typeof(Series))
            {
                s = (value as Media).BackdropURL;

            }
            else if (value.GetType() == typeof(MovieEntry))
            {
                s = (value as MovieEntry).Movie.BackdropURL;

            }
            else if (value.GetType() == typeof(SeriesEntry))
            {
                s = (value as SeriesEntry).Series.BackdropURL;
            }

            return new BitmapImage(new Uri("https://image.tmdb.org/t/p/w780" + s, UriKind.Absolute));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
