using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Psm32.Stores;
using Psm32.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psm32.Services;


public class LayoutNavigationService<TViewModel>  : INavigationService
        where TViewModel : ViewModelBase
{
    public LayoutNavigationService(
        DeviceStore deviceStore, 
        NavigationStore navigtionStore, 
        Func<TViewModel> createViewModel,
        Func<LoginViewModel> createLoginViewModel,
        Func<TopNavigationBarViewModel> createTopNavigationBarViewModel,
        Func<SubNavigationBarViewModel> createSubNavigationBarViewModel)
    {
        _deviceStore = deviceStore;
        _naviagtionStore = navigtionStore;
        _createViewModel = createViewModel;
        _createLoginViewModel = createLoginViewModel;
        _createTopNavigationBarViewModel = createTopNavigationBarViewModel;
        _createSubNavigationBarViewModel = createSubNavigationBarViewModel;
    }
    public void Navigate()
    {
        if (_deviceStore.CurrentSession.LoggedIn())
        {
            //TODO: move to dep injection?
            _naviagtionStore.CurrentViewModel = new LayoutViewModel(_createTopNavigationBarViewModel(), _createSubNavigationBarViewModel(), _createViewModel());
        }
        else
        {
            _naviagtionStore.CurrentViewModel = new LayoutViewModel(_createTopNavigationBarViewModel(), _createSubNavigationBarViewModel(), _createLoginViewModel());
        }
    }

    private readonly DeviceStore _deviceStore;
    private readonly NavigationStore _naviagtionStore;
    private readonly Func<TViewModel> _createViewModel;
    private readonly Func<LoginViewModel> _createLoginViewModel;
    private readonly Func<TopNavigationBarViewModel> _createTopNavigationBarViewModel;
    private readonly Func<SubNavigationBarViewModel> _createSubNavigationBarViewModel;
}
