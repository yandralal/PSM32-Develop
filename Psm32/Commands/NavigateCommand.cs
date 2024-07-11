using Psm32.Models;
using Psm32.Services;
using Psm32.Stores;
using Psm32.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psm32.Commands;

public class NavigateCommand<TViewModel, TLoginViewModel> : CommandBase 
    where TViewModel : ViewModelBase
    where TLoginViewModel : LoginViewModel
{
    public NavigateCommand(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }
    public override void Execute(object? parameter)
    {
        _navigationService.Navigate();
    }

    private readonly INavigationService _navigationService;
}
