using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Microsoft.Win32;
using Psm32.Models;
using Psm32.Services;
using Psm32.Stores;
using Psm32.ViewModels;

namespace Psm32.Commands;

public class UserClickedCommand : CommandBase
{
    private readonly DeviceStore _deviceStore;
    private readonly INavigationService _loginViewNavigationService;

    public UserClickedCommand(
        DeviceStore deviceStore,
        INavigationService loginViewNavigationService)
    {
        _deviceStore = deviceStore;
        _loginViewNavigationService = loginViewNavigationService;
    }

    public override void Execute(object? parameter)
    {
        var session = _deviceStore.CurrentSession;

        if (session.LoggedIn())
        {
            session.EndSession();
            _loginViewNavigationService.Navigate();
        }
        else
        {
            _loginViewNavigationService.Navigate();
        }
    }
}
