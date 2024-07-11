using Psm32.Exceptions;
using Psm32.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Psm32.Stores;

public class DeviceStore
{
    private readonly Device _device;    
    private readonly List<MotorTask> _motorTasks;
    private readonly List<QSUnit> _qSUnits;
    private readonly Lazy<Task> _initializeMotorTasksLazy;
    private readonly Lazy<Task> _scanQSUnitsLazy;
    private readonly Lazy<Task> _loadAppConfigLazy;

    private MotorTask? _currentMotorTask;
    private List<QSUnit>? _currentMuscleTest;
    private AppConfiguration _appConfiguration;
    private Session _currentSession;
    private List<MuscleName> _muscleNames;
    private DateTime _lastScannedTime;

    public event Action<MotorTask>? MotorTaskAdded;
    public event Action<IEnumerable<QSUnit>>? QSUnitsScanned;
    public Session CurrentSession => _currentSession;

    public DeviceStore(Device device)
    {
        _device = device;
        _motorTasks = [];
        _qSUnits = [];
        _currentMotorTask = null;
        _currentMuscleTest = null; 
        _currentSession = new();
        _appConfiguration = new AppConfiguration();
        _initializeMotorTasksLazy = new Lazy<Task>(InitializeMotorTasks);
        _scanQSUnitsLazy = new Lazy<Task>(ScanQSUs);
        _loadAppConfigLazy = new Lazy<Task>(LoadAppConfigurationFromDb);
        _muscleNames = ReadMuscleNames().ToList();
    }

    public async Task StartUp()
    {
        await LoadAppConfiguration();
        await ScanQSUs();

        _device.Startup();
    }

    public IEnumerable<MotorTask> MotorTasks => _motorTasks;
    public IEnumerable<QSUnit> QSUnits => _qSUnits;
    public AppConfiguration AppConfig => _appConfiguration;

    public MotorTask? CurrentMotorTask => _currentMotorTask;

    public List<QSUnit>? CurrentMuscleTest => _currentMuscleTest;

    public DateTime LastScannedTime
    {
        get
        {
            return _lastScannedTime;
        }
        set
        {
            _lastScannedTime = value;
        }
    }

    public List<MuscleName> MuscleNames => _muscleNames;

    public async Task LoadMotorTasks()
    {
        await _initializeMotorTasksLazy.Value;
    }

    public async Task LoadQSUs()
    {
        await _scanQSUnitsLazy.Value;
    }

    public async Task LoadAppConfiguration()
    {
        await _loadAppConfigLazy.Value;
    }

    public async Task SaveMotorTask()
    {
        if (_currentMotorTask is null)
        {
            throw new MotorTaskException("No Motor Task to save");
        }

        await _device.SaveMotorTask(_currentMotorTask);
        if (_motorTasks.Find(mt => mt.ID == _currentMotorTask.ID) is null)
        {
            _motorTasks.Add(_currentMotorTask);// TODO: load motor tasks on startup?s

            OnMotorTaskAdded(_currentMotorTask);
        }
    }


    private void OnMotorTaskAdded(MotorTask motorTask)
    {
        MotorTaskAdded?.Invoke(motorTask);
    }
    
    private void OnQSUnitsScanned(IEnumerable<QSUnit> qsUnits)
    {
        _lastScannedTime = DateTime.Now;
        QSUnitsScanned?.Invoke(qsUnits);
    }
    private async Task InitializeMotorTasks()
    {
        IEnumerable<MotorTask> motorTasks = await _device.GetAllMotorTasks();

        _motorTasks.Clear();
        _motorTasks.AddRange(motorTasks);
    }

    public async Task ScanQSUs()
    {
        var qsUnits = await _device.ScanQSUs();

        _qSUnits.Clear();
        _qSUnits.AddRange(qsUnits);

        OnQSUnitsScanned(qsUnits);
    }

    public IEnumerable<MuscleName> ReadMuscleNames()
    {
        return _device.ReadMuscleNames();
    }

    public async Task LoadAppConfigurationFromDb()
    {
        _appConfiguration = await _device.LoadAppConfiguration();
    }

    public void SetCurrentMotorTask(MotorTask  motorTask)
    {
        _currentMotorTask = motorTask;
    }

    public void ClearCurrentMotorTask()
    {
        _currentMotorTask = null;
    }

    public bool MotorTaskOpen()
    {
        return _currentMotorTask is not null;
    }

    public async Task<User> UserLogin(string username, string password)
    {
        return await _device.UserLogin(username, password);
    }

    public async Task UserRegister(string username, string password, UserRole userRole)
    {
        await _device.UserRegister(username, password, userRole);
    }


    public void FromMotorTaskToMuscleTest()
    {
        if (_currentMotorTask == null)
        {
            throw new InvalidOperationException("Can not perfom muscle testing when CurrentMotorTask is null");
        }
        _currentMuscleTest = [];

        foreach (var unit in _qSUnits.ToList())
        {
            _currentMuscleTest.Add(new QSUnit(unit));
        }

        var motorTaskConfiguration = _currentMotorTask.Configuration;
        foreach (var muscleGroup in motorTaskConfiguration.MuscleGroups)
        {
            foreach (var muscle in muscleGroup.Muscles)
            {
                FromMuscleToQSU(muscle);

            }

            foreach (var muscle in motorTaskConfiguration.UngrouppedMuscles)
            {
                FromMuscleToQSU(muscle.Muscle);
            }
        }
    }

    private void FromMuscleToQSU(Muscle muscle)
    {

        var foundUnit = _currentMuscleTest?.Find(unit => unit.ID == muscle.UnitId);
        if (foundUnit == null)
        {
            throw new InvalidOperationException("Unit not found whole converting Motor Task to Musle Test");
        }

        switch (muscle.Letter)
        {
            case 'A':
                foundUnit.ChannelA = muscle;
                break;
            case 'B':
                foundUnit.ChannelB = muscle;
                break;
            case 'C':
                foundUnit.ChannelC = muscle;
                break;
            case 'D':
                foundUnit.ChannelD = muscle;
                break;
            case 'G':
                foundUnit.ChannelG = muscle;
                break;
            default: throw new InvalidOperationException("Ivalid Channel Letter");

        }
    }
}
