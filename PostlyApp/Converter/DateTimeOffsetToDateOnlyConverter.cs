using System.Globalization;

namespace PostlyApp.Converter
{
    /// <summary>
    /// This converter converts a DateTimeOffset to formatted string containing only the date.
    /// </summary>
    internal class DateTimeOffsetToDateOnlyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is DateTimeOffset date)
            {
                return date.LocalDateTime.ToString("d");
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
