using System;

namespace Psm32.Models;

public class UngrouppedMuscle: IEquatable<UngrouppedMuscle>
{
    public UngrouppedMuscle(Muscle muscle)
    {
        Muscle = muscle;
    }

    public TimeOnly ContractionStartTime { get; set; }
    public TimeSpan ContractionRunTime { get; set; }
    public TimeSpan ContractionRampUpTime { get; set; }
    public TimeSpan ContractionRampDownTime { get; set; }
    public Muscle Muscle { get; set; }

    public bool Equals(UngrouppedMuscle? muscle)
    {
        if (muscle == null)
        {
            return false;
        }
        //Note: comparison of MuscleID is case insensitive
        return ContractionStartTime.Equals(muscle.ContractionStartTime) &&
               TimeSpan.Compare(ContractionRunTime, muscle.ContractionRunTime) == 0 &&
               TimeSpan.Compare(ContractionRampUpTime, muscle.ContractionRampUpTime) == 0 &&
               TimeSpan.Compare(ContractionRampDownTime, muscle.ContractionRampDownTime) == 0 &&
               Muscle.ID.ToLower() == muscle.Muscle.ID.ToLower();
    }

    public override bool Equals(object? obj) => Equals(obj as UngrouppedMuscle);

    public override int GetHashCode()
    {
        return HashCode.Combine(ContractionStartTime, ContractionRunTime, ContractionRampUpTime, ContractionRampDownTime, Muscle.ID);
    }
}