using Psm32.Models;
using System;
using System.Windows.Data;

namespace Psm32.Converters;

public class UnitStatusToChannelColorConverter: IValueConverter
{

  public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
  {
        UnitStatus status = (UnitStatus)value;

        if (status == UnitStatus.NA)
            return "#696969";

        return parameter;
  }

  public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
  {
    throw new NotImplementedException();
  }

}
