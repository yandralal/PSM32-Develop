using Psm32.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psm32.ViewModels;

public class QSUnitChannelViewModel : ViewModelBase
{
    public string ID => _channel.ID;
    public MuscleStatus ChannelStatus => _channel.Status;

    private readonly Muscle _channel;

    public QSUnitChannelViewModel(Muscle channel)
    {
        _channel = channel;
    }

}
