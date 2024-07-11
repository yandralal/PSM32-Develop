using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Psm32.Exceptions;
using Psm32.Models;
using Psm32.Services;
using Psm32.Stores;
using Psm32.ViewModels;

namespace Psm32.Commands;

public class LoginCommand : AsyncCommandBase
{
    private readonly DeviceStore _deviceStore;
    private readonly LoginViewModel _loginViewModel;
    private readonly INavigationService _qsUnitsViewNavigationService;

    public LoginCommand(
        DeviceStore deviceStore,
        LoginViewModel loginViewModel,
        INavigationService qsUnitsViewNavigationService)
    {
        _deviceStore = deviceStore;
        _loginViewModel = loginViewModel;
        _qsUnitsViewNavigationService = qsUnitsViewNavigationService;
    }

    public override async Task ExecuteAsync(object? parameter)
    {
        try { 
            var user = await _deviceStore.UserLogin(
                _loginViewModel.Username,
                _loginViewModel.Password
                );

                _deviceStore.CurrentSession.StartSession(user);
                _qsUnitsViewNavigationService.Navigate();
        }
        catch (AuthenticationServiceException e)
        {
            _loginViewModel.ErrorMsg = e.Message;
        }
    }
}
