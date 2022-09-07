using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using StreamKing.Data.Accounts;

namespace StreamKing.Resources
{
    public class AccountTypeVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
            {
                return Visibility.Collapsed;
            }
            return ((Account)value).Type == AccountType.Admin ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
