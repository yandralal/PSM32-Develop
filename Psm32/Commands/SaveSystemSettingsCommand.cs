using Microsoft.Extensions.Configuration;
using Psm32.Models;
using Psm32.Services;
using Psm32.Stores;
using Psm32.ViewModels;

namespace Psm32.Commands;

public class SaveSystemSettingsCommand : CommandBase
{
    private readonly SystemSettingsViewModel _systemSettings;
    private readonly INavigationService _qSUnitsNavigationService;

    public SaveSystemSettingsCommand(
        SystemSettingsViewModel systemSettings, 
        INavigationService qSUnitsNavigationService)
    {
        _systemSettings = systemSettings;
        _qSUnitsNavigationService = qSUnitsNavigationService;
    }
    public override void Execute(object? parameter)
    {
        var configuration = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json");

        var config = configuration.Build();
        var systemSettings = config.GetSection("SystemSettings");

        systemSettings["ComPort"] = _systemSettings.ComPort;
        systemSettings["ComBoud"] = _systemSettings.ComBoud.ToString();
        systemSettings["LogDays"] = _systemSettings.LogDays.ToString();

        _qSUnitsNavigationService.Navigate();
    }
}
