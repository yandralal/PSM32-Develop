using System;
using System.Windows.Data;

namespace Psm32.Converters;

public class DebugConverter : IValueConverter
{

  public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
  {
    // Set breakpoint here
    return value;
  }

  public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
  {
    // Set breakpoint here
    return value;
  }

}
