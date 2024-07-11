
using Psm32.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psm32.Models;

public class MotorTaskConfiguration : IEquatable<MotorTaskConfiguration>
{ 
    public TimeSpan CycleDuration { get; set; }
    public int Cycles { get; set; }
    public int SpeedPercent { get; set; }
    public decimal AmpStep { get; set; }
    public List<MuscleGroup> MuscleGroups { get; set; } = new();
    public List<UngrouppedMuscle> UngrouppedMuscles { get; set; } = new();
    public List<Chime> Chimes { get; set; } = new();
    public List<Rest> Rests { get; set; } = new();


    public bool Equals(MotorTaskConfiguration? configuration)
    {
       
        if (configuration == null)
        {
            return false;
        }

        var grouppedEqual = 
            !MuscleGroups.Except(configuration.MuscleGroups).Any() && 
            !configuration.MuscleGroups.Except(MuscleGroups).Any();

        var ungrouppedEqual =
            !UngrouppedMuscles.Except(configuration.UngrouppedMuscles).Any() &&
            !configuration.UngrouppedMuscles.Except(UngrouppedMuscles).Any();

        var chimesEqual =
            !Chimes.Except(configuration.Chimes).Any() &&
            !configuration.Chimes.Except(Chimes).Any();

        var restsEqual =
            !Rests.Except(configuration.Rests).Any() &&
            !configuration.Rests.Except(Rests).Any();


        return TimeSpan.Compare(CycleDuration, configuration.CycleDuration) == 0 &&
               Cycles == configuration.Cycles &&
               SpeedPercent == configuration.SpeedPercent &&
               AmpStep == configuration.AmpStep &&
               grouppedEqual &&
               ungrouppedEqual &&
               chimesEqual &&
               restsEqual;
    }

    public override bool Equals(object? obj) => Equals(obj as MotorTaskConfiguration);

    public override int GetHashCode()
    {
        return HashCode.Combine(
            CycleDuration, 
            Cycles, 
            SpeedPercent,
            AmpStep,
            MuscleGroups, 
            UngrouppedMuscles);
    }

    public void AddMuscleGroup(string groupName)
    {
        var muscleGroup = MuscleGroups.Find(mg => mg.Name == groupName);
        if (muscleGroup != null)
        {
            throw new Exception($"Muscle Group named `{groupName}` already exists");
        }
        MuscleGroups.Add(new MuscleGroup(groupName));
    }

    public void AddMusclesToGroup(string groupName, List<Muscle> muscles)
    {
        var muscleGroup = MuscleGroups.Find(mg => mg.Name == groupName);
        if (muscleGroup == null)
        {
             throw new Exception($"Muscle Group named `{groupName}` not found");
        }

        ValidateMusclesInGroups(muscles);

        muscleGroup.AddMuscles(muscles);
    }

    public void AddUngrouppedMuscle(Muscle muscle)
    {
        var ungrouppedMuscle = UngrouppedMuscles.Find(um => um.Muscle.ID == muscle.ID);
        if (ungrouppedMuscle != null)
        {
            throw new Exception($"Muscle `{muscle.ID} is already in the motor taks ungroupped list");
        }
        UngrouppedMuscles.Add(new UngrouppedMuscle(muscle));
    }

    private void ValidateMusclesInGroups(List<Muscle> muscles)
    {
        var allMuscleIdsInGroups = getAllMuscleIdsInGroups();
        foreach (var muscle in muscles)
        {
            var found = allMuscleIdsInGroups.Find(id => id == muscle.ID);
            if (found != null)
            {
                throw new Exception($"Muscle `{muscle.ID}` is already in another group");
            }

        }
    }
    private List<string>  getAllMuscleIdsInGroups()
    {
        List<string> muscles = new();
        foreach(var group in MuscleGroups)
        {
            muscles.AddRange(group.Muscles.Select( m => m.ID));
        }
        return muscles;
    }

}
