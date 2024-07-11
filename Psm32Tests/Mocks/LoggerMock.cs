using Moq;
using Serilog;

namespace Psm32Tests.Mocks;

internal class LoggerMock
{
    private readonly Mock<ILogger> _mockLogger;

    public Mock<ILogger> MockObject => _mockLogger;

    public LoggerMock()
    {
        _mockLogger = new Mock<ILogger>();
        _mockLogger.Setup(m => m.Information(It.IsAny<string>())).Verifiable();
        _mockLogger.Setup(m => m.Error(It.IsAny<string>())).Verifiable();
    }
}
