using Psm32.Models;

namespace Psm32Tests.Models;

public class NuscleTest
{
    [Fact]
    public void Init_InvalidChannelLetter_Throws()
    {

        Action action = () => new Muscle(1, 'F');

        var exception = Assert.Throws<Exception>(action);

        Assert.NotNull(exception);
        Assert.Equal("Invalid Channel letter `F`", exception.Message);
    }

    [Fact]
    public void Init_InvalidDeviceId_Throws()
    {

        Action action = () => new Muscle(0, 'A');

        var exception = Assert.Throws<Exception>(action);

        Assert.NotNull(exception);
        Assert.Equal("Invalid Unit Id `0`", exception.Message);
    }

    [Fact]
    public void Init_Channel_Success()
    {

        var channel = new Muscle(1, 'A');

        Assert.Equal("1A", channel.ID);
        Assert.Equal('A', channel.Letter);
        Assert.Equal("", channel.MuscleConfig.Name.Name); //TODO: test this 
    }
}
