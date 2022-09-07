using StreamKing.Data.Media;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StreamKing.Resources.Converter
{
    public class MediaTypeMovieVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Collapsed;
            }
            try
            {
                ((Media)value).GetType();
            }
            catch (Exception)
            {
                return Visibility.Collapsed;
            }

            return (((Media)value).GetType() == typeof(Movie)) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MediaTypeSeriesVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Collapsed;
            }
            try
            {
                ((Media)value).GetType();
            }
            catch (Exception)
            {
                return Visibility.Collapsed;
            }

            return (((Media)value).GetType() == typeof(Series)) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
