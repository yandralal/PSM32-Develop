using System.Collections.Generic;
using System.Linq;

namespace Psm32.Models;



public class AppConfiguration
{
    public ComPortConfiguration ComPortConfiguration { get; set; }

    public AppConfiguration(ComPortConfiguration? comPortConfiguration = null)
    {
        ComPortConfiguration = comPortConfiguration ?? new ComPortConfiguration();
    }

    public static AppConfiguration CreateDefaultConfiguration()
    {
        return new AppConfiguration();
    }

    public Dictionary<string, string> ToDTODictionary()
    {
        var dtoDict = new Dictionary<string, string>();

        return dtoDict.Concat(ComPortConfiguration.ToDTODictionary())
            .ToLookup(x => x.Key, x => x.Value)
            .ToDictionary(x => x.Key, g => g.First());
    }

    public override bool Equals(object? obj)
    {
        AppConfiguration? config = obj as AppConfiguration;
        
        if (config == null)
        {
            return false;
        }

        return ComPortConfiguration.Equals(config.ComPortConfiguration);
    }

    public override int GetHashCode()
    {
        return ComPortConfiguration.GetHashCode();
    }

    public static AppConfiguration FromDTODictionary(Dictionary<string, string> configuration)
    {
        return new AppConfiguration(ComPortConfiguration.FromDTODictionary(configuration));
    }

    public static List<string> GetConfigItemNames()
    {
        return CreateDefaultConfiguration().ToDTODictionary().Keys.ToList();
    }

    public static string? GetDefaultValueForConfigItem(string itemName)
    {
        var configDict = CreateDefaultConfiguration().ToDTODictionary();
     
        return configDict.ContainsKey(itemName) ? configDict[itemName] : null;
    }
}
