using Psm32.Exceptions;
using Psm32.Stores;
using Psm32.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Psm32.Commands;

public class ScanQSUsCommand : AsyncCommandBase
{
    public ScanQSUsCommand(DeviceStore deviceStore, SubNavigationBarViewModel subNavigationBarViewModel)
    {
        _deviceStore = deviceStore;
        _subNavigationBarViewModel = subNavigationBarViewModel;
    }
    public override async Task ExecuteAsync(object? parameter)
    {
        try
        {
            await _deviceStore.ScanQSUs();
            _deviceStore.LastScannedTime = DateTime.Now;
            _subNavigationBarViewModel.OnLastScannedTimeChanged();


            MessageBox.Show("Successfully Scanned Units", "Success",
               MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (QSUException ex)
        {
            MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private readonly DeviceStore _deviceStore;
    private readonly SubNavigationBarViewModel _subNavigationBarViewModel;
}
