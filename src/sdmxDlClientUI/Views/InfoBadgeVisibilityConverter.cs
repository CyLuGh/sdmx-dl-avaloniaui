using Avalonia.Data;
using Avalonia.Data.Converters;
using sdmxDlClient.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdmxDlClientUI.Views
{
    public class InfoBadgeVisibilityConverter : IValueConverter
    {
        public static readonly InfoBadgeVisibilityConverter Instance = new();

        public object? Convert( object? value , Type targetType , object? parameter , CultureInfo culture )
        {
            if ( value is MessageKind messageKind && parameter is MessageKind targetKind )
            {
                return messageKind == targetKind;
            }

            return new BindingNotification( new InvalidCastException() , BindingErrorType.Error );
        }

        public object? ConvertBack( object? value , Type targetType , object? parameter , CultureInfo culture )
        {
            throw new NotSupportedException();
        }
    }
}