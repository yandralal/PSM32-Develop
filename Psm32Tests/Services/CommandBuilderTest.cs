using Psm32.Services;
using Psm32.Models;

namespace Psm32Tests.Services;

public class CommandBuilderTest
{
    [Fact]
    public void QsuReset_Command_InvalidUnitId_Throws()
    {
        Action action = () => CommandBuilder.QsuReset_Command(10);

        var exception = Assert.Throws<Exception>(action);

        Assert.NotNull(exception);
        Assert.Equal("Invalid unitId `10`. Must be between 1 and 8", exception.Message);
    }

    [Theory]
    [InlineData(null, 0)]
    [InlineData(1, 1)]
    public void QsuReset_Command_Success(int? unitId, byte qsuId)
    {
        var command = CommandBuilder.QsuReset_Command(unitId);
        var expected = new CommandMessage(qsuId: qsuId, channelId: 0, command: '*');

        Assert.True(command.Equals(expected));
    }


    [Fact]
    public void QsuEnum_Command_InvalidUnitId_Throws()
    {
        Action action = () => CommandBuilder.QsuEnum_Command(10);

        var exception = Assert.Throws<Exception>(action);

        Assert.NotNull(exception);
        Assert.Equal("Invalid unitId `10`. Must be between 1 and 8", exception.Message);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(8)]
    public void QsuEnum_Command_Success(int unitId)
    {
        var command = CommandBuilder.QsuEnum_Command(unitId);
        var expected = new CommandMessage(qsuId: (byte)unitId, channelId: 0, command: 'E');

        Assert.True(command.Equals(expected));
    }

    [Fact]
    public void ChannelGoIdle_Command_InvalidUnitId_Throws()
    {
        Action action = () => CommandBuilder.ChannelGoIdle_Command(10, null);

        var exception = Assert.Throws<Exception>(action);

        Assert.NotNull(exception);
        Assert.Equal("Invalid unitId `10`. Must be between 1 and 8", exception.Message);
    }

    [Fact]
    public void ChannelGoIdle_Command_InvalidChannelLetter_Throws()
    {
        Action action = () => CommandBuilder.ChannelGoIdle_Command(1, 'F');

        var exception = Assert.Throws<Exception>(action);

        Assert.NotNull(exception);
        Assert.Equal($"Invalid Channel letter `F`", exception.Message);
    }

    [Fact]
    public void ChannelGoIdle_Command_AllUnitsAllChannels_Success()
    {
        var command = CommandBuilder.ChannelGoIdle_Command(null, null);
        var expected = new CommandMessage(qsuId: 0, channelId: 0, command: 'I');

        Assert.True(command.Equals(expected));
    }

    [Fact]
    public void ChannelGoIdle_Command_OneChannel_Success()
    {
        var command = CommandBuilder.ChannelGoIdle_Command(1, 'B');
        var expected = new CommandMessage(qsuId: 1, channelId: 2, command: 'I');

        Assert.True(command.Equals(expected));
    }

    [Fact]
    public void ChannelGoRun_Command_InvalidUnitId_Throws()
    {
        Action action = () => CommandBuilder.ChannelGoRun_Command(10, null);

        var exception = Assert.Throws<Exception>(action);

        Assert.NotNull(exception);
        Assert.Equal("Invalid unitId `10`. Must be between 1 and 8", exception.Message);
    }

    [Fact]
    public void ChannelGoRun_Command_InvalidChannelLetter_Throws()
    {
        Action action = () => CommandBuilder.ChannelGoRun_Command(1, 'F');

        var exception = Assert.Throws<Exception>(action);

        Assert.NotNull(exception);
        Assert.Equal($"Invalid Channel letter `F`", exception.Message);
    }

    [Fact]
    public void ChannelGoRun_Command_AllUnitsAllChannels_Success()
    {
        var command = CommandBuilder.ChannelGoRun_Command(null, null);
        var expected = new CommandMessage(qsuId: 0, channelId: 0, command: 'G');

        Assert.True(command.Equals(expected));
    }

    [Fact]
    public void ChannelGoRun_Command_OneChannel_Success()
    {
        var command = CommandBuilder.ChannelGoRun_Command(1, 'B');
        var expected = new CommandMessage(qsuId: 1, channelId: 2, command: 'G');

        Assert.True(command.Equals(expected));
    }

    [Fact]
    public void ChannelGoOff_Command_InvalidUnitId_Throws()
    {
        Action action = () => CommandBuilder.ChannelGoOff_Command(10, null);

        var exception = Assert.Throws<Exception>(action);

        Assert.NotNull(exception);
        Assert.Equal("Invalid unitId `10`. Must be between 1 and 8", exception.Message);
    }

    [Fact]
    public void ChannelGoOff_Command_InvalidChannelLetter_Throws()
    {
        Action action = () => CommandBuilder.ChannelGoOff_Command(1, 'M');

        var exception = Assert.Throws<Exception>(action);

        Assert.NotNull(exception);
        Assert.Equal($"Invalid Channel letter `M`", exception.Message);
    }

    [Fact]
    public void ChannelGoOff_Command_AllUnitsAllChannels_Success()
    {
        var command = CommandBuilder.ChannelGoOff_Command(null, null);
        var expected = new CommandMessage(qsuId: 0, channelId: 0, command: 'F');

        Assert.True(command.Equals(expected));
    }

    [Fact]
    public void ChannelGoOff_Command_OneChannel_Success()
    {
        var command = CommandBuilder.ChannelGoOff_Command(1, 'B');
        var expected = new CommandMessage(qsuId: 1, channelId: 2, command: 'F');

        Assert.True(command.Equals(expected));
    }

    [Fact]
    public void ChannelSetRamp_Command_InvalidUnitId_Throws()
    {
        Action action = () => CommandBuilder.ChannelSetRamp_Command(10, null, 1, 1);

        var exception = Assert.Throws<Exception>(action);

        Assert.NotNull(exception);
        Assert.Equal("Invalid unitId `10`. Must be between 1 and 8", exception.Message);
    }

    [Fact]
    public void ChannelSetRamp_Command_InvalidChannelLetter_Throws()
    {
        Action action = () => CommandBuilder.ChannelSetRamp_Command(1, 'M', 1, 1);

        var exception = Assert.Throws<Exception>(action);

        Assert.NotNull(exception);
        Assert.Equal($"Invalid Channel letter `M`", exception.Message);
    }

    [Fact]
    public void ChannelSetRamp_Command_InvalidRampTargetPercent_Throws()
    {
        Action action = () => CommandBuilder.ChannelSetRamp_Command(1, 'B', 150, 1);

        var exception = Assert.Throws<Exception>(action);

        Assert.NotNull(exception);
        Assert.Equal($"Invalid Ramp Target Percent value: `150`. Must be between 0 and 100", exception.Message);
    }

    [Fact]
    public void ChannelSetRamp_Command_InvalidRampPeriod_Throws()
    {
        Action action = () => CommandBuilder.ChannelSetRamp_Command(1, 'B', 50, 70);

        var exception = Assert.Throws<Exception>(action);

        Assert.NotNull(exception);
        Assert.Equal($"Invalid Ramp Period value: `70`. Must be between 0 and 50", exception.Message);
    }

    [Fact]
    public void ChannelSetRamp_Command_AllUnitsAllChannels_Success()
    {
        var command = CommandBuilder.ChannelSetRamp_Command(null, null, 10, 30);
        var expected = new CommandMessage(qsuId: 0, channelId: 0, command: 'R', data: new List<byte>() { 10, 30 });

        Assert.True(command.Equals(expected));
    }

    [Fact]
    public void ChannelSetRamp_Command_OneChannel_Success()
    {
        var command = CommandBuilder.ChannelSetRamp_Command(1, 'B', 10, 30);
        var expected = new CommandMessage(qsuId: 1, channelId: 2, command: 'R', data: new List<byte>() { 10, 30 });

        Assert.True(command.Equals(expected));
    }

    [Fact]
    public void ChannelSetPulse_Command_InvalidUnitId_Throws()
    {
        Action action = () => CommandBuilder.ChannelSetPulse_Command(unitId: 10, letter: null, ampPos: 0, ampNeg: 0, pwPos: 0, pwNeg: 0, frequency: 0);

        var exception = Assert.Throws<Exception>(action);

        Assert.NotNull(exception);
        Assert.Equal("Invalid unitId `10`. Must be between 1 and 8", exception.Message);
    }

    [Fact]
    public void ChannelSetPulse_Command_InvalidChannelLetter_Throws()
    {
        Action action = () => CommandBuilder.ChannelSetPulse_Command(unitId: 1, letter: 'M', ampPos: 0, ampNeg: 0, pwPos: 0, pwNeg: 0, frequency: 0);

        var exception = Assert.Throws<Exception>(action);

        Assert.NotNull(exception);
        Assert.Equal($"Invalid Channel letter `M`", exception.Message);
    }

    [Fact]
    public void ChannelSetPulse_Command_InvalidAmpPos_Throws()
    {
        Action action = () => CommandBuilder.ChannelSetPulse_Command(unitId: 1, letter: 'B', ampPos: 300, ampNeg: 0, pwPos: 0, pwNeg: 0, frequency: 0);

        var exception = Assert.Throws<Exception>(action);

        Assert.NotNull(exception);
        Assert.Equal($"Invalid Amp+ value: `300`. Must be between 0 and 250", exception.Message);
    }

    [Fact]
    public void ChannelSetPulse_Command_InvalidAmpNeg_Throws()
    {
        Action action = () => CommandBuilder.ChannelSetPulse_Command(unitId: 1, letter: 'B', ampPos: 200, ampNeg: -2, pwPos: 0, pwNeg: 0, frequency: 0);

        var exception = Assert.Throws<Exception>(action);

        Assert.NotNull(exception);
        Assert.Equal($"Invalid Amp- value: `-2`. Must be between 0 and 250", exception.Message);
    }

    [Fact]
    public void ChannelSetPulse_Command_InvalidPwPos_Throws()
    {
        Action action = () => CommandBuilder.ChannelSetPulse_Command(unitId: 1, letter: 'B', ampPos: 200, ampNeg: 100, pwPos: 50, pwNeg: 0, frequency: 10);

        var exception = Assert.Throws<Exception>(action);

        Assert.NotNull(exception);
        Assert.Equal($"Invalid PulseWidth+ value: `50`. Must be between 0 and 30", exception.Message);
    }

    [Fact]
    public void ChannelSetPulse_Command_InvalidPwNeg_Throws()
    {
        Action action = () => CommandBuilder.ChannelSetPulse_Command(unitId: 1, letter: 'B', ampPos: 200, ampNeg: 100, pwPos: 1, pwNeg: -5, frequency: 0);

        var exception = Assert.Throws<Exception>(action);

        Assert.NotNull(exception);
        Assert.Equal($"Invalid PulseWidth- value: `-5`. Must be between 0 and 30", exception.Message);
    }

    [Fact]
    public void ChannelSetPulse_Command_InvalidFrequency_Throws()
    {
        Action action = () => CommandBuilder.ChannelSetPulse_Command(unitId: 1, letter: 'B', ampPos: 200, ampNeg: 100, pwPos: 1, pwNeg: 2, frequency: 0);

        var exception = Assert.Throws<Exception>(action);

        Assert.NotNull(exception);
        Assert.Equal($"Invalid Frequency value: `0`. Must be between 1 and 100", exception.Message);
    }

    [Fact]
    public void ChannelSetPulse_Command_AllUnitsAllChannels_Success()
    {
        var command = CommandBuilder.ChannelSetPulse_Command(unitId: null, letter: null, ampPos: 200, ampNeg: 100, pwPos: 1, pwNeg: 2, frequency: 20);
        var expected = new CommandMessage(qsuId: 0, channelId: 0, command: 'P', data: new List<byte>() { 20, 200, 100, 1, 2 });

        Assert.True(command.Equals(expected));
    }

    [Fact]
    public void ChannelSetPulse_Command_OneChannel_Success()
    {
        var command = CommandBuilder.ChannelSetPulse_Command(unitId: 1, letter: 'B', ampPos: 200, ampNeg: 100, pwPos: 1, pwNeg: 2, frequency: 20);
        var expected = new CommandMessage(qsuId: 1, channelId: 2, command: 'P', data: new List<byte>() { 20, 200, 100, 1, 2 });

        Assert.True(command.Equals(expected));
    }
}
