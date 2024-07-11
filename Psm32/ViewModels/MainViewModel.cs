using Psm32.Commands;
using Psm32.Models;
using Psm32.Services;
using Psm32.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Psm32.ViewModels;

public class MainViewModel : ViewModelBase
{
    public ViewModelBase? CurrentViewModel { get => _nagivationStore?.CurrentViewModel; }
    public bool IsLoggedIn { get => _deviceStore.CurrentSession.LoggedIn(); }

    public ICommand UserClickedCommand { get; }

    public MainViewModel(
        DeviceStore deviceStore, 
        NavigationStore navigationStore,
        //ModalNavigationStore modalNavigationStore,
        INavigationService loginNavigationService)
    {
        _nagivationStore = navigationStore;
        //_modalNavigationStore = modalNavigationStore;
        _deviceStore = deviceStore;
        _nagivationStore.CurrentViewModelChanged += OnCurrentModelChanged;

        UserClickedCommand = new UserClickedCommand(
            _deviceStore,
            loginNavigationService);
    }

    public override void Dispose()
    {
        _nagivationStore.CurrentViewModelChanged -= OnCurrentModelChanged;

        base.Dispose();
    }

    public static MainViewModel LoadViewModel(DeviceStore deviceStore,
        NavigationStore navigationStore,
        //ModalNavigationStore modalNavigationStore,
        INavigationService loginNavigationService)
    {
        return new(deviceStore, navigationStore,/* modalNavigationStore,*/ loginNavigationService);
    }


    private void OnCurrentModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }

    private readonly NavigationStore _nagivationStore;
    //private readonly ModalNavigationStore _modalNavigationStore;
    private readonly DeviceStore _deviceStore;
}
