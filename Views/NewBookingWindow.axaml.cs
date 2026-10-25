using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace HotelAvalonia;

public partial class NewBookingWindow : Window
{
    private decimal _nightlyRate = 800_000m;

    public NewBookingWindow()
    {
        InitializeComponent();

        var today = DateTime.Today;
        CheckInPicker.SelectedDate = new DateTimeOffset(today.AddDays(1));
        CheckOutPicker.SelectedDate = new DateTimeOffset(today.AddDays(3));

        GuestCountInput.Value = 2;
        UpdatePriceSummary();
    }

    private void RoomSelection_Changed(object? sender, SelectionChangedEventArgs e)
    {
        if (RoomTypeCombo is null ||
        NightlyRateText is null)
        {
            return;
        }

        var roomType = (RoomTypeCombo.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Standard";
        _nightlyRate = roomType switch
        {
            "Deluxe" => 1_500_000m,
            "Suite" => 2_200_000m,
            _ => 800_000m
        };

        NightlyRateText.Text = $"{_nightlyRate:N0} ₫ / night";
        UpdatePriceSummary();
    }

    private void UpdatePriceSummary()
    {
        var checkIn = CheckInPicker?.SelectedDate?.DateTime.Date ?? DateTime.Today.AddDays(1);
        var checkOut = CheckOutPicker?.SelectedDate?.DateTime.Date ?? checkIn.AddDays(2);
        var nights = Math.Max(0, (checkOut - checkIn).Days);
        var total = nights * _nightlyRate;

        if (NightsText is not null)
            NightsText.Text = $"{nights} night(s)";
        if (SubtotalText is not null)
            SubtotalText.Text = $"{total:N0} ₫";
        if (TotalText is not null)
            TotalText.Text = $"{total:N0} ₫";
    }

    private void DatePicker_Changed(object? sender, DatePickerSelectedValueChangedEventArgs e) => UpdatePriceSummary();

    private async void ConfirmBooking_Click(object? sender, RoutedEventArgs e)
    {
        ValidationText.IsVisible = false;

        var guestName = (GuestCombo.SelectedItem as ComboBoxItem)?.Content?.ToString();
        var roomType = (RoomTypeCombo.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Standard";
        var roomNumber = (RoomNumberCombo.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "105";
        var checkIn = CheckInPicker.SelectedDate?.DateTime.Date;
        var checkOut = CheckOutPicker.SelectedDate?.DateTime.Date;
        var guestCount = (int)(GuestCountInput.Value ?? 1);

        if (string.IsNullOrWhiteSpace(guestName))
        {
            ShowValidation("Select a guest to continue.");
            return;
        }

        if (checkIn is null || checkOut is null || checkOut.Value.Date <= checkIn.Value.Date)
        {
            ShowValidation("Check-out must be later than check-in.");
            return;
        }

        var nights = (checkOut.Value.Date - checkIn.Value.Date).Days;
        var total = nights * _nightlyRate;
        var draft = new BookingDraft(guestName, roomType, roomNumber, checkIn.Value, checkOut.Value, guestCount, total);

        var confirmation = new BookingConfirmationWindow(draft);
        var confirmed = await confirmation.ShowDialog<bool>(this);

        if (confirmed)
            Close(draft);
    }

    private void ShowValidation(string message)
    {
        ValidationText.Text = message;
        ValidationText.IsVisible = true;
    }

    private void Cancel_Click(object? sender, RoutedEventArgs e) => Close(null);
}
