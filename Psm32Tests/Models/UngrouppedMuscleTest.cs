using Psm32.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psm32Tests.Models;

public class UngrouppedMuscleTest
{
    [Fact]
    public void Compare_Equal()
    {
        UngrouppedMuscle muscleGroup1 = new(new Muscle(4, 'C'))
        {
            ContractionStartTime = new TimeOnly(hour: 1, minute: 30),
            ContractionRunTime = new TimeSpan(hours: 0, minutes: 30, seconds: 0),
            ContractionRampUpTime = new TimeSpan(hours: 0, minutes: 1, seconds: 3),
            ContractionRampDownTime = new TimeSpan(hours: 0, minutes: 10, seconds: 40),
        };

        UngrouppedMuscle muscleGroup2 = new(new Muscle(4, 'C'))
        {
            ContractionStartTime = new TimeOnly(hour: 1, minute: 30),
            ContractionRunTime = new TimeSpan(hours: 0, minutes: 30, seconds: 0),
            ContractionRampUpTime = new TimeSpan(hours: 0, minutes: 1, seconds: 3),
            ContractionRampDownTime = new TimeSpan(hours: 0, minutes: 10, seconds: 40),
        };

        Assert.True(muscleGroup1.Equals(muscleGroup2));
    }

    [Fact]
    public void Compare_NotEqual()
    {
        UngrouppedMuscle muscleGroup1 = new(new Muscle(3, 'B'))
        {
            ContractionStartTime = new TimeOnly(hour: 1, minute: 30),
            ContractionRunTime = new TimeSpan(hours: 0, minutes: 30, seconds: 0),
            ContractionRampUpTime = new TimeSpan(hours: 0, minutes: 1, seconds: 3),
            ContractionRampDownTime = new TimeSpan(hours: 0, minutes: 10, seconds: 40),
        };

        UngrouppedMuscle muscleGroup2 = new(new Muscle(3, 'B'))
        {
            ContractionStartTime = new TimeOnly(hour: 1, minute: 30),
            ContractionRunTime = new TimeSpan(hours: 0, minutes: 30, seconds: 0),
            ContractionRampUpTime = new TimeSpan(hours: 0, minutes: 1, seconds: 4),
            ContractionRampDownTime = new TimeSpan(hours: 0, minutes: 10, seconds: 40),
        };

        UngrouppedMuscle muscleGroup3 = new(new Muscle(6, 'A'))
        {
            ContractionStartTime = new TimeOnly(hour: 1, minute: 30),
            ContractionRunTime = new TimeSpan(hours: 0, minutes: 30, seconds: 0),
            ContractionRampUpTime = new TimeSpan(hours: 0, minutes: 1, seconds: 3),
            ContractionRampDownTime = new TimeSpan(hours: 0, minutes: 10, seconds: 40),
        };

        Assert.False(muscleGroup1.Equals(muscleGroup2));
        Assert.False(muscleGroup1.Equals(muscleGroup3));
    }
}
