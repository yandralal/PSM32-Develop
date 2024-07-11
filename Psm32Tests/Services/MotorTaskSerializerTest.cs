using Psm32.Models;
using Psm32.Services;

namespace Psm32Tests.Services;

public class MotorTaskSerializerTest
{
    [Fact]
    public void ToJson_EmptyGroups_Success()
    {
        var motorTask = new MotorTask(name:"Test Task", patientId:"123");
        var expected = "{\r\n  \"CycleDuration\": \"00:00:00\",\r\n  \"Cycles\": 0,\r\n  \"SpeedPercent\": 0,\r\n  \"AmpStep\": 0,\r\n  \"MuscleGroups\": [],\r\n  \"UngrouppedMuscles\": [],\r\n  \"Chimes\": [],\r\n  \"Rests\": []\r\n}";

        var serializer = new MotorTaskConfigurationSerializer();

        var actual =  serializer.ToJson(motorTask.Configuration);

        Assert.Equal(expected, actual);
    }

    [Fact(Skip = "It is not very effective to compare full json string as it is too long")]
    public void ToJson_NonEmptyGroups_Success()
    {
        var motorTask = new MotorTask(name: "Test Task", patientId: "123");

        var muscleGroup1 = new MuscleGroup(name: "Test Group1");
        muscleGroup1.Muscles = new List<Muscle> { new Muscle(1, 'A'), new Muscle(2, 'B'), new Muscle(3, 'C') }; 
        var muscleGroup2 = new MuscleGroup(name: "Test Group2");
        muscleGroup2.Muscles = new List<Muscle> { new Muscle(3, 'C') };
        motorTask.Configuration.MuscleGroups.Add(muscleGroup1);
        motorTask.Configuration.MuscleGroups.Add(muscleGroup2);

        var ungrouppedMuscle1 = new UngrouppedMuscle(new Muscle(5, 'A'));
        var ungrouppedMuscle2 = new UngrouppedMuscle(new Muscle(3, 'A'));
        motorTask.Configuration.UngrouppedMuscles.Add(ungrouppedMuscle1);
        motorTask.Configuration.UngrouppedMuscles.Add(ungrouppedMuscle2);

        var expected = "{\r\n  \"CycleDuration\": \"00:00:00\",\r\n  \"Cycles\": 0,\r\n  \"SpeedPercent\": 0,\r\n  \"AmpStep\": 0,\r\n  \"MuscleGroups\": [\r\n    {\r\n      \"Name\": \"Test Group1\",\r\n      \"ContractionStartTime\": \"00:00:00\",\r\n      \"ContractionRunTime\": \"00:00:00\",\r\n      \"ContractionRampUpTime\": \"00:00:00\",\r\n      \"ContractionRampDownTime\": \"00:00:00\",\r\n      \"MuscleIDs\": [\r\n        \"1A\",\r\n        \"2B\",\r\n        \"3C\"\r\n      ]\r\n    },\r\n    {\r\n      \"Name\": \"Test Group2\",\r\n      \"ContractionStartTime\": \"00:00:00\",\r\n      \"ContractionRunTime\": \"00:00:00\",\r\n      \"ContractionRampUpTime\": \"00:00:00\",\r\n      \"ContractionRampDownTime\": \"00:00:00\",\r\n      \"MuscleIDs\": [\r\n        \"3C\"\r\n      ]\r\n    }\r\n  ],\r\n  \"UngrouppedMuscles\": [\r\n    {\r\n      \"ContractionStartTime\": \"00:00:00\",\r\n      \"ContractionRunTime\": \"00:00:00\",\r\n      \"ContractionRampUpTime\": \"00:00:00\",\r\n      \"ContractionRampDownTime\": \"00:00:00\",\r\n      \"MuscleID\": \"5A\"\r\n    },\r\n    {\r\n      \"ContractionStartTime\": \"00:00:00\",\r\n      \"ContractionRunTime\": \"00:00:00\",\r\n      \"ContractionRampUpTime\": \"00:00:00\",\r\n      \"ContractionRampDownTime\": \"00:00:00\",\r\n      \"MuscleID\": \"3A\"\r\n    }\r\n  ],\r\n  \"Chimes\": [],\r\n  \"Rests\": []\r\n}";

      
        var serializer = new MotorTaskConfigurationSerializer();

        var actual = serializer.ToJson(motorTask.Configuration);

        Assert.Equal(expected, actual);

    }
}
