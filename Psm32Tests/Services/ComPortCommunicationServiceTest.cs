using Moq;
using Psm32.Models;
using Psm32.Services;
using Psm32Tests.Mocks;

namespace Psm32Tests.Services;

//***** NOTЕ ********: These tests are time sensitive and may be unstable

public class ComPortCommunicationServiceTest: IDisposable
{
    private readonly ComPortCommunicationService _comPortCommunicationService;
    private readonly List<List<byte>> _recievedMessages;

    private Mock<IComPortWrapper> _mockComPortWrapper;
    public ComPortCommunicationServiceTest()
    {
        _mockComPortWrapper = new Mock<IComPortWrapper>();
        _recievedMessages = new List<List<byte>>();

        _mockComPortWrapper.Setup(m => m.Open()).Verifiable();
        _mockComPortWrapper.Setup(m => m.Close()).Verifiable();
        _mockComPortWrapper.Setup(m => m.Write(It.IsAny<List<byte>>())).Verifiable();

        _comPortCommunicationService = new ComPortCommunicationService(_mockComPortWrapper.Object, new LoggerMock().MockObject.Object);

        void OnCommandRecieved(List<byte> message)
        {
            _recievedMessages.Add(message);
        }

        _comPortCommunicationService.Start(OnCommandRecieved);
    }

    public void Dispose()
    {
        _comPortCommunicationService.Stop();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public void NoMessageToSend_Success()
    {
        Thread.Sleep(1000);

        _mockComPortWrapper.Verify(m => m.Write(It.IsAny<List<byte>>()), Times.Never);
    }

    [Fact]
    public void SendOneMessage_Success()
    {
        var data = new List<byte> { 1, 2, 3 };
        var messageToSend = new TransferMessage(new CommandMessage(qsuId: 1, channelId: 1, command: 'A', data: data));
        _comPortCommunicationService.EnqueueWriteMessage(messageToSend);
              
        Thread.Sleep(1000);

        _mockComPortWrapper.Verify(m => m.Write(It.IsAny<List<byte>>()), Times.Once);
    }

    [Fact]
    public void SendMultipleMessages_Success()
    {
        var data = new List<byte> { 1, 2, 3 };
        var messageToSend1 = new TransferMessage(new CommandMessage(qsuId: 1, channelId: 1, command: 'A', data: data));
        var messageToSend2 = new TransferMessage(new CommandMessage(qsuId: 1, channelId: 1, command: 'A', data: data));
        var messageToSend3 = new TransferMessage(new CommandMessage(qsuId: 1, channelId: 1, command: 'A', data: data));
        _comPortCommunicationService.EnqueueWriteMessage(messageToSend1);
        _comPortCommunicationService.EnqueueWriteMessage(messageToSend2);
        _comPortCommunicationService.EnqueueWriteMessage(messageToSend3);

        Thread.Sleep(1000);

        _mockComPortWrapper.Verify(m => m.Write(It.IsAny<List<byte>>()), Times.Exactly(3));
        Assert.True(messageToSend1.MessageId != messageToSend2.MessageId && messageToSend2.MessageId != messageToSend3.MessageId);
    }

    [Fact]
    public void NoRecievedMessage_Success()
    {
        Thread.Sleep(1000);

        Assert.Empty(_recievedMessages);
    }

    [Fact]
    public void RecievedOneMessage_Success()
    {
        _mockComPortWrapper.SetupSequence(m => m.Read())
           .Returns(() => new List<byte> { 81, 1, 17, 65, 2, 100, 101 })
           .Returns(() => new List<byte> { });

        Thread.Sleep(1000);

        Assert.Single(_recievedMessages);
    }

    [Fact]
    public void RecievedMultipleMessages_Success()
    {
        _mockComPortWrapper.SetupSequence(m => m.Read())
           .Returns(() => new List<byte> { 81, 1, 17, 65, 2, 100, 101, 81, 1, 17, 65 })
           .Returns(() => new List<byte> { 3, 99, 99, 99 }) 
           .Returns(() => new List<byte> {} );

        Thread.Sleep(1000);

        Assert.Equal(2, _recievedMessages.Count);
        Assert.Equal(new List<byte> { 81, 1, 17, 65, 2, 100, 101 }, _recievedMessages[0]);
        Assert.Equal(new List<byte> { 81, 1, 17, 65, 3, 99, 99, 99 }, _recievedMessages[1]);
    }

    [Fact]
    public void RecievedMultipleMessages_DiscardJunk()
    {
        _mockComPortWrapper.SetupSequence(m => m.Read())
           .Returns(() => new List<byte> { 97, 97, 97, 81, 100, 100, 100, 1 })  // aaaQddd1
           .Returns(() => new List<byte> { 100, 101, 101, 81, 102, 102 })  // deeQff
           .Returns(() => new List<byte> { 102, 3 }) // f3
           .Returns(() => new List<byte> { 102, 102, 102 })  // fff
           .Returns(() => new List<byte> { 103, 103 });  // gg

        Thread.Sleep(1000);

        Assert.Equal(2, _recievedMessages.Count);
        Assert.Equal(new List<byte> { 81, 100, 100, 100, 1, 100 }, _recievedMessages[0]);
        Assert.Equal(new List<byte> { 81, 102, 102, 102, 3, 102, 102, 102 }, _recievedMessages[1]);
    }
}
