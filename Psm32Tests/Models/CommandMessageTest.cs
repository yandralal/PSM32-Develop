using Psm32.Models;
using Psm32.Exceptions;
using System.Text;

namespace Psm32Tests.Models;

public class CommandMessageTest
{
    [Fact]
    public void Init_InvalidQsuId_Throws()
    {

        Action action = () => new CommandMessage(10, 3, 'F');

        var exception = Assert.Throws<CommandMessageException>(action);

        Assert.NotNull(exception);
        Assert.Equal("Invalid QSU ID, given `10`, must be between 0 and 8", exception.Message);
    }

    [Fact]
    public void Init_InvalidChannelId_Throws()
    {

        Action action = () => new CommandMessage(1, 6, 'F');

        var exception = Assert.Throws<CommandMessageException>(action);

        Assert.NotNull(exception);
        Assert.Equal("Invalid Channel ID, given `6`, must be between 0 and 4", exception.Message);
    }

    [Fact]
    public void Init_InvalidCommand_Throws()
    {

        Action action = () => new CommandMessage(1, 1, 'B');

        var exception = Assert.Throws<CommandMessageException>(action);

        Assert.NotNull(exception);
        Assert.Equal("Command `B` is not a valid command", exception.Message);
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

    [Fact]
    public void GetMessage_Success()
    {
        var data = new List<byte> { 1, 2, 3, 4, 5 };
        var messageCommand = new CommandMessage(1, 3, 'F', data);
        var expected = new List<byte> { 49, 70, 5, 1, 2, 3, 4, 5 };

        var messageToSend = messageCommand.Serialize();

        Assert.Equal(expected, messageToSend);
    }


    [Fact]
    public void Parse_NoData_Success()
    {
        byte[] message = { 49, 70, 0 };
        var expected = new CommandMessage(1, 3, 'F');

        var command = CommandMessage.Parse(message);

        Assert.True(Object.Equals(expected, command));
    }

    [Fact]
    public void Parse_WithData_Success()
    {
        var data = new List<byte> { 1, 2, 3, 4, 5 };
        byte[] message = { 49, 70, 5, 1, 2, 3, 4, 5 };
        var expected = new CommandMessage(1, 3, 'F', data);


        var command = CommandMessage.Parse(message);

        Assert.True(Object.Equals(expected, command));
    }

    [Fact]
    public void Parse_Throws()
    {
        byte[] message = { 49, 66, 5, 1, 2, 3, 4, 5 };

        Action action = () => CommandMessage.Parse(message);

        var exception = Assert.Throws<CommandMessageException>(action);

        Assert.NotNull(exception);
        Assert.Equal("Command `B` is not a valid command", exception.Message);
    }

    [Fact]
    public void GetDataLength_Success()
    {
        var len = CommandMessage.GetDataLength(new byte[] { 1, 2, 3, 4 });

        Assert.NotNull(len);
        Assert.Equal((byte)3, len);
    }

    [Fact]
    public void GetDataLength_Null()
    {
        var len = CommandMessage.GetDataLength(new byte[] { 1 });

        Assert.Null(len);
    }
}
