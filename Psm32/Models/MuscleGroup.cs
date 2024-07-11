using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Psm32.Models;

public class MuscleGroup : IEquatable<MuscleGroup>
{
    public MuscleGroup(string name)
    {
        Name = name;
    }

    public string Name { get;set; }
    public TimeOnly ContractionStartTime { get; set; }
    public TimeSpan ContractionRunTime { get; set; }
    public TimeSpan ContractionRampUpTime { get; set; }
    public TimeSpan ContractionRampDownTime { get; set; }
    public List<Muscle> Muscles { get; set; } = new();

    public bool Equals(MuscleGroup? group)
    {
        if (group == null)
        {
            return false;
        }

        //Note: comparison of MuscleIDs is case insensitive
        var muscleIDs = Muscles.Select(m => m.ID);
        var groupMuscleIDs = group.Muscles.Select(m => m.ID);
        var muscleIDsEqual =
           !muscleIDs.Except(groupMuscleIDs, StringComparer.OrdinalIgnoreCase).Any() &&
           !groupMuscleIDs.Except(muscleIDs, StringComparer.OrdinalIgnoreCase).Any();

        return Name == group.Name &&
               ContractionStartTime.Equals(group.ContractionStartTime) &&
               TimeSpan.Compare(ContractionRunTime, group.ContractionRunTime) == 0 &&
               TimeSpan.Compare(ContractionRampUpTime, group.ContractionRampUpTime) == 0 &&
               TimeSpan.Compare(ContractionRampDownTime, group.ContractionRampDownTime) == 0 &&
               muscleIDsEqual;
    }

    public override bool Equals(object? obj) => Equals(obj as MuscleGroup); 
    public override int GetHashCode()
    {
        var IdsAsString = string.Join(",", Muscles.Select(m=>m.ID).OrderBy(id => id));
        return HashCode.Combine(
            ContractionStartTime, 
            ContractionRunTime, 
            ContractionRampUpTime, 
            ContractionRampDownTime,
            IdsAsString
            );
    }

    public void AddMuscles(List<Muscle> muscles)
    {
        Muscles.AddRange(muscles);
    }

}