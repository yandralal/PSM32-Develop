using Psm32.Models;

namespace Psm32Tests.Models;

public class AppConfigurationTest
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

        var config = new AppConfiguration();

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

        var actual = AppConfiguration.FromDTODictionary(configDict);
        var expected = new AppConfiguration();

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

        var actual = AppConfiguration.FromDTODictionary(configDict);
        var expected = new AppConfiguration();

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

        var actual = AppConfiguration.FromDTODictionary(configDict);
        var expected = new AppConfiguration();

        Assert.True(expected.Equals(actual));
    }

    [Fact]
    public void GetConfigItemNames_Success()
    {
        var expected = new List<string> { "PortName", "BoudRate", "ReadTimeOut", "WriteTimeOut" };
        var actual = AppConfiguration.GetConfigItemNames();

        Assert.True(Enumerable.SequenceEqual(expected, actual));

    }

    [Fact]
    public void GetDefaultValueForConfigItem_ItemExists_Success()
    {
        var actual = AppConfiguration.GetDefaultValueForConfigItem("WriteTimeOut");

        Assert.Equal("500", actual);
    }

    [Fact]
    public void GetDefaultValueForConfigItem_ItemDoesntExist_Success()
    {
        var actual = AppConfiguration.GetDefaultValueForConfigItem("SomeItem");

        Assert.Null(actual);
    }
}
