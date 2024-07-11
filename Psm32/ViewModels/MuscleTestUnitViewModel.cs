using Psm32.Models;
using Psm32.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psm32.ViewModels;

public class MuscleTestUnitViewModel : ViewModelBase   
{
    public int ID => _id;
    public IEnumerable<MuscleTestChannelViewModel> MuscleTestUnit => _muscleTestViewModel;

    private readonly int _id;
    private readonly ObservableCollection<MuscleTestChannelViewModel> _muscleTestViewModel;
    private readonly DeviceStore _deviceStore;
    private readonly QSUnit muscleTest;

    public MuscleTestUnitViewModel(DeviceStore deviceStore, QSUnit muscleTest)
    {
        _id = muscleTest.ID;
        _deviceStore = deviceStore;
        this.muscleTest = muscleTest;
        _muscleTestViewModel = new ObservableCollection<MuscleTestChannelViewModel>();

        _muscleTestViewModel.CollectionChanged += MuscleTestUnitChanged;


        var item = ToChannelViewModel(muscleTest.ChannelA);

        _muscleTestViewModel.Add(item);
        _muscleTestViewModel.Add(ToChannelViewModel(muscleTest.ChannelB));
        _muscleTestViewModel.Add(ToChannelViewModel(muscleTest.ChannelC));
        _muscleTestViewModel.Add(ToChannelViewModel(muscleTest.ChannelD));
    }

    public override void Dispose()
    {
        foreach (var item in _muscleTestViewModel)
        {
            item.PropertyChanged -= ChannelChanged;
        }
        _muscleTestViewModel.CollectionChanged -= MuscleTestUnitChanged;
        base.Dispose();
    }

    private void OnChannelChanged(object? sender, PropertyChangedEventArgs e)
    {
        var channelViewModel = sender as MuscleTestChannelViewModel;

        if (channelViewModel == null)
        {
            return;
        }

        if (channelViewModel.ID == muscleTest.ChannelA.ID)
        {
            muscleTest.ChannelA = channelViewModel.ToChannel();
        }
        else if (channelViewModel.ID == muscleTest.ChannelB.ID)
        {
            muscleTest.ChannelB = channelViewModel.ToChannel();
        }
        else if (channelViewModel.ID == muscleTest.ChannelC.ID)
        {
            muscleTest.ChannelC = channelViewModel.ToChannel();
        }
        else if (channelViewModel.ID == muscleTest.ChannelD.ID)
        {
            muscleTest.ChannelD = channelViewModel.ToChannel();
        }
    }


    void MuscleTestUnitChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
            foreach (MuscleTestChannelViewModel item in e.NewItems)
                item.PropertyChanged += ChannelChanged;

        if (e.OldItems != null)
            foreach (MuscleTestChannelViewModel item in e.OldItems)
                item.PropertyChanged -= ChannelChanged;
    }

    void ChannelChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnChannelChanged(sender, e);
    }

    private MuscleTestChannelViewModel ToChannelViewModel(Muscle muscle)
    {
        return new MuscleTestChannelViewModel(_deviceStore, muscle);
    }


}
