using System;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace StreamKing.Resources
{
    public class BackdropImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string s = value as string;

            if (s == null)
                return null;

            return new BitmapImage(new Uri("https://image.tmdb.org/t/p/w500" + s, UriKind.Absolute));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
