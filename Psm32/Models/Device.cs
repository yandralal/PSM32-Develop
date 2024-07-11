using Psm32.Exceptions;
using Psm32.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psm32.Models;

public class Device
{
    private const int MAX_QSUS_ALLOWED = 8;

    public Device(
        IMotorTaskDBService motorTaskDbService,
        IAppConfigDBService appConfigDbService,
        IMotorTaskValidator motorTaskValidator,
        IQSUMessageReceiver qsuMessageReceiver,
        IQSUScanner qsuSanner, 
        IAuthenticationService authenticationService,
        IMuscleNamesReadService muscleNamesReadService
        )
    {

        _motorTaskDbService = motorTaskDbService;
        _appConfigDbService = appConfigDbService;
        _motorTaskValidator = motorTaskValidator;
        _qsuMessageReceiver = qsuMessageReceiver;
        _qsuSanner = qsuSanner;
        _authenticationService = authenticationService;
        _muscleNamesReadService = muscleNamesReadService;
    }

    public void Startup()
    {
        _qsuMessageReceiver.Start();
    }

    public async Task<AppConfiguration> LoadAppConfiguration()
    {
        return await _appConfigDbService.Load();
    }

    public async Task<List<QSUnit>> ScanQSUs()
    {
        var units = await _qsuSanner.Scan();

        if (units == null)
        {
            units = new List<QSUnit>();
        }

        if (units.Count() > MAX_QSUS_ALLOWED)
        {
            throw new QSUException($"Detected {units.Count()} Quad Stim units, only {MAX_QSUS_ALLOWED} Quad Stim units is allowed");
        }

        units = AddRemainingNAUnits(units);

        return units;
    }


    private static List<QSUnit> AddRemainingNAUnits(List<QSUnit> units)
    {
        var newList = new List<QSUnit>(units);

        for (int i = units.Count() + 1; i <= MAX_QSUS_ALLOWED; i++)
        {
            var naUnit = new QSUnit(i);
            naUnit.SetUnitAsNA();
            newList.Add(naUnit);
        }

        return newList;
    }

    public async Task<IEnumerable<MotorTask>> GetAllMotorTasks()
    {
        return await _motorTaskDbService.GetAllMotorTasks();
    }

    public async Task SaveMotorTask(MotorTask motorTask)
    {
        await _motorTaskDbService.SaveMotorTask(motorTask);
    }

    public async Task<User> UserLogin(string username, string password)
    {
        return await _authenticationService.Login(username, password);
    }

    public IEnumerable<MuscleName> ReadMuscleNames()
    {
        return _muscleNamesReadService.ReadMuscleNames();
    }


    internal async Task UserRegister(string username, string password, UserRole userRole)
    {
        await _authenticationService.Register(username, password, userRole);
    }

    /*public async Task AddMotorTask(MotorTask motorTask)
    {
        if (await _motorTaskValidator.DoesNameExist(motorTask))
        {
            throw new MotorNameConflictTaskException($"Motor Task with name {motorTask.Name} already exists. Use different name");
        }
        

        await _motorTaskCreator.AddMotorTask(motorTask);
    }*/

    private readonly IQSUScanner _qsuSanner;
    private readonly IAuthenticationService _authenticationService;
    private readonly IMuscleNamesReadService _muscleNamesReadService;
    private readonly IMotorTaskDBService _motorTaskDbService;
    private readonly IAppConfigDBService _appConfigDbService;
    private readonly IMotorTaskValidator _motorTaskValidator;
    private readonly IQSUMessageReceiver _qsuMessageReceiver;

    //private AppConfiguration _appConfig;
}
