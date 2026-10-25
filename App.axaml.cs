
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using HotelAvalonia.Services;
using HotelAvalonia.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HotelAvalonia;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();

        // ViewModels
        services.AddTransient<MainViewModel>();
        services.AddTransient<NewBookingViewModel>();

        // Windows
        services.AddTransient<MainWindow>();
        services.AddTransient<NewBookingWindow>();

        // Dialog services
        services.AddSingleton<IBookingDialogService, BookingDialogService>();
        services.AddSingleton<INewBookingWindowService, NewBookingWindowService>();

        Services = services.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = Services.GetRequiredService<MainWindow>();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
