using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Psm32.Models;

public class MotorTask
{
    public MotorTask(Guid iD, string name, string? patientId, MotorTaskConfiguration configuration, DateTime created, DateTime updated)
    {
        ID = iD;
        Name = name;
        PatientId = patientId;
        Configuration = configuration;
        Created = created;
        Updated = updated;
    }

    public MotorTask(string name, string? patientId)
    {
        ID = Guid.NewGuid();
        Name = name;
        PatientId = patientId;
        Configuration = new();
        Created = DateTime.Now;
        Updated = DateTime.Now;
    }

    public Guid ID { get; }
    public string Name { get; }
    public string? PatientId { get; }
    public DateTime Created { get; }
    public DateTime Updated { get; }

    public MotorTaskConfiguration Configuration { get; set;  } //TODO : remove setter?

    public void AddMuscleGroup(string groupName)
    {
        Configuration.AddMuscleGroup(groupName);
    }

    public void AddMusclesToGroup(string groupName, List<Muscle> muscles)
    {
        Configuration.AddMusclesToGroup(groupName, muscles);
    }

    public void AddUngrouppedMuscle(Muscle muscle)
    {
        Configuration.AddUngrouppedMuscle(muscle);
    }
}
