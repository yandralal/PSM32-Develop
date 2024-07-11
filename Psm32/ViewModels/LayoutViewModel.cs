using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psm32.ViewModels;

internal class LayoutViewModel : ViewModelBase
{
    public TopNavigationBarViewModel TopNavigationBarViewModel { get; }
    public SubNavigationBarViewModel SubNavigationBarViewModel { get; }
    public ViewModelBase ContentViewModel { get; }

    public LayoutViewModel(TopNavigationBarViewModel topNavigationBarViewModel, SubNavigationBarViewModel subNavigationBarViewModel,  ViewModelBase contentViewModel)
    {
        TopNavigationBarViewModel = topNavigationBarViewModel;
        SubNavigationBarViewModel = subNavigationBarViewModel;
        ContentViewModel = contentViewModel;
    }

    public override void Dispose()
    {
        TopNavigationBarViewModel.Dispose();
        SubNavigationBarViewModel.Dispose();
        ContentViewModel.Dispose();

        base.Dispose();
    }
}
