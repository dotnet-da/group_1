using StreamKing.Data.Accounts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace StreamKing.Resources.Converter
{
    public class RuntimeFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "";
            }
            var runtimeInMinutes = (int)value;
            string runtimeString = "";
            if(runtimeInMinutes >= 60)
            {
                runtimeString = $"{runtimeInMinutes / 60}h {runtimeInMinutes % 60}min";
            }
            else
            {
                runtimeString = $"{runtimeInMinutes % 60}min";
            }

            return runtimeString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
