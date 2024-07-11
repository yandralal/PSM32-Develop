using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Psm32.Commands;
using Psm32.Models;
using Psm32.Services;
using Psm32.Stores;

namespace Psm32.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private string _username;
    public string Username
    {
        get
        {
            return _username;
        }
        set
        {
            _username = value;
            _errorMsg = "";
            OnPropertyChanged(nameof(Username));
            OnPropertyChanged(nameof(ErrorMsg));
        }
    }

    private string _password;
    private readonly DeviceStore _deviceStore;
    private readonly INavigationService _qsUnitsNavigationService;

    public string Password
    {
        get
        {
          
            return _password;
        }
        set
        {
            _password = value;
            _errorMsg = "";
            OnPropertyChanged(nameof(Password));
            OnPropertyChanged(nameof(ErrorMsg));
        }
    }

    private string _errorMsg;
    public string ErrorMsg
    {
        get
        {
            return _errorMsg;
        }
        set
        {
            _errorMsg = value;
            OnPropertyChanged(nameof(ErrorMsg));
        }
    }

    public LoginViewModel(
        DeviceStore deviceStore,
        INavigationService qsUnitsNavigationService)
    {
        _username = "";
        _password = "";
        _errorMsg = "";
        _deviceStore = deviceStore;
        _qsUnitsNavigationService = qsUnitsNavigationService;

        LoginCommand = new LoginCommand(
            _deviceStore,
            this,
            _qsUnitsNavigationService);

    }

    public ICommand LoginCommand { get; }

    public static LoginViewModel LoadViewModel(
        DeviceStore deviceStore,
        INavigationService qsUnitsNavigationService
        )
    {
        return new LoginViewModel(
            deviceStore, 
            qsUnitsNavigationService
            );
    }
}
