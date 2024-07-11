using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Psm32.Commands;
using Psm32.Services;
using Psm32.Stores;

namespace Psm32.ViewModels;

public class TopNavigationBarViewModel : ViewModelBase
{
    DeviceStore _deviceStore;
    string _motorTaskName;
    string? _patientID;

    public bool IsLoggedIn => _deviceStore.CurrentSession.LoggedIn();
    public bool MotorTaskOpen => _deviceStore.MotorTaskOpen();

    public bool CanCreateMotorTask => !string.IsNullOrEmpty(_motorTaskName);
    public ICommand GoToTestScreenCommand { get; }

    public ICommand CreateMotorTaskCommand { get; }

    public string MotorTaskName
    {
        get
        {
            return _motorTaskName;
        }
        set
        {
            _motorTaskName = value;
            OnPropertyChanged(nameof(MotorTaskName));
            OnPropertyChanged(nameof(CanCreateMotorTask));
        }
    }

    public string? PatientID
    {
        get
        {
            return _patientID;
        }
        set
        {
            _patientID = value;
            OnPropertyChanged(nameof(PatientID));
        }
    }

    public TopNavigationBarViewModel(
        DeviceStore deviceStore,
        INavigationService muscleTestNavigationService)
    {
        _deviceStore = deviceStore;
        GoToTestScreenCommand = new GoToTestScreenCommand(_deviceStore, muscleTestNavigationService);
        CreateMotorTaskCommand = new CreateInMemoryMotorTaskCommand(this, _deviceStore);
        if (_deviceStore.CurrentMotorTask != null)
        {
            _motorTaskName = _deviceStore.CurrentMotorTask.Name;
            _patientID = _deviceStore.CurrentMotorTask.PatientId;
        }
        else
        {
            _motorTaskName = "";
            _patientID = null;
        }
    }

    public static TopNavigationBarViewModel LoadViewModel(
        DeviceStore deviceStore,
        INavigationService muscleTestNavigationService)
    {
        return new TopNavigationBarViewModel(deviceStore, muscleTestNavigationService);
    }

    public void OnMotorTaskOpenPropertyChanged()
    {
        OnPropertyChanged(nameof(MotorTaskOpen));
    }
}
