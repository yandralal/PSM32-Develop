using Psm32.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psm32.ViewModels;

public class MotorTaskViewModel : ViewModelBase
{

    public Guid ID => _motorTask.ID;
    public string Name => _motorTask.Name;
    public string? PatientId => _motorTask.PatientId;
    public string Configuration => ""; // TODO: implement viewmodel for motortask config _motorTask.Configuration;
    public DateTime Created => _motorTask.Created;
    public DateTime Updated => _motorTask.Updated;

    public MotorTaskViewModel(MotorTask motorTask)
    {
        _motorTask = motorTask;
    }

    private readonly MotorTask _motorTask;
}
