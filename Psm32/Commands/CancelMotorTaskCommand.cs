using Psm32.Models;
using Psm32.Services;
using Psm32.Stores;
using Psm32.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psm32.Commands;

public class CancelMotorTaskCommand : CommandBase
{
    public CancelMotorTaskCommand(DeviceStore deviceStore, INavigationService qSUnitsNavigationService)
    {
        _deviceStore = deviceStore;
        _qSUnitsNavigationService = qSUnitsNavigationService;
    }
    public override void Execute(object? parameter)
    {
        _deviceStore.ClearCurrentMotorTask();
        _qSUnitsNavigationService.Navigate();
    }

    private readonly DeviceStore _deviceStore;
    private readonly INavigationService _qSUnitsNavigationService;
}
