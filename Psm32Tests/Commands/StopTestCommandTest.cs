using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Moq;
using Psm32.Commands;
using Psm32.Models;
using Psm32.Services;
using Psm32.Stores;
using Psm32.ViewModels;
using Psm32Tests.Fixtures;

namespace Psm32Tests.Commands;

public class StopTestCommandTest
{
    private Mock<INavigationService> _mockNavigationService;
    private Mock<IQSUMessageSender> _mockMessageSender;
    private DeviceStoreFixture _deviceStoreFixture;

    public StopTestCommandTest()
    {
        _mockNavigationService = new Mock<INavigationService>();
        _mockMessageSender = new Mock<IQSUMessageSender>();
        
        _deviceStoreFixture = new DeviceStoreFixture();


        _mockMessageSender.Setup(x => x.ChannelGoOff(It.IsAny<int>(), It.IsAny<char?>()))
            .Verifiable();
       }
    [Fact]
    public async void Execute_OneAvailableMuscle()
    {
        var unit1 = new QSUnit(1);

        var muscle1A = new Muscle(1, 'A')
        {
            Status = MuscleStatus.Up,
            MuscleConfig = new MuscleConfig()
            {
                Enabled = true,
            }
        };

        var muscle1B = new Muscle(1, 'B')
        {
            Status = MuscleStatus.Up,
            MuscleConfig = new MuscleConfig()
            {
                Enabled = false,
            }
        };

        var muscle1C = new Muscle(1, 'C')
        {
            Status = MuscleStatus.NA,
            MuscleConfig = new MuscleConfig()
            {
                Enabled = true,
            }
        };

        MuscleGroup muscleGroup1 = new(name: "Test Group")
        {
            ContractionStartTime = new TimeOnly(hour: 1, minute: 30),
            ContractionRunTime = new TimeSpan(hours: 0, minutes: 30, seconds: 0),
            ContractionRampUpTime = new TimeSpan(hours: 0, minutes: 1, seconds: 3),
            ContractionRampDownTime = new TimeSpan(hours: 0, minutes: 10, seconds: 40),
            Muscles = [muscle1A, muscle1B]
        };

        UngrouppedMuscle muscle1 = new(muscle1C)
        {
            ContractionStartTime = new TimeOnly(hour: 1, minute: 30),
            ContractionRunTime = new TimeSpan(hours: 0, minutes: 30, seconds: 0),
            ContractionRampUpTime = new TimeSpan(hours: 0, minutes: 1, seconds: 3),
            ContractionRampDownTime = new TimeSpan(hours: 0, minutes: 10, seconds: 40),
        };


        MotorTaskConfiguration mtConfig = new()
        {
            CycleDuration = new TimeSpan(hours: 0, minutes: 24, seconds: 30),
            Cycles = 5,
            SpeedPercent = 100,
            MuscleGroups = [muscleGroup1],
            UngrouppedMuscles = [muscle1],
        };



        var units = new List<QSUnit>()
        {
            unit1
        };

        _deviceStoreFixture.DeviceFixture.MockQsuScanner.Setup(x => x.Scan()).Returns(Task.FromResult(units));
        var currentMotorTask = new MotorTask(Guid.NewGuid(), "mt", "1234", mtConfig, DateTime.Now, DateTime.Now);

        _deviceStoreFixture.DeviceStore.SetCurrentMotorTask(currentMotorTask);
        await _deviceStoreFixture.DeviceStore.ScanQSUs();
        var vm = MuscleTestViewModel.LoadViewModel(_deviceStoreFixture.DeviceStore, _mockNavigationService.Object, _mockMessageSender.Object);

        var stopTestCommand = new StopTestCommand(vm, _deviceStoreFixture.DeviceStore, _mockMessageSender.Object);


        stopTestCommand.Execute(null);

        _mockMessageSender.Verify(m => m.ChannelGoOff(It.IsAny<int>(), It.IsAny<char?>()), Times.Once);
    }


    [Fact]
    public async void Execute_NoAvailableMuscles()
    {
        var units = new List<QSUnit>()
        {
            new(1),
            new(2)
        };

        var muscle1A = new Muscle(1, 'A')
        {
            Status = MuscleStatus.Up,
            MuscleConfig = new MuscleConfig()
            {
                Enabled = false,
            }
        };

        var muscle1B = new Muscle(1, 'B')
        {
            Status = MuscleStatus.Down,
            MuscleConfig = new MuscleConfig()
            {
                Enabled = true,
            }
        };

        var muscle2C = new Muscle(2, 'C')
        {
            Status = MuscleStatus.Up,
            MuscleConfig = new MuscleConfig()
            {
                Enabled = false
            }
        };

        MuscleGroup muscleGroup1 = new(name: "Test Group")
        {
            ContractionStartTime = new TimeOnly(hour: 1, minute: 30),
            ContractionRunTime = new TimeSpan(hours: 0, minutes: 30, seconds: 0),
            ContractionRampUpTime = new TimeSpan(hours: 0, minutes: 1, seconds: 3),
            ContractionRampDownTime = new TimeSpan(hours: 0, minutes: 10, seconds: 40),
            Muscles = [muscle1A, muscle1B]
        };

        UngrouppedMuscle muscle1 = new(muscle2C)
        {
            ContractionStartTime = new TimeOnly(hour: 1, minute: 30),
            ContractionRunTime = new TimeSpan(hours: 0, minutes: 30, seconds: 0),
            ContractionRampUpTime = new TimeSpan(hours: 0, minutes: 1, seconds: 3),
            ContractionRampDownTime = new TimeSpan(hours: 0, minutes: 10, seconds: 40),
        };

        MotorTaskConfiguration mtConfig = new()
        {
            CycleDuration = new TimeSpan(hours: 0, minutes: 24, seconds: 30),
            Cycles = 5,
            SpeedPercent = 100,
            MuscleGroups = [muscleGroup1],
            UngrouppedMuscles = [muscle1],
        };

        var currentMotorTask = new MotorTask(Guid.NewGuid(), "mt", "1234", mtConfig, DateTime.Now, DateTime.Now);
        _deviceStoreFixture.DeviceStore.SetCurrentMotorTask(currentMotorTask);

        _deviceStoreFixture.DeviceFixture.MockQsuScanner.Setup(x => x.Scan()).Returns(Task.FromResult(units));

        await _deviceStoreFixture.DeviceStore.ScanQSUs();
        var vm = MuscleTestViewModel.LoadViewModel(_deviceStoreFixture.DeviceStore, _mockNavigationService.Object, _mockMessageSender.Object);
        vm.RampUpEnabled = true;

        var stopTestCommand = new StopTestCommand(vm, _deviceStoreFixture.DeviceStore, _mockMessageSender.Object);

        stopTestCommand.Execute(null);
        _mockMessageSender.Verify(m => m.ChannelGoOff(It.IsAny<int>(), It.IsAny<char?>()), Times.Never);
    }


    [Fact]
    public async void Execute_NoAvailableUnit()
    {
        var units = new List<QSUnit>()
        {
            new(1)
            {
                Status = UnitStatus.Error
            }
        };

        var muscle1A = new Muscle(1, 'A')
        {
            Status = MuscleStatus.Up,
            MuscleConfig = new MuscleConfig()
            {
                Enabled = true,
            }
        };

        var muscle1B = new Muscle(1, 'B')
        {
            Status = MuscleStatus.Up,
            MuscleConfig = new MuscleConfig()
            {
                Enabled = true,
            }
        };

        MuscleGroup muscleGroup1 = new(name: "Test Group")
        {
            ContractionStartTime = new TimeOnly(hour: 1, minute: 30),
            ContractionRunTime = new TimeSpan(hours: 0, minutes: 30, seconds: 0),
            ContractionRampUpTime = new TimeSpan(hours: 0, minutes: 1, seconds: 3),
            ContractionRampDownTime = new TimeSpan(hours: 0, minutes: 10, seconds: 40),
            Muscles = [muscle1A, muscle1B]
        };

        MotorTaskConfiguration mtConfig = new()
        {
            CycleDuration = new TimeSpan(hours: 0, minutes: 24, seconds: 30),
            Cycles = 5,
            SpeedPercent = 100,
            MuscleGroups = [muscleGroup1],
        };

        var currentMotorTask = new MotorTask(Guid.NewGuid(), "mt", "1234", mtConfig, DateTime.Now, DateTime.Now);
        _deviceStoreFixture.DeviceStore.SetCurrentMotorTask(currentMotorTask);

        _deviceStoreFixture.DeviceFixture.MockQsuScanner.Setup(x => x.Scan()).Returns(Task.FromResult(units));

        await _deviceStoreFixture.DeviceStore.ScanQSUs();
        var vm = MuscleTestViewModel.LoadViewModel(_deviceStoreFixture.DeviceStore, _mockNavigationService.Object, _mockMessageSender.Object);
        vm.RampUpEnabled = true;

        var stopTestCommand = new StopTestCommand(vm, _deviceStoreFixture.DeviceStore, _mockMessageSender.Object);

        stopTestCommand.Execute(null);
        _mockMessageSender.Verify(m => m.ChannelGoOff(It.IsAny<int>(), It.IsAny<char?>()), Times.Never);
    }
}

