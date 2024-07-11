using System;
using System.ComponentModel.DataAnnotations;

namespace Psm32.DB;

public class MotorTaskDTO
{
    public MotorTaskDTO(Guid iD, string name, string? patientId, string configuration, DateTime created, DateTime updated)
    {
        ID = iD;
        Name = name;
        PatientId = patientId;
        Configuration = configuration;
        Created = created;
        Updated = updated;
    }

    [Key]
    public Guid ID { get; set;}
    public string Name { get; set;}
    public string? PatientId { get; set;}
    public string Configuration { get; set;}
    public DateTime Created { get; set;}
    public DateTime Updated { get; set; }
}
