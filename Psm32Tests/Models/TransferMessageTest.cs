using Psm32.Models;
using Psm32.Exceptions;
using Psm32.Services;

namespace Psm32Tests.Models;

public class TransferMessageTest : IDisposable
{
    public void Dispose()
    {
        TransferMessage.ResetMessageId();
    }

    [Fact]
    public void Init_InvalidChannelId_Success()
    {
        var data = new List<byte>() { 1, 2, 3, 4, 5 };
        var cMessage = new CommandMessage(8, 3, 'F', data);
        var transferMessage = new TransferMessage(cMessage);

        Assert.Equal('Q', transferMessage.Sync);
        Assert.Equal(1, transferMessage.Retries);
        Assert.True(cMessage.Equals(transferMessage.CommandMessage));
    }

    [Fact]
    public void PrepareToResend_Success()
    {
        var data = new List<byte>() { 1, 2, 3, 4, 5};
        var transferMessage = new TransferMessage(new CommandMessage(8, 3, 'F', data));
        var oldMessageId = transferMessage.MessageId;
        var oldSentTime = transferMessage.SentTime;

        transferMessage.PrepareToResend();

        Assert.True(oldMessageId < transferMessage.MessageId);
        Assert.True(oldSentTime < transferMessage.SentTime);
        Assert.Equal(2, transferMessage.Retries);
    }

    [Fact]
    public void CanResend_True()
    {
        var data = new List<byte>() { 1, 2, 3, 4, 5 };
        var transferMessage = new TransferMessage(new CommandMessage(8, 3, 'F', data));

        Assert.True(transferMessage.CanResend());
    }

    [Fact]
    public void CanResend_False()
    {
        var data = new List<byte>() { 1, 2, 3, 4, 5 };
        var transferMessage = new TransferMessage(new CommandMessage(8, 3, 'F', data));
        transferMessage.PrepareToResend();
        transferMessage.PrepareToResend();

        Assert.False(transferMessage.CanResend());
    }

    [Fact]
    public void Serialize_ValidMessage()
    {
        var data = new List<byte>() { 1, 2 };
        var transferMessage = new TransferMessage(new CommandMessage(8, 3, 'F', data));

        var messageToSend = transferMessage.Serialize();

        Assert.Equal(new List<byte> { 81, 1, 56, 70, 2, 1,2, 82}, messageToSend);
    }


    [Fact]
    public void Serialize_QsuReset()
    {
        var transferMessage = new TransferMessage(CommandBuilder.QsuReset_Command(null));

        var messageToSend = transferMessage.Serialize();

        Assert.Equal(new List<byte> { 81, 1, 0, 42, 0, 241 }, messageToSend);
    }

    [Fact]
    public void Serialize_QsuEnum()
    {
        var transferMessage = new TransferMessage(CommandBuilder.QsuEnum_Command(1));

        var messageToSend = transferMessage.Serialize();

        Assert.Equal(new List<byte> { 81, 1, 1, 69, 0, 24 }, messageToSend);
    }

    [Fact]
    public void Serialize_ChannelGoIdle()
    {
        var transferMessage = new TransferMessage(CommandBuilder.ChannelGoIdle_Command(1,'A'));

        var messageToSend = transferMessage.Serialize();

        Assert.Equal(new List<byte> { 81, 1, 17, 73, 0, 31 }, messageToSend);
    }

    [Fact]
    public void Serialize_ChannelGoRun()
    {
        var transferMessage = new TransferMessage(CommandBuilder.ChannelGoRun_Command(1, 'A'));

        var messageToSend = transferMessage.Serialize();

        Assert.Equal(new List<byte> { 81, 1, 17, 71, 0, 195 }, messageToSend);
    }

    [Fact]
    public void Serialize_ChannelGoOff()
    {
        var transferMessage = new TransferMessage(CommandBuilder.ChannelGoOff_Command(1, 'A'));

        var messageToSend = transferMessage.Serialize();

        Assert.Equal(new List<byte> { 81, 1, 17, 70, 0, 7 }, messageToSend);
    }

    [Fact]
    public void Serialize_ChannelSetRamp()
    {
        //Create message 4 times to increase messageId to 4
        _ = new TransferMessage(CommandBuilder.ChannelSetRamp_Command(1, 'B', 70, 25));
        _ = new TransferMessage(CommandBuilder.ChannelSetRamp_Command(1, 'B', 70, 25));
        _ = new TransferMessage(CommandBuilder.ChannelSetRamp_Command(1, 'B', 70, 25));
        var transferMessage = new TransferMessage(CommandBuilder.ChannelSetRamp_Command(1, 'B', 70, 25));

        var messageToSend = transferMessage.Serialize();

        Assert.Equal(new List<byte> { 81, 4, 33, 82, 2, 70, 25, 72 }, messageToSend);
    }

    [Fact]
    public void Serialize_ChannelSetPulse()
    {
        //Create message 6 times to increase messageId to 6
        _ = new TransferMessage(CommandBuilder.ChannelSetPulse_Command(1, 'C', 125, 50, 12, 28, 75));
        _ = new TransferMessage(CommandBuilder.ChannelSetPulse_Command(1, 'C', 125, 50, 12, 28, 75));
        _ = new TransferMessage(CommandBuilder.ChannelSetPulse_Command(1, 'C', 125, 50, 12, 28, 75));
        _ = new TransferMessage(CommandBuilder.ChannelSetPulse_Command(1, 'C', 125, 50, 12, 28, 75));
        _ = new TransferMessage(CommandBuilder.ChannelSetPulse_Command(1, 'C', 125, 50, 12, 28, 75));
        var transferMessage = new TransferMessage(CommandBuilder.ChannelSetPulse_Command(1, 'C', 125, 50, 12, 28, 75));


        var messageToSend = transferMessage.Serialize();

        Assert.Equal(new List<byte> { 81, 6, 49, 80, 5, 75, 125, 50, 12, 28, 126 }, messageToSend);
    }

    [Fact]
    public void GetDataLength_Success()
    {
        var len = TransferMessage.GetDataLength(new byte[] { 1, 2, 3, 4, 5, 6 });

        Assert.NotNull(len);
        Assert.Equal((byte)5, len);
    }


    [Fact]
    public void GetDataLength_Null()
    {
        var len = TransferMessage.GetDataLength(new byte[] { 1, 2, 3 });

        Assert.Null(len);
    }

    [Theory]
    [InlineData(1, 1, 17)]
    [InlineData(8, 5, 88)]
    public void CreateAddressTest(byte qsuId, byte channelId, byte expectedAddress)
    {
        var result = CommandMessage.CreateAddress(qsuId, channelId);

        Assert.Equal(expectedAddress, result);

    }

    [Theory]
    [InlineData(17, 1, 1)]
    [InlineData(88, 8, 5)]
    public void GetIdFromAddressTest(byte address, byte expectedQsuId, byte expectedChannelId)
    {
        CommandMessage.GetIdsFromAddress(address, out var qsuId, out var channelId);

        Assert.Equal(expectedQsuId, qsuId);
        Assert.Equal(expectedChannelId, channelId);
    }
}
