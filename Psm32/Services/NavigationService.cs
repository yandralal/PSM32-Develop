using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Psm32.Stores;
using Psm32.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psm32.Services;

public interface INavigationService
{
    void Navigate();
}

public class NavigationService<TViewModel, TLogInViewModel>  : INavigationService
        where TViewModel : ViewModelBase
        where TLogInViewModel : LoginViewModel
{
    public NavigationService(
        DeviceStore deviceStore, 
        NavigationStore navigtionStore, 
        Func<TViewModel> CreateViewModel,
        Func<TLogInViewModel> CreateLoginViewModel)
    {
        _deviceStore = deviceStore;
        _naviagtionStore = navigtionStore;
        _createViewModel = CreateViewModel;
        _createLoginViewModel = CreateLoginViewModel;
    }
    public void Navigate()
    {
        if (_deviceStore.CurrentSession.LoggedIn())
        {
            _naviagtionStore.CurrentViewModel = _createViewModel();
        }
        else
        {
            _naviagtionStore.CurrentViewModel = _createLoginViewModel();
        }
    }

    private readonly DeviceStore _deviceStore;
    private readonly NavigationStore _naviagtionStore;
    private readonly Func<TViewModel> _createViewModel;
    private readonly Func<TLogInViewModel> _createLoginViewModel;
}
