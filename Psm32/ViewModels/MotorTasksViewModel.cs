using Psm32.Commands;
using Psm32.Models;
using Psm32.Services;
using Psm32.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Psm32.ViewModels;

public class MotorTasksViewModel : ViewModelBase
{
    public ICommand SaveMotorTaskCommand { get; }
    public ICommand DeleteMotorTaskCommand { get; }
    public ICommand LoadMotorTasksCommand { get;  }
    public IEnumerable<MotorTaskViewModel> МotorTasks => _motorTasks;


    public MotorTasksViewModel(DeviceStore deviceStore)
    {
        _deviceStore = deviceStore;
        _motorTasks = new ObservableCollection<MotorTaskViewModel>();


        SaveMotorTaskCommand = new SaveMotorTaskCommand(_deviceStore);

        DeleteMotorTaskCommand = new DeleteMotorTaskCommand();

        LoadMotorTasksCommand = new LoadMotorTasksCommand(_deviceStore, this);

        _deviceStore.MotorTaskAdded += OnMotorTaskAdded;
    }

    public override void Dispose()
    {
        _deviceStore.MotorTaskAdded -= OnMotorTaskAdded;
        base.Dispose();
    }

    private void OnMotorTaskAdded(MotorTask motorTask)
    {
        var motorTaskViewModel = new MotorTaskViewModel(motorTask);
        _motorTasks.Add(motorTaskViewModel);
    }

    public static MotorTasksViewModel LoadViewModel(DeviceStore deviceStore)
    {
        MotorTasksViewModel motorTasksViewModel = new MotorTasksViewModel(deviceStore);
        motorTasksViewModel.LoadMotorTasksCommand.Execute(motorTasksViewModel);

        return motorTasksViewModel;
    }

    public void UpdateMotorTasks(IEnumerable<MotorTask> motorTasks)
    {
        _motorTasks.Clear();

        foreach (var motorTask in motorTasks.ToList())
        {
            _motorTasks.Add(new MotorTaskViewModel(motorTask));
        }
    }

    private readonly ObservableCollection<MotorTaskViewModel> _motorTasks;

    private readonly DeviceStore _deviceStore;
}
