using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;

namespace MgTube.Converters
{
    public class PlayButtonIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // the type of value is bool
            return (bool) value ? new PackIcon { Kind = PackIconKind.Pause } : new PackIcon { Kind = PackIconKind.Play };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
