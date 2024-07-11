using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Psm32.DB;
using Psm32.Models;
using Psm32.Services;

namespace Psm32Tests.Services;

public class MotorTaskDBServiceTest
{
    private readonly Psm32DbContextFactory _dbContextFactory;

    public MotorTaskDBServiceTest()
    {
        var serviceProvider = new ServiceCollection()
               .AddEntityFrameworkInMemoryDatabase()
               .BuildServiceProvider();

        var options = new DbContextOptionsBuilder<Psm32DbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .UseInternalServiceProvider(serviceProvider)
            .Options;

        _dbContextFactory = new Psm32DbContextFactory(options);
    }

    [Fact]
    public async void SaveMotorTaskTest_NewTask_Success()
    {
        var mtDBService = new MotorTaskDBService(_dbContextFactory);

        var motorTask = new MotorTask(
            iD: Guid.NewGuid(),
            name: "Test Name",
            patientId: "Test ID",
            configuration: new MotorTaskConfiguration(),
            created: DateTime.Now,
            updated: DateTime.Now
        );

        await mtDBService.SaveMotorTask(motorTask);

        var motorTasks = await mtDBService.GetAllMotorTasks();
            

        Assert.Single(motorTasks);
        
        var actual = motorTasks.First();
        Assert.Equal(actual.ID, motorTask.ID);
        Assert.Equal(actual.Name, motorTask.Name);
        Assert.Equal(actual.PatientId, motorTask.PatientId);
        Assert.Equal(actual.Configuration, motorTask.Configuration);
        Assert.Equal(actual.Created, motorTask.Created);
        Assert.Equal(actual.Updated, motorTask.Updated);
    }

    [Fact]
    public async void SaveMotorTaskTest_ExistingTask_Success()
    {
        var mtDBService = new MotorTaskDBService(_dbContextFactory);
        var motorTask = new MotorTask(
            iD: Guid.NewGuid(),
            name: "Test Name",
            patientId: "Test ID",
            configuration: new MotorTaskConfiguration(),
            created: DateTime.Now,
            updated: DateTime.Now
        );

        await mtDBService.SaveMotorTask(motorTask);

        var updatedMotorTask = new MotorTask(
            iD: motorTask.ID,
            name: "New Test Name",
            patientId: motorTask.PatientId,
            configuration: new MotorTaskConfiguration(),
            created: motorTask.Created,
            updated: DateTime.Now
            );

        await mtDBService.SaveMotorTask(updatedMotorTask);

        var motorTasks = await mtDBService.GetAllMotorTasks();

        Assert.Single(motorTasks);

        var actual = motorTasks.First();
        Assert.Equal(actual.ID, updatedMotorTask.ID);
        Assert.Equal(actual.Name, updatedMotorTask.Name);
        Assert.Equal(actual.PatientId, updatedMotorTask.PatientId);
        Assert.Equal(actual.Configuration, updatedMotorTask.Configuration);
        Assert.Equal(actual.Created, updatedMotorTask.Created);
        Assert.Equal(actual.Updated, updatedMotorTask.Updated);
    }


    [Fact]
    public async void GetAllMotorTasksTest_Success()
    {
        var mtDBService = new MotorTaskDBService(_dbContextFactory);
        var motorTask1 = new MotorTask(
            iD: Guid.NewGuid(),
            name: "Test Name 1",
            patientId: "Test ID 1",
            configuration: new MotorTaskConfiguration(),
            created: DateTime.Now,
            updated: DateTime.Now
        );
        var motorTask2 = new MotorTask(
           iD: Guid.NewGuid(),
           name: "Test Name 2",
           patientId: "Test ID 2",
           configuration: new MotorTaskConfiguration(),
           created: DateTime.Now,
           updated: DateTime.Now
       );

        await mtDBService.SaveMotorTask(motorTask1);
        await mtDBService.SaveMotorTask(motorTask2);

        var result = await mtDBService.GetAllMotorTasks();

        Assert.IsAssignableFrom<IEnumerable<MotorTask>>(result);

        var motorTasks = result.ToList();

        Assert.Equal(2, motorTasks.Count);

        Assert.Equal(result.ElementAt(0).ID, motorTask1.ID);
        Assert.Equal(result.ElementAt(0).Name, motorTask1.Name);
        Assert.Equal(result.ElementAt(0).PatientId, motorTask1.PatientId);
        Assert.True(result.ElementAt(0).Configuration.Equals(motorTask1.Configuration));
        Assert.Equal(result.ElementAt(0).Created, motorTask1.Created);
        Assert.Equal(result.ElementAt(0).Updated, motorTask1.Updated);

        Assert.Equal(result.ElementAt(1).ID, motorTask2.ID);
        Assert.Equal(result.ElementAt(1).Name, motorTask2.Name);
        Assert.Equal(result.ElementAt(1).PatientId, motorTask2.PatientId);
        Assert.True(result.ElementAt(1).Configuration.Equals(motorTask2.Configuration));
        Assert.Equal(result.ElementAt(1).Created, motorTask2.Created);
        Assert.Equal(result.ElementAt(1).Updated, motorTask2.Updated);
    }

    [Fact]
    public async void GetAllMotorTasksTest_EmptyDB_Success()
    {
        var mtDBService = new MotorTaskDBService(_dbContextFactory);
        
        var result = await mtDBService.GetAllMotorTasks();

        Assert.IsAssignableFrom<IEnumerable<MotorTask>>(result);

        Assert.False(result.Any());
    }

    [Fact]
    public async void GetMotorTaskByNameTest_Success()
    {
        var mtDBService = new MotorTaskDBService(_dbContextFactory);
        var motorTask = new MotorTask(
            iD: Guid.NewGuid(),
            name: "Test Name",
            patientId: "Test ID",
            configuration: new MotorTaskConfiguration(),
            created: DateTime.Now,
            updated: DateTime.Now
        );

        await mtDBService.SaveMotorTask(motorTask);

        var actual = await mtDBService.GetMotorTaskByName(motorTask.Name);

        Assert.NotNull(actual);
        Assert.Equal(actual?.ID, motorTask.ID);
        Assert.Equal(actual?.Name, motorTask.Name);
        Assert.Equal(actual?.PatientId, motorTask.PatientId);
        Assert.True(actual?.Configuration.Equals(motorTask.Configuration));
        Assert.Equal(actual?.Created, motorTask.Created);
        Assert.Equal(actual?.Updated, motorTask.Updated);
    }

    [Fact]
    public async void GetMotorTaskByNameTest_NotFound()
    {
        var mtDBService = new MotorTaskDBService(_dbContextFactory);
        var motorTask = new MotorTask(
            iD: Guid.NewGuid(),
            name: "Test Name",
            patientId: "Test ID",
            configuration: new MotorTaskConfiguration(),
            created: DateTime.Now,
            updated: DateTime.Now
        );

        await mtDBService.SaveMotorTask(motorTask);

        var actual = await mtDBService.GetMotorTaskByName("Some Name");

        Assert.Null(actual);
    }
}
