using Psm32.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psm32Tests.Models;

public class MuscleGroupTest
{
    [Fact]
    public void Compare_Equal()
    {
        MuscleGroup muscle1 = new(name: "Test Group")
        {
            ContractionStartTime = new TimeOnly(hour: 1, minute: 30),
            ContractionRunTime = new TimeSpan(hours: 0, minutes: 30, seconds: 0),
            ContractionRampUpTime = new TimeSpan(hours: 0, minutes: 1, seconds: 3),
            ContractionRampDownTime = new TimeSpan(hours: 0, minutes: 10, seconds: 40),
            Muscles = [ new(3, 'B'), new(4, 'A') ]
        };


        MuscleGroup muscle2 = new(name: "Test Group")
        {
            ContractionStartTime = new TimeOnly(hour: 1, minute: 30),
            ContractionRunTime = new TimeSpan(hours: 0, minutes: 30, seconds: 0),
            ContractionRampUpTime = new TimeSpan(hours: 0, minutes: 1, seconds: 3),
            ContractionRampDownTime = new TimeSpan(hours: 0, minutes: 10, seconds: 40),
            Muscles =[ new(3, 'B'), new(4, 'A') ]
        };

        MuscleGroup muscle3 = new(name: "Test Group")
        {
            ContractionStartTime = new TimeOnly(hour: 1, minute: 30),
            ContractionRunTime = new TimeSpan(hours: 0, minutes: 30, seconds: 0),
            ContractionRampUpTime = new TimeSpan(hours: 0, minutes: 1, seconds: 3),
            ContractionRampDownTime = new TimeSpan(hours: 0, minutes: 10, seconds: 40),
            Muscles = [ new(4, 'A'), new(3, 'B') ]
        };

        Assert.True(muscle1.Equals(muscle2));
        Assert.True(muscle2.Equals(muscle3));

    }

    [Fact]
    public void Compare_NotEqual()
    {
        MuscleGroup muscle1 = new(name: "Test Group")
        {
            ContractionStartTime = new TimeOnly(hour: 1, minute: 30),
            ContractionRunTime = new TimeSpan(hours: 0, minutes: 30, seconds: 0),
            ContractionRampUpTime = new TimeSpan(hours: 0, minutes: 1, seconds: 3),
            ContractionRampDownTime = new TimeSpan(hours: 0, minutes: 10, seconds: 40),
            Muscles = new List<Muscle>() { new Muscle(3, 'B'), new Muscle(4, 'A') }
        };

        MuscleGroup muscle2 = new(name: "Test Group")
        {
            ContractionStartTime = new TimeOnly(hour: 1, minute: 30),
            ContractionRunTime = new TimeSpan(hours: 0, minutes: 30, seconds: 0),
            ContractionRampUpTime = new TimeSpan(hours: 0, minutes: 1, seconds: 4),
            ContractionRampDownTime = new TimeSpan(hours: 0, minutes: 10, seconds: 40),
            Muscles = new List<Muscle>() { new Muscle(3, 'B'), new Muscle(4, 'A') }
        };

        MuscleGroup muscle3 = new(name: "Test Group")
        {
            ContractionStartTime = new TimeOnly(hour: 1, minute: 30),
            ContractionRunTime = new TimeSpan(hours: 0, minutes: 30, seconds: 0),
            ContractionRampUpTime = new TimeSpan(hours: 0, minutes: 1, seconds: 3),
            ContractionRampDownTime = new TimeSpan(hours: 0, minutes: 10, seconds: 40),
            Muscles = new List<Muscle>() { new Muscle(4, 'A') }
        };

        MuscleGroup muscle4 = new(name: "Test Group4")
        {
            ContractionStartTime = new TimeOnly(hour: 1, minute: 30),
            ContractionRunTime = new TimeSpan(hours: 0, minutes: 30, seconds: 0),
            ContractionRampUpTime = new TimeSpan(hours: 0, minutes: 1, seconds: 3),
            ContractionRampDownTime = new TimeSpan(hours: 0, minutes: 10, seconds: 40),
            Muscles = new List<Muscle>() { new Muscle(3, 'B'), new Muscle(4, 'A') }
        };

        Assert.False(muscle1.Equals(muscle2));
        Assert.False(muscle1.Equals(muscle3));
        Assert.False(muscle1.Equals(muscle4));
    }
}
