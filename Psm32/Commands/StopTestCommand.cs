using Psm32.Models;
using Psm32.Services;
using Psm32.Stores;
using Psm32.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psm32.Commands;

public class StopTestCommand : CommandBase
{
    public StopTestCommand
        (MuscleTestViewModel muscleTestViewModel, 
        DeviceStore deviceStore, 
        IQSUMessageSender messageSender)
    {
        _muscleTestViewModel = muscleTestViewModel;
        _deviceStore = deviceStore;
        _messageSender = messageSender;
    }
    public override void Execute(object? parameter)
    {
        if (_deviceStore.CurrentMuscleTest == null)
        {
            throw new InvalidOperationException("Cannot execute Stop Test command when CurrentMuscleTest is null");
        }

        foreach (var testUnit in _deviceStore.CurrentMuscleTest)
        {
            if (testUnit.Status != UnitStatus.Ok)
            {
                continue;
            }
            if (testUnit.ChannelA.AvailableForCommand())
            {
                StopChannel(testUnit.ChannelA);
            }
            if (testUnit.ChannelB.AvailableForCommand())
            {
                StopChannel(testUnit.ChannelB);
            }
            if (testUnit.ChannelC.AvailableForCommand())
            {
                StopChannel(testUnit.ChannelC);
            }
            if (testUnit.ChannelD.AvailableForCommand())
            {
                StopChannel(testUnit.ChannelD);
            }
        }
        _muscleTestViewModel.StopTest();
    }

    private void StopChannel(Muscle channel)
    {
        _messageSender.ChannelGoOff(channel.UnitId, channel.Letter);
    }

    private readonly DeviceStore _deviceStore;
    private readonly IQSUMessageSender _messageSender;
    private readonly MuscleTestViewModel _muscleTestViewModel;
}
