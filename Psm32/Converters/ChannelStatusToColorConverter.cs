using Psm32.Models;
using System;
using System.Windows.Data;

namespace Psm32.Converters;

public class ChannelStatusToColorConverter : IValueConverter
{

  public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
  {
        MuscleStatus status = (MuscleStatus)value;

        if (status == MuscleStatus.Up)
            return "#27a045";
        if (status == MuscleStatus.Down)
            return "#f40b0b";
        if (status == MuscleStatus.NA)
            return "#a5a1a1";
        return "black";
  }

  public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
  {
    throw new NotImplementedException();
  }

}
