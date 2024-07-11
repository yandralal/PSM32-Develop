using Psm32.Models;
using Psm32.Stores;
using Psm32.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psm32.Commands;

public class SaveMotorTaskCommand : AsyncCommandBase
{
    public SaveMotorTaskCommand(DeviceStore deviceStore)
    {
        _deviceStore = deviceStore;
    }
    public override async Task ExecuteAsync(object? parameter)
    {
        await _deviceStore.SaveMotorTask();
    }

    private readonly DeviceStore _deviceStore;
}
