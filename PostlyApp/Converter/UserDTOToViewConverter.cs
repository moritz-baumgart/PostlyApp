using PostlyApp.Models.DTOs;
using System.Globalization;

namespace PostlyApp.Converter
{
    /// <summary>
    /// This converter generates a formatted string containing the users Displayname and Username.
    /// If there is no Displayname set it only formats the Username.
    /// </summary>
    internal class UserDTOToViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var user = (UserDTO?)value;
            if (user?.DisplayName != null && user.DisplayName.Length > 0)
            {
                return $"{user.DisplayName} (@{user.Username})";
            }
            else
            {
                return $"@{user?.Username}";
            }
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
