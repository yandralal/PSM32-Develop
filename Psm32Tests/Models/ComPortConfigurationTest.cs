using Psm32.Models;

namespace Psm32Tests.Models;

public class ComPortConfigurationTest
{
    [Fact]
    public void ToDTODictionary_Success()
    {
        var expected = new Dictionary<string, string>
        {
            {"PortName", "COM3" },
            {"BoudRate", "9600" },
            {"ReadTimeOut", "500"},
            {"WriteTimeOut", "500"},
        };

        var config = new ComPortConfiguration();

        var actual = config.ToDTODictionary();

        Assert.Equal(expected.Count, actual.Count);
        Assert.False(expected.Except(actual).Any());
    }

    [Fact]
    public void FromDTODictionary_Success()
    {
        var configDict = new Dictionary<string, string>
        {
            {"PortName", "COM3" },
            {"BoudRate", "9600" },
            {"ReadTimeOut", "500"},
            {"WriteTimeOut", "500"},
        };

        var actual = ComPortConfiguration.FromDTODictionary(configDict);
        var expected = new ComPortConfiguration();

        Assert.True(expected.Equals(actual));
    }

    [Fact]
    public void FromDTODictionary_ExtraItems_Success()
    {
        var configDict = new Dictionary<string, string>
        {
            {"PortName", "COM3" },
            {"BoudRate", "9600" },
            {"ReadTimeOut", "500"},
            {"WriteTimeOut", "500"},
            {"ExtraItem", "SomeValue" }
        };

        var actual = ComPortConfiguration.FromDTODictionary(configDict);
        var expected = new ComPortConfiguration();

        Assert.True(expected.Equals(actual));
    }

    [Fact]
    public void FromDTODictionary_MissingItems_Success()
    {
        var configDict = new Dictionary<string, string>
        {
            {"BoudRate", "9600" },
            {"ReadTimeOut", "500"},
            {"WriteTimeOut", "500"},
        };

        var actual = ComPortConfiguration.FromDTODictionary(configDict);
        var expected = new ComPortConfiguration();

        Assert.True(expected.Equals(actual));
    }
}
