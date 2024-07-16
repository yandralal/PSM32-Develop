using Psm32.Commands;
using Psm32.Models;
using Psm32.Services;
using Psm32.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Psm32.ViewModels;

public class QSUnitsViewModel:ViewModelBase
{
    public ICommand ScanCommand { get; }
    public ICommand LoadQSUsCommand { get; }

    public IEnumerable<QSUnitViewModel> UnitsLeft => _units.Where(u => u.ID % 2 != 0).ToList();
    public IEnumerable<QSUnitViewModel> UnitsRight => _units.Where(u => u.ID % 2 == 0).ToList();

    public bool CanCreateMotorTask => true;

    private readonly DeviceStore _deviceStore;

    public QSUnitsViewModel(DeviceStore deviceStore, INavigationService muscleTestNavigationService)
    {
        _deviceStore = deviceStore;
        _units = new ObservableCollection<QSUnitViewModel>();
       
        //ScanCommand = new ScanQSUsCommand(this, _deviceStore);
        LoadQSUsCommand = new LoadQSUsCommand(this, deviceStore);

        _deviceStore.QSUnitsScanned += OnQSUnitsScanned;
    }

    public override void Dispose()
    {
        _deviceStore.QSUnitsScanned -= OnQSUnitsScanned;
        base.Dispose();
    }

    public static QSUnitsViewModel LoadViewModel(DeviceStore deviceStore, INavigationService muscleTestNavigationService)
    {
        QSUnitsViewModel qsUnitsViewModel = new(deviceStore, muscleTestNavigationService);
        qsUnitsViewModel.LoadQSUsCommand.Execute(qsUnitsViewModel);

        return qsUnitsViewModel;
    }

    public void UpdateQSUs(IEnumerable<QSUnit> qsUnits)
    {
        _units.Clear();
        foreach (var deviceUnit in qsUnits)
        {
            _units.Add(new QSUnitViewModel(deviceUnit));
        }
    }

    private void OnQSUnitsScanned(IEnumerable<QSUnit> qsUnits)
    {
        UpdateQSUs(qsUnits);
    }

    private readonly ObservableCollection<QSUnitViewModel> _units;
}
