using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Psm32.Models;
using Psm32.Services;

namespace Psm32Tests.Fixtures;

public class DeviceFixture
{
    public Device Device { get; }
    public Mock<IMotorTaskDBService> MockMotorTaskDBService { get; set; }
    public Mock<IMotorTaskValidator> MockMotorTaskValidator { get; set; }
    public Mock<IQSUMessageReceiver> MockQsuMessageReceiver { get; set; }
    public Mock<IAppConfigDBService> MockAppConfigDBService { get; set; }
    public Mock<IQSUScanner> MockQsuScanner { get; set; }
    public Mock<IAuthenticationService> MockAuthenticationService { get; set; }
    public Mock<IMuscleNamesReadService> MockMuscleNamesReadService { get; set; }

    public DeviceFixture()
    {

        MockMotorTaskDBService = new Mock<IMotorTaskDBService>();
        MockMotorTaskValidator = new Mock<IMotorTaskValidator>();
        MockQsuMessageReceiver = new Mock<IQSUMessageReceiver>();
        MockAppConfigDBService = new Mock<IAppConfigDBService>();
        MockQsuScanner = new Mock<IQSUScanner>();
        MockAuthenticationService = new Mock<IAuthenticationService>();
        MockMuscleNamesReadService = new Mock<IMuscleNamesReadService>();

        Device = new Device(
            MockMotorTaskDBService.Object,
            MockAppConfigDBService.Object,
            MockMotorTaskValidator.Object,
            MockQsuMessageReceiver.Object,
            MockQsuScanner.Object,
            MockAuthenticationService.Object,
            MockMuscleNamesReadService.Object);
    }
}
