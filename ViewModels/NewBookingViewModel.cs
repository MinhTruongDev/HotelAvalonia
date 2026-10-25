using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelAvalonia.Models;
using HotelAvalonia.Services;
using System;
using System.Threading.Tasks;

namespace HotelAvalonia.ViewModels;

public partial class NewBookingViewModel : ObservableObject
{
    private readonly IBookingDialogService _dialogService;

    public Action<BookingDraft>? BookingConfirmed { get; set; }

    [ObservableProperty]
    private string? _selectedGuest;

    [ObservableProperty]
    private string _selectedRoomType = "Standard";

    [ObservableProperty]
    private string? _selectedRoomNumber;

    [ObservableProperty]
    private DateTimeOffset? _checkInDate =
        DateTimeOffset.Now.Date.AddDays(1);

    [ObservableProperty]
    private DateTimeOffset? _checkOutDate =
        DateTimeOffset.Now.Date.AddDays(3);

    [ObservableProperty]
    private int _guestCount = 2;

    [ObservableProperty]
    private string _validationMessage = string.Empty;

    public string[] Guests { get; } =
    [
        "Nguyen Minh Anh",
        "Tran Quoc Bao",
        "Le Thu Ha",
        "Pham Duc Long",
        "Vo Ngoc Linh",
        "New guest (demo)"
    ];

    public string[] RoomTypes { get; } =
    [
        "Standard",
        "Deluxe",
        "Suite"
    ];

    public string[] RoomNumbers { get; } =
    [
        "105", "108", "302", "305", "501", "502"
    ];

    public bool HasValidationMessage =>
        !string.IsNullOrWhiteSpace(ValidationMessage);

    public decimal NightlyRate => SelectedRoomType switch
    {
        "Deluxe" => 1_500_000m,
        "Suite" => 2_200_000m,
        _ => 800_000m
    };

    public int Nights
    {
        get
        {
            if (CheckInDate is null || CheckOutDate is null)
                return 0;

            return Math.Max(
                0,
                (CheckOutDate.Value.Date - CheckInDate.Value.Date).Days);
        }
    }

    public string NightsText => $"{Nights} night(s)";

    public decimal TotalPrice => Nights * NightlyRate;

    public string NightlyRateText => $"{NightlyRate:N0} ₫ / night";

    public string TotalPriceText => $"{TotalPrice:N0} ₫";

    // The subtotal currently has no additional fees or discounts.
    public string SubtotalText => TotalPriceText;

    public NewBookingViewModel(IBookingDialogService dialogService)
    {
        _dialogService = dialogService;

        SelectedGuest = Guests[0];
        SelectedRoomNumber = RoomNumbers[0];
    }

    partial void OnSelectedRoomTypeChanged(string value)
    {
        NotifyPricePropertiesChanged();
    }

    partial void OnCheckInDateChanged(DateTimeOffset? value)
    {
        NotifyDatePropertiesChanged();
    }

    partial void OnCheckOutDateChanged(DateTimeOffset? value)
    {
        NotifyDatePropertiesChanged();
    }

    partial void OnValidationMessageChanged(string value)
    {
        OnPropertyChanged(nameof(HasValidationMessage));
    }

    private void NotifyDatePropertiesChanged()
    {
        OnPropertyChanged(nameof(Nights));
        OnPropertyChanged(nameof(NightsText));
        NotifyPricePropertiesChanged();
    }

    private void NotifyPricePropertiesChanged()
    {
        OnPropertyChanged(nameof(NightlyRate));
        OnPropertyChanged(nameof(NightlyRateText));
        OnPropertyChanged(nameof(TotalPrice));
        OnPropertyChanged(nameof(TotalPriceText));
        OnPropertyChanged(nameof(SubtotalText));
    }

    [RelayCommand]
    private async Task ConfirmBookingAsync()
    {
        ValidationMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(SelectedGuest))
        {
            ValidationMessage = "Select a guest to continue.";
            return;
        }

        if (string.IsNullOrWhiteSpace(SelectedRoomNumber))
        {
            ValidationMessage = "Select a room to continue.";
            return;
        }

        if (CheckInDate is null || CheckOutDate is null ||
            CheckOutDate.Value.Date <= CheckInDate.Value.Date)
        {
            ValidationMessage = "Check-out must be later than check-in.";
            return;
        }

        if (GuestCount < 1)
        {
            ValidationMessage = "At least one guest is required.";
            return;
        }

        var draft = new BookingDraft(
            SelectedGuest,
            SelectedRoomType,
            SelectedRoomNumber,
            CheckInDate.Value.Date,
            CheckOutDate.Value.Date,
            GuestCount,
            TotalPrice);

        var confirmed = await _dialogService.ConfirmBookingAsync(draft);

        if (confirmed)
        {
            BookingConfirmed?.Invoke(draft);
        }
    }
}