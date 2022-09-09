using StreamKing.Data.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StreamKing.Resources.Converter
{
    public class StreamingAvailabilityVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Visible;
            }

            List<StreamingInfo> s = new List<StreamingInfo>();

            if (value.GetType() == typeof(Media) || value.GetType() == typeof(Movie) || value.GetType() == typeof(Series))
            {
                s = (value as Media).StreamingInfos;

            }
            else if (value.GetType() == typeof(MovieEntry))
            {
                s = (value as MovieEntry).Movie.StreamingInfos;

            }
            else if (value.GetType() == typeof(SeriesEntry))
            {
                s = (value as SeriesEntry).Series.StreamingInfos;
            }else
            {
                return Visibility.Visible;
            }

            if (s == null)
            {
                return Visibility.Visible;
            }

            return (s.Count > 0) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
