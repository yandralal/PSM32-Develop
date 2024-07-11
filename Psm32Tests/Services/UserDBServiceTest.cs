using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Psm32.DB;
using Psm32.Exceptions;
using Psm32.Models;
using Psm32.Services;
using Psm32Tests.Mocks;

namespace Psm32Tests.Services;

public class UserDBServiceTest
{
    private readonly Psm32DbContextFactory _dbContextFactory;

    public UserDBServiceTest()
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
    public async void AddUser_Success()
    {
        var userDBService = new UserDBService(
            _dbContextFactory, 
            new LoggerMock().MockObject.Object);

        var user = new User("test", "test", "salt", UserRole.Tech);

        await userDBService.AddUser(user);

        Assert.True(_dbContextFactory.CreateDbContext().Users.Any());
    }

    [Fact]
    public async void AddUser_UserExists_Throws()
    {
        var userDBService = new UserDBService(
            _dbContextFactory,
            new LoggerMock().MockObject.Object);

        var user = new User("test", "test", "salt", UserRole.Tech);

        await userDBService.AddUser(user);

        var exception = await Assert.ThrowsAsync<UserDBServiceException>(
            () => userDBService.AddUser(user));

        Assert.NotNull(exception);
        Assert.Equal("User already exists", exception.Message);
    }

    [Fact]
    public async void GetUser_Success()
    {
        var userDBService = new UserDBService(
            _dbContextFactory,
            new LoggerMock().MockObject.Object);

        await userDBService.AddUser(new User("test", "test", "salt", UserRole.Tech));

        var user = await userDBService.GetUser("test");

        Assert.NotNull(user);
        Assert.Equal("test", user?.Username);
    }

    [Fact]
    public async void GetUser_NotFound()
    {
        var userDBService = new UserDBService(
            _dbContextFactory,
            new LoggerMock().MockObject.Object);

        var user = await userDBService.GetUser("test");

        Assert.Null(user);
    }
}
