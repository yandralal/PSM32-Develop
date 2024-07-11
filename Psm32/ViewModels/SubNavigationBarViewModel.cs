using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Psm32.Commands;
using Psm32.Stores;

namespace Psm32.ViewModels;

public class SubNavigationBarViewModel : ViewModelBase
{
    DeviceStore _deviceStore;

    public ICommand ScanQSUsCommand { get; }
    public ICommand OpenMotorTaskCommand { get; }

    public string LastScannedTime => _deviceStore.LastScannedTime.ToString();


    public bool IsLoggedIn => _deviceStore.CurrentSession.LoggedIn();

    public SubNavigationBarViewModel(DeviceStore deviceStore)
    {
        _deviceStore = deviceStore;
        ScanQSUsCommand = new ScanQSUsCommand(_deviceStore, this);
        OpenMotorTaskCommand = new OpenMotorTaskCommand(_deviceStore);
    }

    public static SubNavigationBarViewModel LoadViewModel(DeviceStore deviceStore)
    {
        return new SubNavigationBarViewModel(deviceStore);
    }

    public void OnLastScannedTimeChanged()
    {
        OnPropertyChanged(nameof(LastScannedTime));
    }
}
