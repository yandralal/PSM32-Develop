using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Psm32.Services;
using Psm32.Stores;
using Psm32.ViewModels;
using System;

namespace Psm32.HostBuilders
{
    internal static class AddViewModelsHostBuilderExtensions
    {
        public static IHostBuilder AddServices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<IMotorTaskDBService, MotorTaskDBService>();
                services.AddSingleton<IAppConfigDBService, AppConfigDBService>();
                services.AddSingleton<IMotorTaskValidator, MotorTaskValidator>(); //TODO: move into IMotorTaskDBService
                services.AddSingleton<IUserDBService, UserDBService>();
                services.AddSingleton<IQSUScanner, QSUScanner>();
                services.AddSingleton<IComPortWrapper, ComPortWrapper>();
                services.AddSingleton<IComPortCommunicationService, ComPortCommunicationService>();
                services.AddSingleton<IQSUMessageSender, QSUMessageSender>();
                services.AddSingleton<IQSUMessageReceiver, QSUMessageReceiver>();
                services.AddSingleton<IAuthenticationService, AuthenticationService>();
                services.AddSingleton<IMuscleNamesReadService, MuscleNamesReadService>();
            });
            return hostBuilder;
        }

        public static IHostBuilder AddViewModels(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                //services.AddSingleton<NavigationService<QSUnitsViewModel, LoginViewModel>>();
                //services.AddSingleton<NavigationService<SystemSettingsViewModel, LoginViewModel>>();
                //services.AddSingleton<NavigationService<MotorTasksViewModel, LoginViewModel>>();
                //services.AddSingleton<NavigationService<MuscleTestViewModel, LoginViewModel>>();
                //services.AddSingleton<NavigationService<LoginViewModel, LoginViewModel>>();


                services.AddSingleton<LayoutNavigationService<QSUnitsViewModel>>();
                services.AddSingleton<LayoutNavigationService<SystemSettingsViewModel>>();
                services.AddSingleton<LayoutNavigationService<MotorTasksViewModel>>();
                services.AddSingleton<LayoutNavigationService<MuscleTestViewModel>>();
                services.AddSingleton<LayoutNavigationService<LoginViewModel>>();

                services.AddSingleton<Func<QSUnitsViewModel>>((s) => () => s.GetRequiredService<QSUnitsViewModel>());
                services.AddSingleton<Func<SystemSettingsViewModel>>((s) => () => s.GetRequiredService<SystemSettingsViewModel>());
                services.AddSingleton<Func<MotorTasksViewModel>>((s) => () => s.GetRequiredService<MotorTasksViewModel>());
                services.AddSingleton<Func<MuscleTestViewModel>>((s) => () => s.GetRequiredService<MuscleTestViewModel>());
                services.AddSingleton<Func<LoginViewModel>>((s) => () => s.GetRequiredService<LoginViewModel>());
                services.AddSingleton<Func<TopNavigationBarViewModel>>((s) => () => s.GetRequiredService<TopNavigationBarViewModel>());
                services.AddSingleton<Func<SubNavigationBarViewModel>>((s) => () => s.GetRequiredService<SubNavigationBarViewModel>());

                services.AddTransient(s => CreateQSUnitsViewModel(s));
                services.AddTransient(s => CreateMotorTasksViewModel(s));
                services.AddTransient(s => CreateMuscleTestViewModelModel(s));
                services.AddTransient(s => CreateLoginViewModel(s));
                services.AddTransient(s => CreateTopNavigationBarViewModel(s));
                services.AddTransient(s => CreateSubNavigationBarViewModel(s));
            });
            return hostBuilder;
        }

        private static MotorTasksViewModel CreateMotorTasksViewModel(IServiceProvider services)
        {
            return MotorTasksViewModel.LoadViewModel(services.GetRequiredService<DeviceStore>());
        }

        private static LoginViewModel CreateLoginViewModel(IServiceProvider services)
        {
            return LoginViewModel.LoadViewModel(
                services.GetRequiredService<DeviceStore>(),
                services.GetRequiredService<LayoutNavigationService<QSUnitsViewModel>>()
                );
        }

        private static MuscleTestViewModel CreateMuscleTestViewModelModel(IServiceProvider services)
        {
            return MuscleTestViewModel.LoadViewModel(
                services.GetRequiredService<DeviceStore>(), 
                services.GetRequiredService<LayoutNavigationService<QSUnitsViewModel>>(),
                services.GetRequiredService<IQSUMessageSender>());
        }

        private static QSUnitsViewModel CreateQSUnitsViewModel(IServiceProvider services)
        {
            return QSUnitsViewModel.LoadViewModel(
                services.GetRequiredService<DeviceStore>(),
                services.GetRequiredService<LayoutNavigationService<MuscleTestViewModel>>());
        }

        private static TopNavigationBarViewModel CreateTopNavigationBarViewModel(IServiceProvider services)
        {
            return TopNavigationBarViewModel.LoadViewModel(
                services.GetRequiredService<DeviceStore>(),
                services.GetRequiredService<LayoutNavigationService<MuscleTestViewModel>>());
        }

        private static SubNavigationBarViewModel CreateSubNavigationBarViewModel(IServiceProvider services)
        {
            return SubNavigationBarViewModel.LoadViewModel(
                services.GetRequiredService<DeviceStore>());
        }
    }
}
