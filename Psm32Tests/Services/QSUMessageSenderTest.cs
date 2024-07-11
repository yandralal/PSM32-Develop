using Moq;
using Psm32.Models;
using Psm32.Services;
using Psm32Tests.Mocks;
using Serilog;
using System.Reflection;

namespace Psm32Tests.Services;


public class QSUMessageSenderTest : IDisposable
{
    private readonly IQSUMessageSender _qsuMessageSender;
    private readonly LoggerMock _loggerMock;
    private readonly ComPortCommunicationServiceMock _comPortCommunicationServiceMock;

    public QSUMessageSenderTest()
    {
        _loggerMock = new LoggerMock();
        _comPortCommunicationServiceMock = new ComPortCommunicationServiceMock();
        _qsuMessageSender = new QSUMessageSender(_comPortCommunicationServiceMock.MockObject.Object, _loggerMock.MockObject.Object);
    }

    public void Dispose()
    {
        QSUMessageSender.ResetSentMessagesDict();
    }

    [Fact]
    public void QsuReset_Success()
    {
        var sentMessage = _qsuMessageSender.QsuReset(1);

        _comPortCommunicationServiceMock.MockObject.Verify(m => m.EnqueueWriteMessage(It.IsAny<TransferMessage>()), Times.Once);
        Assert.True(_qsuMessageSender.MessageSent(sentMessage.MessageId));
    }

    [Fact]
    public void QsuEnum_Success()
    {
        var sentMessage = _qsuMessageSender.QsuEnum(1);

        _comPortCommunicationServiceMock.MockObject.Verify(m => m.EnqueueWriteMessage(It.IsAny<TransferMessage>()), Times.Once);
        Assert.True(_qsuMessageSender.MessageSent(sentMessage.MessageId));
    }

    [Fact]
    public void ChannelGoIdle_Success()
    {
        var sentMessage = _qsuMessageSender.ChannelGoIdle(1, 'A');

        _comPortCommunicationServiceMock.MockObject.Verify(m => m.EnqueueWriteMessage(It.IsAny<TransferMessage>()), Times.Once);
        Assert.True(_qsuMessageSender.MessageSent(sentMessage.MessageId));
    }

    [Fact]
    public void ChannelGoRun_Success()
    {
        var sentMessage = _qsuMessageSender.ChannelGoRun(1, 'A');

        _comPortCommunicationServiceMock.MockObject.Verify(m => m.EnqueueWriteMessage(It.IsAny<TransferMessage>()), Times.Once);
        Assert.True(_qsuMessageSender.MessageSent(sentMessage.MessageId));
    }

    [Fact]
    public void ChannelGoOff_Success()
    {
        var sentMessage = _qsuMessageSender.ChannelGoOff(1, 'A');

        _comPortCommunicationServiceMock.MockObject.Verify(m => m.EnqueueWriteMessage(It.IsAny<TransferMessage>()), Times.Once);
        Assert.True(_qsuMessageSender.MessageSent(sentMessage.MessageId));
    }

    [Fact]
    public void ChannelSetRamp_Success()
    {
        var sentMessage = _qsuMessageSender.ChannelSetRamp(1, 'A', 70, 30);

        _comPortCommunicationServiceMock.MockObject.Verify(m => m.EnqueueWriteMessage(It.IsAny<TransferMessage>()), Times.Once);
        Assert.True(_qsuMessageSender.MessageSent(sentMessage.MessageId));
    }

    [Fact]
    public void ChannelSetPulse_Success()
    {
        var sentMessage = _qsuMessageSender.ChannelSetPulse(1, 'A', 100, 100, 2, 2, 70);

        _comPortCommunicationServiceMock.MockObject.Verify(m => m.EnqueueWriteMessage(It.IsAny<TransferMessage>()), Times.Once);
        Assert.True(_qsuMessageSender.MessageSent(sentMessage.MessageId));
    }

    [Fact]
    public void SendMultipleMessages_Success()
    {
        TransferMessage SendMessage1()
        {
            return _qsuMessageSender.ChannelSetPulse(1, 'A', 100, 100, 2, 2, 70);
        }
        TransferMessage SendMessage2()
        {
            return _qsuMessageSender.ChannelSetPulse(1, 'A', 200, 200, 1, 1, 60);
        }
        TransferMessage SendMessage3()
        {
            return _qsuMessageSender.ChannelSetPulse(1, 'A', 50, 50, 3, 3, 50);
        }

        Task<TransferMessage> task1 = Task.Factory.StartNew(() => SendMessage1());
        Task<TransferMessage> task2 = Task.Factory.StartNew(() => SendMessage2());
        Task<TransferMessage> task3 = Task.Factory.StartNew(() => SendMessage3());

        Task.WaitAll(task1, task2, task3);

        var sentMessage1 = task1.Result;
        var sentMessage2 = task2.Result;
        var sentMessage3 = task3.Result;

        _comPortCommunicationServiceMock.MockObject.Verify(m => m.EnqueueWriteMessage(It.IsAny<TransferMessage>()), Times.Exactly(3));
        Assert.True(_qsuMessageSender.MessageSent(sentMessage1.MessageId));
        Assert.True(_qsuMessageSender.MessageSent(sentMessage2.MessageId));
        Assert.True(_qsuMessageSender.MessageSent(sentMessage3.MessageId));
    }

    [Fact]
    public void ResendMessage_Success()
    {
        var data = new List<byte> { 1, 2, 3 };
        var message = new TransferMessage(new CommandMessage(1, 1, 'A', data));

        var sentMessage = _qsuMessageSender.ResendMessage(message);

        _loggerMock.MockObject.Verify(m => m.Error(It.IsAny<string>()), Times.Never);
        Assert.True(_qsuMessageSender.MessageSent(sentMessage.MessageId));

    }

    [Fact]
    public void ResendMessage_ToоManyRetires_DontResend()
    {
        var data = new List<byte> { 1, 2, 3 };
        var message = new TransferMessage(new CommandMessage(1, 1, 'A', data));

        var sentMessage1Id = _qsuMessageSender.ResendMessage(message)?.MessageId;
        var sentMessage2Id = _qsuMessageSender.ResendMessage(message)?.MessageId;
        var sentMessage3Id = _qsuMessageSender.ResendMessage(message)?.MessageId;

        _loggerMock.MockObject.Verify(m => m.Error(It.IsAny<string>()), Times.Once);
        Assert.NotNull(sentMessage1Id);
        Assert.NotNull(sentMessage2Id);
        Assert.Null(sentMessage3Id);
        Assert.True(_qsuMessageSender.MessageSent(sentMessage1Id.Value));
        Assert.True(_qsuMessageSender.MessageSent(sentMessage2Id.Value));
    }

    [Fact]
    public void GetTimedOutMessages_Success()
    {
        _qsuMessageSender.QsuReset(1);
        _qsuMessageSender.QsuReset(2);

        QSUMessageSender.WAIT_TIMEOUT_SEC = 0;

        var result = _qsuMessageSender.GetTimedOutMessages();
        Assert.Equal(2, result.Count);

        QSUMessageSender.ResetSentMessagesDict();

    }
}
