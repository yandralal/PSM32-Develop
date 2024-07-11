using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Psm32.Stores;

namespace Psm32.Commands;

class OpenMotorTaskCommand : CommandBase
{
    public OpenMotorTaskCommand(DeviceStore deviceStore)
    {
        _deviceStore = deviceStore;
    }
    public override void Execute(object? parameter)
    {
        throw new NotImplementedException();
    }

    private readonly DeviceStore _deviceStore;
}
