using Psm32.Exceptions;
using Psm32.Models;
using Psm32.Services;
using Psm32.Stores;
using Psm32.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;

namespace Psm32.Commands;

public class GoToTestScreenCommand : CommandBase
{
    private readonly INavigationService _muscleTestViewNavigationService;
    private readonly DeviceStore _deviceStore;

    public GoToTestScreenCommand(
        DeviceStore deviceStore,
        INavigationService muscleTestViewNavigationService)
    {
        _deviceStore = deviceStore;
        _muscleTestViewNavigationService = muscleTestViewNavigationService;
    }
    public override void Execute(object? parameter)
    {
        _muscleTestViewNavigationService.Navigate();
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
