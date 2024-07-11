using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Psm32.ViewModels;

namespace Psm32.Stores;

interface INavigationStore
{
}

public abstract class NavigationStoreBase : INavigationStore
{
    public ViewModelBase? CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel?.Dispose();
            _currentViewModel = value;
            OnCurrentViewModelChanged();
        }
    }

    private void OnCurrentViewModelChanged()
    {
        CurrentViewModelChanged?.Invoke();
    }


    public event Action? CurrentViewModelChanged;

    private ViewModelBase? _currentViewModel;
}
