using Psm32.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psm32.ViewModels;

public class QSUnitViewModel : ViewModelBase
{
    public int ID => _unit.ID;
    public UnitStatus Status => _unit.Status;

    public string ChannelAId => _unit.ChannelA.ID;
    public string ChannelBId => _unit.ChannelB.ID;
    public string ChannelCId => _unit.ChannelC.ID;
    public string ChannelDId => _unit.ChannelD.ID;

    public MuscleStatus ChannelAStatus => _unit.ChannelA.Status;
    public MuscleStatus ChannelBStatus => _unit.ChannelB.Status;
    public MuscleStatus ChannelCStatus => _unit.ChannelC.Status;
    public MuscleStatus ChannelDStatus => _unit.ChannelD.Status;



    public QSUnitViewModel(QSUnit unit)
    {
        _unit=unit;
    }

    private readonly QSUnit _unit;
}
