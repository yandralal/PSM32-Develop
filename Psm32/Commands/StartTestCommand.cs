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

public class StartTestCommand : CommandBase
{
    public StartTestCommand(MuscleTestViewModel muscleTestViewModel, DeviceStore deviceStore, IQSUMessageSender messageSender)
    {
        _muscleTestViewModel = muscleTestViewModel;
        _deviceStore = deviceStore;
        _messageSender = messageSender;
    }
    public override void Execute(object? parameter)
    {
        if (_deviceStore.CurrentMuscleTest == null)
        {
            throw new InvalidOperationException("Cannot execute Start Test command when CurrentMuscleTest is null");
        }

        foreach (var testUnit in _deviceStore.CurrentMuscleTest)
        {
            if (testUnit.Status != UnitStatus.Ok)
            {
                continue;
            }
            if (testUnit.ChannelA.AvailableForCommand())
            {
                StartChannel(testUnit.ChannelA);
            }
            if (testUnit.ChannelB.AvailableForCommand())
            {
                StartChannel(testUnit.ChannelB);
            }
            if (testUnit.ChannelC.AvailableForCommand())
            {
                StartChannel(testUnit.ChannelC);
            }
            if (testUnit.ChannelD.AvailableForCommand())
            {
                StartChannel(testUnit.ChannelD);
            }
        }
        _muscleTestViewModel.StartTest();
    }

    private void StartChannel(Muscle muscle)
    {
        if (_muscleTestViewModel.RampUpEnabled)
        {
            var rampUpPeriod = (int)_muscleTestViewModel.RampUpDuration.TotalSeconds * 10;
            _messageSender.ChannelSetRamp(muscle.UnitId, muscle.Letter, 100, rampUpPeriod);
        }

        _messageSender.ChannelSetPulse(muscle.UnitId, muscle.Letter, (int)muscle.MuscleConfig.AmpPos * 10,
            (int)muscle.MuscleConfig.AmpNeg * 10, (int)muscle.MuscleConfig.PwPos * 10, (int)muscle.MuscleConfig.PwNeg * 10, muscle.MuscleConfig.Freq);
        _messageSender.ChannelGoRun(muscle.UnitId, muscle.Letter);
    }

    private readonly MuscleTestViewModel _muscleTestViewModel;
    private readonly DeviceStore _deviceStore;
    private readonly IQSUMessageSender _messageSender;
}
