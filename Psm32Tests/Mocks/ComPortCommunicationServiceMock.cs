using Moq;
using Psm32.Models;
using Psm32.Services;

namespace Psm32Tests.Mocks;

internal class ComPortCommunicationServiceMock
{
    private readonly Mock<IComPortCommunicationService> _mockComPortCommunicationService;

    public Mock<IComPortCommunicationService> MockObject => _mockComPortCommunicationService;

    public ComPortCommunicationServiceMock()
    {
        _mockComPortCommunicationService = new Mock<IComPortCommunicationService>();
        _mockComPortCommunicationService.Setup(m => m.Start(It.IsAny<Action<List<byte>>>())).Verifiable();
        _mockComPortCommunicationService.Setup(m => m.EnqueueWriteMessage(It.IsAny<TransferMessage>())).Verifiable();
    }


}
