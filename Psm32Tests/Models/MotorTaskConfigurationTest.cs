using Psm32.Models;

namespace Psm32Tests.Models;

public class MotorTaskConfigurationTest
{
    [Fact]
    public void Compare_Equal()
    {
        MuscleGroup muscleGroup1 = new(name: "Test Group")
        {
            ContractionStartTime = new TimeOnly(hour: 1, minute: 30),
            ContractionRunTime = new TimeSpan(hours: 0, minutes: 30, seconds: 0),
            ContractionRampUpTime = new TimeSpan(hours: 0, minutes: 1, seconds: 3),
            ContractionRampDownTime = new TimeSpan(hours: 0, minutes: 10, seconds: 40),
            Muscles = new List<Muscle>() { new (3, 'B'), new(4, 'A') }
        };

        MuscleGroup muscleGroup2 = new(name: "Test Group")
        {
            ContractionStartTime = new TimeOnly(hour: 1, minute: 30),
            ContractionRunTime = new TimeSpan(hours: 0, minutes: 30, seconds: 0),
            ContractionRampUpTime = new TimeSpan(hours: 0, minutes: 1, seconds: 3),
            ContractionRampDownTime = new TimeSpan(hours: 0, minutes: 10, seconds: 40),
            Muscles = new List<Muscle>() { new(1, 'B') }
        };

        UngrouppedMuscle muscle1 = new(new Muscle(4, 'C'))
        {
            ContractionStartTime = new TimeOnly(hour: 1, minute: 30),
            ContractionRunTime = new TimeSpan(hours: 0, minutes: 30, seconds: 0),
            ContractionRampUpTime = new TimeSpan(hours: 0, minutes: 1, seconds: 3),
            ContractionRampDownTime = new TimeSpan(hours: 0, minutes: 10, seconds: 40),
        };

        UngrouppedMuscle muscle2 = new(new Muscle(5, 'D'))
        {
            ContractionStartTime = new TimeOnly(hour: 1, minute: 30),
            ContractionRunTime = new TimeSpan(hours: 0, minutes: 30, seconds: 0),
            ContractionRampUpTime = new TimeSpan(hours: 0, minutes: 1, seconds: 3),
            ContractionRampDownTime = new TimeSpan(hours: 0, minutes: 10, seconds: 40),
        };

        MotorTaskConfiguration mtConfig1 = new()
        {
            CycleDuration = new TimeSpan(hours: 0, minutes: 24, seconds: 30),
            Cycles = 5,
            SpeedPercent = 100,
            MuscleGroups = new() { muscleGroup1 , muscleGroup2},
            UngrouppedMuscles = new () { muscle1, muscle2 },
        };

        MotorTaskConfiguration mtConfig2 = new()
        {
            CycleDuration = new TimeSpan(hours: 0, minutes: 24, seconds: 30),
            Cycles = 5,
            SpeedPercent = 100,
            MuscleGroups = new() { muscleGroup2, muscleGroup1 },
            UngrouppedMuscles = new() { muscle2, muscle1 },
        };

        Assert.True(mtConfig1.Equals(mtConfig2));

    }

    [Fact]
    public void Compare_NotEqual()
    {
        MuscleGroup muscleGroup1 = new(name: "Test Group 1")
        {
            ContractionStartTime = new TimeOnly(hour: 1, minute: 30),
            ContractionRunTime = new TimeSpan(hours: 0, minutes: 30, seconds: 0),
            ContractionRampUpTime = new TimeSpan(hours: 0, minutes: 1, seconds: 3),
            ContractionRampDownTime = new TimeSpan(hours: 0, minutes: 10, seconds: 40),
            Muscles = new List<Muscle>() { new(3, 'B'), new(4, 'A') }
        };

        MuscleGroup muscleGroup2 = new(name: "Test Group 1")
        {
            ContractionStartTime = new TimeOnly(hour: 1, minute: 30),
            ContractionRunTime = new TimeSpan(hours: 0, minutes: 30, seconds: 0),
            ContractionRampUpTime = new TimeSpan(hours: 0, minutes: 1, seconds: 3),
            ContractionRampDownTime = new TimeSpan(hours: 0, minutes: 10, seconds: 40),
            Muscles = new List<Muscle>() { new(1, 'D') }
        };

        UngrouppedMuscle muscle1 = new(new Muscle(4, 'C'))
        {
            ContractionStartTime = new TimeOnly(hour: 1, minute: 30),
            ContractionRunTime = new TimeSpan(hours: 0, minutes: 30, seconds: 0),
            ContractionRampUpTime = new TimeSpan(hours: 0, minutes: 1, seconds: 3),
            ContractionRampDownTime = new TimeSpan(hours: 0, minutes: 10, seconds: 40),
        };

        UngrouppedMuscle muscle2 = new(new Muscle(5, 'D'))
        {
            ContractionStartTime = new TimeOnly(hour: 1, minute: 30),
            ContractionRunTime = new TimeSpan(hours: 0, minutes: 30, seconds: 0),
            ContractionRampUpTime = new TimeSpan(hours: 0, minutes: 1, seconds: 3),
            ContractionRampDownTime = new TimeSpan(hours: 0, minutes: 10, seconds: 40),
        };

        MotorTaskConfiguration mtConfig1 = new()
        {
            CycleDuration = new TimeSpan(hours: 0, minutes: 24, seconds: 30),
            Cycles = 5,
            SpeedPercent = 100,
            MuscleGroups = new() { muscleGroup1, muscleGroup2 },
            UngrouppedMuscles = new() { muscle1, muscle2 },
        };

        MotorTaskConfiguration mtConfig2 = new()
        {
            CycleDuration = new TimeSpan(hours: 0, minutes: 24, seconds: 30),
            Cycles = 5,
            SpeedPercent = 100,
            MuscleGroups = new() { muscleGroup2 },
            UngrouppedMuscles = new() { muscle1, muscle2 },
        };

        Assert.False(mtConfig1.Equals(mtConfig2));

    }

    [Fact]
    public void AddMuscleGroup_NameExists_Throws()
    {
        var configuration = new MotorTaskConfiguration();
        configuration.MuscleGroups.Add(new MuscleGroup("Test Group"));

        Action action = () => configuration.AddMuscleGroup("Test Group");

        var exception = Assert.Throws<Exception>(action);

        Assert.NotNull(exception);
        Assert.Equal("Muscle Group named `Test Group` already exists", exception.Message);
    }

    [Fact]
    public void AddMuscleGroup_Success()
    {
        var configuration = new MotorTaskConfiguration();
        configuration.AddMuscleGroup("Test Group");

        Assert.Single(configuration.MuscleGroups);
        Assert.Equal("Test Group", configuration.MuscleGroups[0].Name);
    }

    [Fact]
    public void AddMusclesToGroup_Success()
    {
        var configuration = new MotorTaskConfiguration();
        configuration.AddMuscleGroup("Test Group");
        configuration.AddMusclesToGroup("Test Group", new List<Muscle> { new(1, 'A'), new(1, 'B') });

        var expected = new MuscleGroup("Test Group")
        {
            Muscles = { new(1, 'A'), new(1, 'B') }
        };

        Assert.True(expected.Equals(configuration.MuscleGroups[0]));
    }

    [Fact]
    public void AddMusclesToGroup_GroupNotFound_Throws()
    {
        var configuration = new MotorTaskConfiguration();

        Action action = () => configuration.AddMusclesToGroup("Test Group", new List<Muscle> { new(1, 'A') });

        var exception = Assert.Throws<Exception>(action);

        Assert.NotNull(exception);
        Assert.Empty(configuration.MuscleGroups);
        Assert.Equal("Muscle Group named `Test Group` not found", exception.Message);
    }


    [Fact]
    public void AddMusclesToGroup_MuscleIdInAnotherGroup_Throws()
    {
        var configuration = new MotorTaskConfiguration();
        configuration.AddMuscleGroup("Group1");
        configuration.AddMusclesToGroup("Group1", new List<Muscle> { new(1, 'A'), new(1, 'B') });
        configuration.AddMuscleGroup("Test Group");

        Action action = () => configuration.AddMusclesToGroup("Test Group", new List<Muscle> { new(1, 'A') });

        var exception = Assert.Throws<Exception>(action);

        Assert.NotNull(exception);
        Assert.Equal("Muscle `1A` is already in another group", exception.Message);
    }
}
