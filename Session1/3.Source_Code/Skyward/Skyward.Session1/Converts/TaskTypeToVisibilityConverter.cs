using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Skyward.Session1.Converts;

public class TaskTypeToVisibilityConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string and "Portion Warehouse")
            return true;

        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}