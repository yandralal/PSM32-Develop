using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Psm32.DB;
using Psm32.HostBuilders;
using Psm32.Models;
using Psm32.Services;
using Psm32.Stores;
using Psm32.ViewModels;
using Serilog;
using System;
using System.Windows;

namespace Psm32
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .UseSerilog((host, LoggerConfiguration) =>
                {
                    LoggerConfiguration.WriteTo.File("log.txt", rollingInterval: RollingInterval.Day) //TODO: figure out the right path
                    .MinimumLevel.Information();
                })
                .AddServices()
                .AddViewModels()
                .ConfigureServices((hostContext, services) =>
                 {
                     var dbName = hostContext.Configuration.GetConnectionString("DbName") ?? "powerstim32.db";
                     services.AddSingleton(new Psm32DbContextFactory(new DbContextOptionsBuilder().UseSqlite(BuildConnectionString(dbName)).Options));

                     services.AddDbContext<Psm32DbContext>(options => options.UseSqlite(BuildConnectionString(dbName)));

                     services.AddSingleton<Device>();
                     services.AddSingleton<ComPortConfiguration>();
                     services.AddSingleton<AppConfiguration>();

                     services.AddSingleton<NavigationStore>();
                     //services.AddSingleton<ModalNavigationStore>();
                     services.AddSingleton<DeviceStore>();

                     services.AddSingleton<MainViewModel>(s => CreateMainViewModel(s));
                     services.AddSingleton(s => new MainWindow()
                     {
                         DataContext = s.GetRequiredService<MainViewModel>()
                     });

                 }).Build();
        }

        private static MainViewModel CreateMainViewModel(IServiceProvider services)
        {
            return MainViewModel.LoadViewModel(
                services.GetRequiredService<DeviceStore>(),
                services.GetRequiredService<NavigationStore>(),
                //services.GetRequiredService<ModalNavigationStore>(),
                services.GetRequiredService<LayoutNavigationService<LoginViewModel>>()
                );
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            _host.Start();
           
            var deviceStore = _host.Services.GetRequiredService<DeviceStore>();

            var psm32DbContextFactory = _host.Services.GetRequiredService<Psm32DbContextFactory>();
            using (var dbContext = psm32DbContextFactory.CreateDbContext())
            {
                dbContext.Database.Migrate();
            }


#if DEBUG
            // Seed admin user
            // TODO:for dev purposes onoly, remove this
            var authenticationService = _host.Services.GetRequiredService<IAuthenticationService>();
            try
            {
                await authenticationService.Register("admin", "admin", UserRole.Admin);
            } catch (Exception)
            {
                // if user already exists, ignore
            }

            try
            {
                await deviceStore.StartUp();
            } catch (Exception)
            {
                //TODO: Navigate to error page
            }
#endif


            var navigationService = _host.Services.GetRequiredService<LayoutNavigationService<LoginViewModel>>();
            navigationService.Navigate();

            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host.Dispose();


            base.OnExit(e);
        }
         
        private static string BuildConnectionString(string dbName)
        {
            var folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "psm32");
            var fullDbName = System.IO.Path.Combine(folder, dbName);

            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);

            return $"Data Source={fullDbName}";
        }
    }
}
