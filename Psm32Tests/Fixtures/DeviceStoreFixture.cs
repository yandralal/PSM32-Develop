using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Psm32.Stores;

namespace Psm32Tests.Fixtures;

public class DeviceStoreFixture
{
    public DeviceFixture DeviceFixture;
    public DeviceStore DeviceStore { get; }

    public DeviceStoreFixture()
    {
        DeviceFixture = new DeviceFixture();
        DeviceStore = new DeviceStore(DeviceFixture.Device);
    }
}
