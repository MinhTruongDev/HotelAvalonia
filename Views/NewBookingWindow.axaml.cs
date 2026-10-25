using Avalonia.Controls;
using Avalonia.Interactivity;
using HotelAvalonia.Models;
using HotelAvalonia.Services;
using HotelAvalonia.ViewModels;

namespace HotelAvalonia;

public partial class NewBookingWindow : Window
{
    public NewBookingWindow(
        NewBookingViewModel viewModel,
        IBookingDialogService dialogService)
    {
        InitializeComponent();

        dialogService.SetOwner(this);

        viewModel.BookingConfirmed += draft => Close(draft);

        DataContext = viewModel;
    }

    private void Cancel_Click(object? sender, RoutedEventArgs e)
    {
        Close((BookingDraft?)null);
    }
}