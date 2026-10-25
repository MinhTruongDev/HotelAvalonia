using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace HotelAvalonia;

public partial class MainWindow : Window
{
    private readonly ObservableCollection<BookingDisplayRow> _allBookings = new();

    public MainWindow()
    {
        InitializeComponent();

        _allBookings.Add(new("BK-2026-001", "Nguyen Minh Anh", "Deluxe 302", "26 Oct 2026", "29 Oct 2026", "2", "₫4,500,000", "Confirmed", "Returning guest"));
        _allBookings.Add(new("BK-2026-002", "Tran Quoc Bao", "Standard 105", "26 Oct 2026", "28 Oct 2026", "1", "₫1,600,000", "Pending", "Direct booking"));
        _allBookings.Add(new("BK-2026-003", "Le Thu Ha", "Suite 501", "27 Oct 2026", "31 Oct 2026", "2", "₫8,800,000", "Checked-in", "VIP guest"));
        _allBookings.Add(new("BK-2026-004", "Pham Duc Long", "Deluxe 305", "28 Oct 2026", "30 Oct 2026", "3", "₫3,000,000", "Confirmed", "Online booking"));
        _allBookings.Add(new("BK-2026-005", "Vo Ngoc Linh", "Standard 108", "29 Oct 2026", "30 Oct 2026", "1", "₫800,000", "Cancelled", "Cancelled by guest"));
        _allBookings.Add(new("BK-2026-006", "Bui Thanh Tung", "Suite 502", "30 Oct 2026", "02 Nov 2026", "2", "₫6,600,000", "Pending", "Corporate guest"));

        ApplyFilters();
    }

    private async void NewBooking_Click(object? sender, RoutedEventArgs e)
    {
        var dialog = new NewBookingWindow();
        var result = await dialog.ShowDialog<BookingDraft?>(this);

        if (result is null)
            return;

        var bookingId = $"BK-{DateTime.Now:yyyy}-{_allBookings.Count + 1:000}";
        var nights = Math.Max(1, (result.CheckOut.Date - result.CheckIn.Date).Days);
        var display = new BookingDisplayRow(
            bookingId,
            result.GuestName,
            $"{result.RoomType} {result.RoomNumber}",
            result.CheckIn.ToString("dd MMM yyyy"),
            result.CheckOut.ToString("dd MMM yyyy"),
            result.GuestCount.ToString(),
            result.Total.ToString("C0", System.Globalization.CultureInfo.GetCultureInfo("vi-VN")),
            "Pending",
            $"{nights} night(s)");

        _allBookings.Insert(0, display);
        ApplyFilters();
        TotalBookingsText.Text = _allBookings.Count.ToString();
    }

    private void SearchBox_TextChanged(object? sender, TextChangedEventArgs e) => ApplyFilters();

    private void StatusFilter_SelectionChanged(object? sender, SelectionChangedEventArgs e) => ApplyFilters();

    private void ApplyFilters()
    {
        if (BookingsList is null)
            return;

        var search = SearchBox?.Text?.Trim() ?? "";
        var status = (StatusFilter?.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "All statuses";

        var filtered = _allBookings.Where(b =>
            (string.IsNullOrWhiteSpace(search) ||
             b.BookingId.Contains(search, StringComparison.OrdinalIgnoreCase) ||
             b.Guest.Contains(search, StringComparison.OrdinalIgnoreCase) ||
             b.Room.Contains(search, StringComparison.OrdinalIgnoreCase)) &&
            (status == "All statuses" || b.Status.Equals(status, StringComparison.OrdinalIgnoreCase)));

        BookingsList.ItemsSource = filtered.ToList();
    }
}

public sealed class BookingDisplayRow
{
    public string BookingId { get; }
    public string Guest { get; }
    public string Room { get; }
    public string CheckIn { get; }
    public string CheckOut { get; }
    public string Guests { get; }
    public string Total { get; }
    public string Status { get; }
    public string GuestType { get; }
    public string StatusBackground => Status switch
    {
        "Confirmed" => "#DCFCE7",
        "Pending" => "#FEF3C7",
        "Checked-in" => "#DBEAFE",
        "Cancelled" => "#FEE2E2",
        _ => "#F1F5F9"
    };
    public string StatusForeground => Status switch
    {
        "Confirmed" => "#166534",
        "Pending" => "#92400E",
        "Checked-in" => "#1D4ED8",
        "Cancelled" => "#991B1B",
        _ => "#475569"
    };

    public BookingDisplayRow(string bookingId, string guest, string room, string checkIn,
        string checkOut, string guests, string total, string status, string guestType)
    {
        BookingId = bookingId;
        Guest = guest;
        Room = room;
        CheckIn = checkIn;
        CheckOut = checkOut;
        Guests = guests;
        Total = total;
        Status = status;
        GuestType = guestType;
    }
}

public sealed record BookingDraft(
    string GuestName,
    string RoomType,
    string RoomNumber,
    DateTime CheckIn,
    DateTime CheckOut,
    int GuestCount,
    decimal Total);
