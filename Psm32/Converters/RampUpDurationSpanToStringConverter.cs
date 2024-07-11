using Psm32.Models;
using System;
using System.Windows.Data;

namespace Psm32.Converters;

public class RampUpDurationSpanToStringConverter : IValueConverter
{

    private const string RAMPUP_TIME_FORMAT = @"ss\.f";
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        TimeSpan span = (TimeSpan)value;
        return span.ToString(RAMPUP_TIME_FORMAT);
    }

  public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
  {
        var result = TimeSpan.TryParseExact((string)value, RAMPUP_TIME_FORMAT, null, out var duration);

        return result ? duration : Binding.DoNothing;
  }
}