using Psm32.Commands;
using Psm32.Models;
using Psm32.Services;
using Psm32.Stores;
using System.Windows.Input;

namespace Psm32.ViewModels;

public class SystemSettingsViewModel:ViewModelBase
{
    private string? _comPort;
    public string? ComPort {
        get => _comPort; 
        set { 
            _comPort = value; 
            OnPropertyChanged(nameof(ComPort)); 
        }
    }

    private int _comBoud;
    public int ComBoud
    {
        get => _comBoud;
        set
        {
            _comBoud = value;
            OnPropertyChanged(nameof(ComBoud));
        }
    }

    private int _logDays;
    public int LogDays
    {
        get => _logDays;
        set
        {
            _logDays = value;
            OnPropertyChanged(nameof(LogDays));
        }
    }

    public ICommand SaveCommand { get;  }   
    public ICommand CancelCommand { get; }

    public SystemSettingsViewModel(DeviceStore deviceStore, INavigationService qSUnitsNavigationService)
    {
        _comPort = deviceStore.AppConfig.ComPortConfiguration.PortName;
        _comBoud = deviceStore.AppConfig.ComPortConfiguration.BoudRate;

        SaveCommand = new SaveSystemSettingsCommand(this, qSUnitsNavigationService);
        CancelCommand = new NavigateCommand<SystemSettingsViewModel, LoginViewModel>(qSUnitsNavigationService);
    }
}
