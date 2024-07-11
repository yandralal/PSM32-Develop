using Psm32.Models;
using Psm32.Services;
using Psm32.Stores;
using Psm32.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Text;

namespace Psm32.Commands;

public class PauseTestCommand : CommandBase
{
    public PauseTestCommand(
        MuscleTestViewModel muscleTestViewModel, 
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
            throw new InvalidOperationException("Cannot execute Pause Test command when CurrentMuscleTest is null");
        }

        foreach (var testUnit in _deviceStore.CurrentMuscleTest)
        {
            if (testUnit.Status != UnitStatus.Ok)
            {
                continue;
            }
            if (testUnit.ChannelA.AvailableForCommand())
            {
                PauseChannel(testUnit.ChannelA);
            }
            if (testUnit.ChannelB.AvailableForCommand())
            {
                PauseChannel(testUnit.ChannelB);
            }
            if (testUnit.ChannelC.AvailableForCommand())
            {
                PauseChannel(testUnit.ChannelC);
            }
            if (testUnit.ChannelD.AvailableForCommand())
            {
                PauseChannel(testUnit.ChannelD);
            }
        }
        _muscleTestViewModel.PauseTest();
        
    }

    private void PauseChannel(Muscle channel)
    {
        _messageSender.ChannelGoOff(channel.UnitId, channel.Letter);
    }

    private readonly MuscleTestViewModel _muscleTestViewModel;
    private readonly DeviceStore _deviceStore;
    private readonly IQSUMessageSender _messageSender;
}
