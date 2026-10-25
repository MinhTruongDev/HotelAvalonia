using Avalonia.Controls;
using Avalonia.Interactivity;

namespace HotelAvalonia;

public partial class BookingConfirmationWindow : Window
{
    public BookingConfirmationWindow(BookingDraft draft)
    {
        InitializeComponent();

        GuestValue.Text = draft.GuestName;
        RoomValue.Text = $"{draft.RoomType} · {draft.RoomNumber}";
        CheckInValue.Text = draft.CheckIn.ToString("dd MMM yyyy");
        CheckOutValue.Text = draft.CheckOut.ToString("dd MMM yyyy");
        var nights = (draft.CheckOut.Date - draft.CheckIn.Date).Days;
        GuestsNightsValue.Text = $"{draft.GuestCount} guest(s) · {nights} night(s)";
        TotalValue.Text = $"{draft.Total:N0} ₫";
    }

    private void GoBack_Click(object? sender, RoutedEventArgs e) => Close(false);

    private void Confirm_Click(object? sender, RoutedEventArgs e) => Close(true);
}
