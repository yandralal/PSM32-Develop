using Psm32.Models;
using Psm32.Stores;
using Psm32.ViewModels;
using System.Threading.Tasks;
using System.Windows;

namespace Psm32.Commands;

public class LoadMotorTasksCommand : AsyncCommandBase
{
    private readonly DeviceStore _deviceStore;
    private readonly MotorTasksViewModel _motorTasksViewModel;

    public LoadMotorTasksCommand(DeviceStore deviceStore, MotorTasksViewModel motorTasksViewModel)
    {
        _deviceStore = deviceStore;
        _motorTasksViewModel = motorTasksViewModel;
    }

    public override async Task ExecuteAsync(object? parameter)
    {
        try
        {
            await _deviceStore.LoadMotorTasks();
            _motorTasksViewModel.UpdateMotorTasks(_deviceStore.MotorTasks);
        }
        catch
        {
            MessageBox.Show("Failed to load Motor Tasks","Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
