using Avalonia;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace iDecryptIt.Converters;

public class ConcatConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        $"{value}{parameter}";

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        AvaloniaProperty.UnsetValue;
}
