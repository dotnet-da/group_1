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
            try
            {
                s = (value as Media).BackdropURL;
            }
            catch (Exception)
            {
                try
                {
                    s = (value as MovieEntry).Movie.BackdropURL;
                }
                catch (Exception)
                {
                    try
                    {
                        s = (value as SeriesEntry).Series.BackdropURL;
                    }
                    catch (Exception)
                    {

                    }
                }
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
            string s = value as string;

            if (s == null)
                return null;

            return new BitmapImage(new Uri("https://image.tmdb.org/t/p/w780" + s, UriKind.Absolute));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
