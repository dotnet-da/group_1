using StreamKing.Data.Media;
using System;
using System.Collections.Generic;
using System.Windows.Data;

namespace StreamKing.Resources.Converter
{
    public class StreamingInfosConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            List<StreamingInfo>? s = null;
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
            }

            return s;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
