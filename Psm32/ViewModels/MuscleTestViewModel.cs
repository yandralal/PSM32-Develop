using Psm32.Commands;
using Psm32.Models;
using Psm32.Services;
using Psm32.Stores;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Psm32.ViewModels;

public class MuscleTestViewModel : ViewModelBase
{
    private const string INITIAL_TIME = "00:00.00";
    public IEnumerable<MuscleTestUnitViewModel> MuscleTestUnits => _muscleTestUnits;

    private readonly DeviceStore _deviceStore;
    private readonly IQSUMessageSender _messageSender;
    private readonly ObservableCollection<MuscleTestUnitViewModel> _muscleTestUnits;

    private bool _rampUpEnabled;
    private TimeSpan _rampUpDuration;
    private string _currentTestTime;

    private DispatcherTimer _testTimer;
    private Stopwatch _stopWatch;
    private bool _isTestRunning;

    public ICommand SaveMotorTaskCommand { get; }
    public ICommand CancelMotorTaskCommand { get; }
    public ICommand StartTestCommand { get; }
    public ICommand StopTestCommand { get; }
    public ICommand PauseTestCommand { get; }

    public string CurrentTestTime
    {
        get
        {
            return _currentTestTime;
        }
        private set
        {
            _currentTestTime = value;
            OnPropertyChanged(nameof(CurrentTestTime));
        }
    }


    public bool RampUpEnabled
    {
        get
        {
            return _rampUpEnabled;
        }
        set
        {
            _rampUpEnabled = value;
            OnPropertyChanged(nameof(RampUpEnabled));
        }
    }

    public TimeSpan RampUpDuration
    {
        get
        {
            return _rampUpDuration;
        }
        set
        {
            _rampUpDuration = value;
            OnPropertyChanged(nameof(RampUpDuration));
        }
    }

    public bool IsTestRunning
    {
        get
        {
            return _isTestRunning;
        }
        set
        {
            _isTestRunning = value;
            OnPropertyChanged(nameof(IsTestRunning));
        }
    }

    public MuscleTestViewModel(
        DeviceStore deviceStore, 
        INavigationService qSUnitsNavigationService, 
        IQSUMessageSender messageSender)
    {
        _deviceStore = deviceStore;
        _messageSender = messageSender;
        _muscleTestUnits = new ObservableCollection<MuscleTestUnitViewModel>();
        SaveMotorTaskCommand = new SaveMotorTaskCommand(_deviceStore);
        CancelMotorTaskCommand = new CancelMotorTaskCommand(deviceStore, qSUnitsNavigationService);
        StartTestCommand = new StartTestCommand(this, deviceStore, _messageSender);
        StopTestCommand = new StopTestCommand(this, deviceStore, _messageSender);
        PauseTestCommand = new PauseTestCommand(this,deviceStore, _messageSender);

        _rampUpEnabled = false;
        _rampUpDuration = TimeSpan.FromSeconds(0);
        _currentTestTime = INITIAL_TIME;

        _testTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(100)
        };

        _testTimer.Tick += new EventHandler(OnTestTimerTick);

        _stopWatch = new Stopwatch();
        _isTestRunning = false;
    }

    public static MuscleTestViewModel LoadViewModel(
        DeviceStore deviceStore, 
        INavigationService qSUnitsNavigationService,
        IQSUMessageSender messageSender)
    {
        MuscleTestViewModel muscleTestViewModel = new(deviceStore, qSUnitsNavigationService, messageSender);
        muscleTestViewModel.UpdateMuscleTestUnits(deviceStore);

        return muscleTestViewModel;
    }

    public void UpdateMuscleTestUnits(DeviceStore deviceStore)
    {
        _muscleTestUnits.Clear();
        deviceStore.FromMotorTaskToMuscleTest();
        if (deviceStore.CurrentMuscleTest == null)
        {
            throw new InvalidOperationException("Muscle Test structure is null");
        }
        foreach (var muscleTest in deviceStore.CurrentMuscleTest)
        {
            _muscleTestUnits.Add(ToMuscleTestUnitViewModel(deviceStore, muscleTest));
        }
    }

    

    private static MuscleTestUnitViewModel ToMuscleTestUnitViewModel(DeviceStore deviceStore, QSUnit muscleTest)
    {
        return new MuscleTestUnitViewModel(deviceStore, muscleTest);
    }

    public void StartTest()
    {
        IsTestRunning = true;
        _testTimer.Start();
        _stopWatch.Start();
    }

    public void StopTest()
    {
        IsTestRunning = false;
        _stopWatch.Stop();
        _testTimer.Stop();
        _stopWatch.Reset();
        CurrentTestTime = INITIAL_TIME;
    }

    public void PauseTest()
    {
        IsTestRunning = false;
        _stopWatch.Stop();
        _testTimer.Stop();
    }

    void OnTestTimerTick(object? sender, EventArgs e)
    {
        if (_stopWatch.IsRunning)
        {
            TimeSpan ts = _stopWatch.Elapsed;
            CurrentTestTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
        }
    }

}
