using StreamKing.Data.Media;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using System.Windows.Data;

namespace StreamKing.Resources.Converter
{
    public class MediaRatingVisibilityEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            if(value.GetType() == typeof(Movie))
            {
                return (int.Parse((string)parameter) <= Math.Ceiling((decimal)((value as Movie).Rating/2)))?Visibility.Collapsed:Visibility.Visible;
            } else if(value.GetType() == typeof(MovieEntry))
            {
                return (int.Parse((string)parameter) <= Math.Ceiling((decimal)((value as MovieEntry).Movie.Rating / 2))) ? Visibility.Collapsed : Visibility.Visible;
            }
            else
            {
                return Visibility.Visible;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MediaRatingVisibilityFullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            if (value.GetType() == typeof(Movie))
            {
                return (int.Parse((string)parameter) <= Math.Ceiling((decimal)((value as Movie).Rating / 2))) ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (value.GetType() == typeof(MovieEntry))
            {
                return (int.Parse((string)parameter) <= Math.Ceiling((decimal)((value as MovieEntry).Movie.Rating / 2))) ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                return Visibility.Collapsed;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

        public class RatingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            if (value.GetType() == typeof(Movie))
            {
                return Math.Round((decimal)((value as Movie).Rating / 2), 1);
            }
            else if (value.GetType() == typeof(MovieEntry))
            {
                return Math.Round((decimal)((value as MovieEntry).Movie.Rating / 2), 1);
            }
            else
            {
                return 0f;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
