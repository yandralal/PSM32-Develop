
using Psm32.Models;

namespace Psm32Tests.Models;

public class QSUnitTest
{
    [Fact]
    public void Init_InvalidDeviceId_Throws()
    {

        Action action = () => new QSUnit(9);

        var exception = Assert.Throws<Exception>(action);

        Assert.NotNull(exception);
        Assert.Equal("Invalid Unit Id `9`", exception.Message);
    }

    [Fact]
    public void Init_QSUnit_Success()
    {
        var unit = new QSUnit(1);

        Assert.Equal(1, unit.ID);
        Assert.Equal(UnitStatus.Ok, unit.Status);
    }
}