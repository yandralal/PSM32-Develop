using Psm32.Models;
using Psm32.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Data;

namespace Psm32.ViewModels;

public class MuscleTestChannelViewModel : ViewModelBase
{
    public string ID => _iD;
    public string Name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    public decimal AmpPos
    {
        get
        {
            return _ampPos;
        }
        set
        {
            _ampPos = value;
            OnPropertyChanged(nameof(AmpPos));
        }
    }

    public decimal AmpNeg
    {
        get
        {
            return _ampNeg;
        }
        set
        { 
            _ampNeg = value;
            OnPropertyChanged(nameof(AmpNeg));
        }
    }

    public decimal PwPos
    {
        get
        {
            return _pwPos;
        }
        set
        {
            _pwPos = value;
            OnPropertyChanged(nameof(PwPos));
        }
    }

    public decimal PwNeg
    {
        get
        {
            return _pwNeg;
        }
        set
        {
            _pwNeg = value;
            OnPropertyChanged(nameof(PwNeg));
        }
    }

    public int Freq
    {
        get
        {
            return _freq;
        }
        set
        {
            _freq = value;
            OnPropertyChanged(nameof(Freq));
        }
    }

    public string Polarity
    {
        get
        {
            return _polarity;
        }
        set
        {
              _polarity = value;
            OnPropertyChanged(nameof(Polarity));
        }
    }

    public string Side
    {
        get
        {
            return _side;
        }
        set
        {
            _side = value;
            OnPropertyChanged(nameof(Side));
        }
    }

    public bool Enabled
    {
        get
        {
            return _enabled;
        }
        set
        { 
            _enabled = value;
            OnPropertyChanged(nameof(Enabled));
        }
    }


    //TODO: temp -> get this list from outside
    private readonly CollectionView _muscleListEntries;
    private readonly DeviceStore _deviceStore;

    public CollectionView MusclesListEntries
    {
        get { return _muscleListEntries; }
    }

    private string _iD;
    private string _name;
    private decimal _ampPos;
    private decimal _ampNeg;
    private decimal _pwPos;
    private decimal _pwNeg;
    private int _freq;
    private string _polarity;
    private string _side;
    private bool _enabled;
    private string _status;

    public MuscleTestChannelViewModel(DeviceStore deviceStore, Muscle muscle)
    {
        _muscleListEntries = new CollectionView(deviceStore.MuscleNames.Select(mn => mn.ShortName).ToList());
        _iD = muscle.ID;
        _name = muscle.MuscleConfig.Name.ShortName;
        _ampPos = muscle.MuscleConfig.AmpPos;
        _ampNeg = muscle.MuscleConfig.AmpNeg;
        _pwPos = muscle.MuscleConfig.PwPos;
        _pwNeg = muscle.MuscleConfig.PwNeg;
        _freq = muscle.MuscleConfig.Freq;
        _polarity = muscle.MuscleConfig.Polarity.ToString();
        _side = muscle.MuscleConfig.Side.ToString();
        _enabled = muscle.MuscleConfig.Enabled;
        _status = muscle.Status.ToString();
        _deviceStore = deviceStore;
    }

    public Muscle ToChannel()
    {
        return new Muscle(_iD, _name, _ampPos, _ampNeg, _pwPos, _pwNeg, _freq, _polarity, _side, _status, _enabled);      
    }
	}
