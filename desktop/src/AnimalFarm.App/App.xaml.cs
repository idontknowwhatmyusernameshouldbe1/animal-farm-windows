using System.Windows;
using AnimalFarm.App.Services;
using AnimalFarm.App.ViewModels;
using AnimalFarm.App.Views;
using AnimalFarm.Core.I18n;
using AnimalFarm.Core.Services;
using AnimalFarm.Core.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace AnimalFarm.App;

public partial class App : Application
{
    private ServiceProvider? _services;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();
        ConfigureServices(services);
        _services = services.BuildServiceProvider();

        var paths = _services.GetRequiredService<AppPaths>();
        paths.EnsureCreated();
        _services.GetRequiredService<PetRepository>().Initialize();

        var window = _services.GetRequiredService<MainWindow>();
        MainWindow = window;
        window.Show();
        _services.GetRequiredService<MainViewModel>().ShowHome();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<AppPaths>();
        services.AddSingleton<PetRepository>();
        services.AddSingleton<ImageService>();
        services.AddSingleton<PetService>();
        services.AddSingleton<SettingsService>();
        services.AddSingleton<ThemeService>();
        services.AddSingleton<LocalizationService>();
        services.AddSingleton<NavigationService>();
        services.AddSingleton<PageFactory>();

        services.AddSingleton<MainViewModel>();
        services.AddTransient<HomeViewModel>();
        services.AddTransient<PetDetailViewModel>();
        services.AddTransient<PetFormViewModel>();

        services.AddTransient<HomePage>();
        services.AddTransient<PetDetailPage>();
        services.AddTransient<PetFormPage>();
        services.AddSingleton<MainWindow>();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _services?.Dispose();
        base.OnExit(e);
    }
}
