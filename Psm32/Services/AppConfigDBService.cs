using Microsoft.EntityFrameworkCore;
using Psm32.DB;
using Psm32.Models;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Psm32.Services;

public interface IAppConfigDBService
{
    Task Save(AppConfiguration appConfiguration);
    Task<AppConfiguration> Load();
}
public class AppConfigDBService: IAppConfigDBService
{
    private readonly Psm32DbContextFactory _psm32DbContextFactory;
    private readonly ILogger _logger;

    public AppConfigDBService(
        Psm32DbContextFactory psm32DbContextFactory,
        ILogger logger)
    {
        _psm32DbContextFactory = psm32DbContextFactory;
        _logger = logger;
    }

    public async Task<AppConfiguration> Load()
    {
        using var dbContext = _psm32DbContextFactory.CreateDbContext();

        await SaveMissingConfiguration(dbContext);

        var configDict = await dbContext.AppConfig.ToDictionaryAsync(c => c.Key, c => c.Value);

        return ToAppConfiguration(configDict);
    }

    public async Task Save(AppConfiguration appConfiguration)
    {
        List<ConfigurationItemDTO> configuration = ToConfigurationItemDTOList(appConfiguration);

        using var dbContext = _psm32DbContextFactory.CreateDbContext();

        await SaveConfigurationItems(dbContext, configuration);
    }

    public async Task<AppConfiguration> GetAppConfiguration()
    {
        using var dbContext = _psm32DbContextFactory.CreateDbContext();
        var configDict = await dbContext.AppConfig.ToDictionaryAsync(c => c.Key, c => c.Value);

        return ToAppConfiguration(configDict);
    }

    private async Task SaveMissingConfiguration(Psm32DbContext dbContext)
    {
        var missingConfigItems = AppConfiguration.GetConfigItemNames()
                   .Where(itemName => !dbContext.AppConfig.Any(configItem => itemName == configItem.Key));

        var configItems = new List<ConfigurationItemDTO>();

        foreach (var missingConfigItem in missingConfigItems)
        {
            string? value = AppConfiguration.GetDefaultValueForConfigItem(missingConfigItem);
            if (value == null)
            {
                _logger.Error($"Missing Config value for item `{missingConfigItem}`");
                continue;
            }
            
            configItems.Add(new ConfigurationItemDTO(missingConfigItem, value));
        }

        await SaveConfigurationItems(dbContext, configItems);
    }

    private static async Task SaveConfigurationItems(Psm32DbContext dbContext, List<ConfigurationItemDTO> configuration)
    {
        dbContext.AppConfig.AddRange(configuration.ToArray());

        await dbContext.SaveChangesAsync();
    }

    private static List<ConfigurationItemDTO> ToConfigurationItemDTOList(AppConfiguration appConfiguration)
    {
        return appConfiguration
            .ToDTODictionary()
            .Select(kvp => new ConfigurationItemDTO(kvp.Key, kvp.Value))
            .ToList();
    }

    private static AppConfiguration ToAppConfiguration(Dictionary<string, string> configDict)
    {
        return AppConfiguration.FromDTODictionary(configDict);
    }
}
