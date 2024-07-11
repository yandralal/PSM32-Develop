using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Psm32.DB;
using Psm32.Models;
using Psm32.Services;
using Psm32Tests.Mocks;
using Serilog;

namespace Psm32Tests.Services;

public class AppConfigDBServiceTest
{
    private readonly Psm32DbContextFactory _dbContextFactory;

    public AppConfigDBServiceTest()
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
    public async void Load_AllDefault_Success()
    {
        var appConfigDBService = new AppConfigDBService(_dbContextFactory, new LoggerMock().MockObject.Object);
        var expected = AppConfiguration.CreateDefaultConfiguration();

        var config = await appConfigDBService.Load();

        Assert.True(_dbContextFactory.CreateDbContext().AppConfig.Any());
        Assert.True(expected.Equals(config));
    }


    [Fact]
    public async void SaveGetAppConfig_Success()
    {
        var appConfigDBService = new AppConfigDBService(_dbContextFactory, new LoggerMock().MockObject.Object);
        var config = AppConfiguration.CreateDefaultConfiguration();
        
        config.ComPortConfiguration.PortName = "COM222";
        config.ComPortConfiguration.BoudRate = 9999;


        await appConfigDBService.Save(config);

        var savedConfig = await appConfigDBService.GetAppConfiguration();

        Assert.True(_dbContextFactory.CreateDbContext().AppConfig.Any());
        Assert.True(config.Equals(savedConfig));
    }
}
