using Avalonia.Controls;
using HotelAvalonia.Services;
using HotelAvalonia.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace HotelAvalonia;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var bookingService =
            App.Services.GetRequiredService<INewBookingWindowService>();

        bookingService.SetOwner(this);

        DataContext =
            App.Services.GetRequiredService<MainViewModel>();
    }
}