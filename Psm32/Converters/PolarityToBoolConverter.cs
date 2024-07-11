using Psm32.Models;
using System;
using System.Windows.Data;

namespace Psm32.Converters;

public class PolarityToBoolConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
  {
        return (string)parameter == (string)value;
    }

  public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
  {
        return (bool)value ? parameter : Binding.DoNothing;
  }
}