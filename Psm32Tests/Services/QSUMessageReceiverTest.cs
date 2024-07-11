using Moq;
using Psm32.Models;
using Psm32.Services;
using Psm32Tests.Mocks;
using Serilog;

namespace Psm32Tests.Services;


public class QSUMessageRecieverTest : IDisposable
{
    private readonly Mock<IQSUMessageSender> _qsuMessageSender;
    private readonly IQSUMessageReceiver _qsuMessageReceiver;
    private readonly LoggerMock _loggerMock;
    private readonly ComPortCommunicationServiceMock _comPortCommunicationServiceMock;

    private Dictionary<byte, TransferMessage> _sentMessagesDict = new();


    public QSUMessageRecieverTest()
    {
        _loggerMock = new LoggerMock();
        _comPortCommunicationServiceMock = new ComPortCommunicationServiceMock();
        _qsuMessageSender = new Mock<IQSUMessageSender>();

        _qsuMessageSender.Setup(m => m.QsuEnum(It.IsAny<int>())).Returns((int id)=>
        {
            var message = CommandBuilder.QsuEnum_Command(id);
            var transferMessage = new TransferMessage(message);
            _sentMessagesDict.Add(transferMessage.MessageId, transferMessage);
            return transferMessage;
        });

        _qsuMessageReceiver = new QSUMessageReceiver(
            _comPortCommunicationServiceMock.MockObject.Object,
            _qsuMessageSender.Object,
            _loggerMock.MockObject.Object);
    }

    public void Dispose()
    {
        QSUMessageReceiver.ResetReceivedMessagesQueue();
    }

    public void GetTimedOutMessages_Success()
    {
        _qsuMessageSender.Object.QsuEnum(1);

        _qsuMessageReceiver.Start();
    }
}

  