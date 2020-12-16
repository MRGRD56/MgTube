using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace MgTube.Converters
{
    public class SecondsToTimeSpanStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var obj = (double) value;
            var timeSpan = TimeSpan.FromSeconds(obj);
            if (timeSpan.Hours == 0)
            {
                return timeSpan.ToString(@"m\:ss");
            }
            else
            {
                return timeSpan.ToString(@"h\:mm\:ss");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
