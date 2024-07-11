using Psm32.Models;
using System;
using System.Windows.Data;

namespace Psm32.Converters;

public class UnitStatusToChannelIdColorConverter : IValueConverter
{

  public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
  {
        UnitStatus status = (UnitStatus)value;

        if (status == UnitStatus.NA)
            return "Gray";

        return "Black";
  }

  public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
  {
    throw new NotImplementedException();
  }

}
