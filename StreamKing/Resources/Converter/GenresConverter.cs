using StreamKing.Data.Media;
using System;
using System.Collections.Generic;
using System.Windows.Data;

namespace StreamKing.Resources.Converter
{
    public class GenresConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            List<Genre>? s = null;
            if(value.GetType() == typeof(Media) || value.GetType() == typeof(Movie) || value.GetType() == typeof(Series))
            {
                s = (value as Media).Genres;

            } else if (value.GetType() == typeof(MovieEntry))
            {
                s = (value as MovieEntry).Movie.Genres;

            }
            else if (value.GetType() == typeof(SeriesEntry))
            {
                s = (value as SeriesEntry).Series.Genres;
            }

            return s;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
