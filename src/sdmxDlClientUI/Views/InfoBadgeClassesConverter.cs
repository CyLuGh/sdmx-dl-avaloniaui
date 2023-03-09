using Avalonia.Data;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdmxDlClientUI.Views
{
    public class InfoBadgeClassesConverter : IValueConverter
    {
        public static readonly InfoBadgeClassesConverter Instance = new();

        public object? Convert( object? value , Type targetType , object? parameter , CultureInfo culture )
        {
            return new BindingNotification( new InvalidCastException() , BindingErrorType.Error );
        }

        public object? ConvertBack( object? value , Type targetType , object? parameter , CultureInfo culture )
        {
            throw new NotSupportedException();
        }
    }
}