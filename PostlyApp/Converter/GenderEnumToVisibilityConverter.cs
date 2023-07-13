using PostlyApp.Enums;
using System.Globalization;

namespace PostlyApp.Converter
{
    internal class GenderEnumToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Gender gender)
            {
                return gender != Gender.NoAnswer;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
