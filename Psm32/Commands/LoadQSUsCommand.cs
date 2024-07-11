using Psm32.Exceptions;
using Psm32.Stores;
using Psm32.ViewModels;
using System.Threading.Tasks;
using System.Windows;

namespace Psm32.Commands;

public class LoadQSUsCommand : AsyncCommandBase
{
    private readonly DeviceStore _deviceStore;
    private readonly QSUnitsViewModel _qsUnitsViewModel;

    public LoadQSUsCommand(QSUnitsViewModel qsUnitsViewModel, DeviceStore deviceStore)
    {
        _deviceStore = deviceStore;
        _qsUnitsViewModel = qsUnitsViewModel;
    }
    public override async Task ExecuteAsync(object? parameter)
    {
        try
        {
            await _deviceStore.LoadQSUs();
            _qsUnitsViewModel.UpdateQSUs(_deviceStore.QSUnits);
        }
        catch (QSUException ex)
        {
            MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
