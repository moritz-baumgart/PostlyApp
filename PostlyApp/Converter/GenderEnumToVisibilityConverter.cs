using PostlyApp.Enums;
using System.Globalization;

namespace PostlyApp.Converter
{
    /// <summary>
    /// This converter returns a boolean value of false when given Gender.NoAnswer.
    /// This can be used the disable elements that should not show when no gender is selected.
    /// </summary>
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
