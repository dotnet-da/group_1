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
            try
            {
                s = (value as Media).Genres;
            }
            catch (Exception)
            {
                try
                {
                    s = (value as MovieEntry).Movie.Genres;
                }
                catch (Exception)
                {
                    try
                    {
                        s = (value as SeriesEntry).Series.Genres;
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            return s;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
