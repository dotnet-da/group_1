using StreamKing.Data.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace StreamKing.Resources.Converter
{
    public class SeasonsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            List<Season>? s = null;
            if(value.GetType() == typeof(Series))
            {
                s = (value as Series).Seasons.OrderBy(s => s.Number).ToList();

            } else if (value.GetType() == typeof(SeriesEntry))
            {
                if((value as SeriesEntry).Series.Seasons is not null && (value as SeriesEntry).Series.Seasons.Count > 0)
                {
                    s = (value as SeriesEntry).Series.Seasons.OrderBy(s => s.Number).ToList();

                }
            }

            return s;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    
        public class SeasonsVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;

            if (value.GetType() == typeof(Series))
            {
                return ((value as Series).Seasons.Count > 0) ? Visibility.Visible : Visibility.Collapsed;

            }
            else if (value.GetType() == typeof(SeriesEntry))
            {
                if ((value as SeriesEntry).Series.Seasons is not null)
                {
                    return ((value as SeriesEntry).Series.Seasons.Count > 0) ? Visibility.Visible : Visibility.Collapsed;
                }
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
