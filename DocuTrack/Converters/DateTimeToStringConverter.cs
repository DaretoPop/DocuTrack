using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace DocuTrack.Converters
{
    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime && dateTime != DateTime.MinValue)
            {
                // Format the date as "dd.MM.yyyy" or any desired format
                return dateTime.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
            }
            return string.Empty; // Return an empty string for null or default DateTime
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string dateString && !string.IsNullOrWhiteSpace(dateString))
            {
                // Try parsing the exact format first
                if (DateTime.TryParseExact(dateString, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
                {
                    return dateTime;
                }

                // Fallback to general parsing
                if (DateTime.TryParse(dateString, out dateTime))
                {
                    return dateTime;
                }
            }

            // If parsing fails or value is null/empty, return DateTime.MinValue
            return DateTime.MinValue;
        }
    }
}