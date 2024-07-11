using Psm32.Exceptions;
using Psm32.Models;
using Psm32.Services;
using Psm32.Stores;
using Psm32.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;

namespace Psm32.Commands;

public class CreateInMemoryMotorTaskCommand : CommandBase
{
    private readonly TopNavigationBarViewModel _topNavigationBarViewModel;
    private readonly DeviceStore _deviceStore;

    public CreateInMemoryMotorTaskCommand(
        TopNavigationBarViewModel topNavigationBarViewModel,
        DeviceStore deviceStore)
    {
        _topNavigationBarViewModel = topNavigationBarViewModel;
        _deviceStore = deviceStore;
    }
    public override void Execute(object? parameter)
    {
        var motorTask = new MotorTask(_topNavigationBarViewModel.MotorTaskName, _topNavigationBarViewModel.PatientID);

        _deviceStore.SetCurrentMotorTask(motorTask);
        _topNavigationBarViewModel.OnMotorTaskOpenPropertyChanged();
    }

    public override bool CanExecute(object? parameter)
    {
        return base.CanExecute(parameter);
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnCanExecuteChanged();

    }
}
