
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelAvalonia.Models;
using HotelAvalonia.Services;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HotelAvalonia.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly INewBookingWindowService _newBookingWindowService;

    public ObservableCollection<BookingDisplayRow> Bookings { get; } = new();

    public ObservableCollection<string> StatusOptions { get; } =
    [
        "All statuses",
        "Confirmed",
        "Pending",
        "Checked-in",
        "Cancelled"
    ];

    public ObservableCollection<BookingDisplayRow> FilteredBookings { get; } = new();

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private string _selectedStatus = "All statuses";

    public int TotalBookings => Bookings.Count;

    public int ConfirmedBookings =>
        Bookings.Count(b => b.Status == "Confirmed");

    public int PendingBookings =>
        Bookings.Count(b => b.Status == "Pending");

    public int CancelledBookings =>
        Bookings.Count(b => b.Status == "Cancelled");

    public MainViewModel(INewBookingWindowService newBookingWindowService)
    {
        _newBookingWindowService = newBookingWindowService;

        Bookings.Add(new("BK-2026-001", "Nguyen Minh Anh", "Deluxe 302",
            "26 Oct 2026", "29 Oct 2026", "2", "₫4,500,000",
            "Confirmed", "Returning guest"));

        Bookings.Add(new("BK-2026-002", "Tran Quoc Bao", "Standard 105",
            "26 Oct 2026", "28 Oct 2026", "1", "₫1,600,000",
            "Pending", "Direct booking"));

        Bookings.Add(new("BK-2026-003", "Le Thu Ha", "Suite 501",
            "27 Oct 2026", "31 Oct 2026", "2", "₫8,800,000",
            "Checked-in", "VIP guest"));

        Bookings.Add(new("BK-2026-004", "Pham Duc Long", "Deluxe 305",
            "28 Oct 2026", "30 Oct 2026", "3", "₫3,000,000",
            "Confirmed", "Online booking"));

        Bookings.Add(new("BK-2026-005", "Vo Ngoc Linh", "Standard 108",
            "29 Oct 2026", "30 Oct 2026", "1", "₫800,000",
            "Cancelled", "Cancelled by guest"));

        Bookings.Add(new("BK-2026-006", "Bui Thanh Tung", "Suite 502",
            "30 Oct 2026", "02 Nov 2026", "2", "₫6,600,000",
            "Pending", "Corporate guest"));

        RefreshFilteredBookings();
    }

    partial void OnSearchTextChanged(string value)
    {
        RefreshFilteredBookings();
    }

    partial void OnSelectedStatusChanged(string value)
    {
        RefreshFilteredBookings();
    }

    [RelayCommand]
    private async Task NewBookingAsync()
    {
        var draft = await _newBookingWindowService.ShowAsync();

        // The user cancelled the dialog.
        if (draft is null)
            return;

        var bookingId = $"BK-{DateTime.Now:yyyy}-{Bookings.Count + 1:000}";
        var nights = Math.Max(1, (draft.CheckOut.Date - draft.CheckIn.Date).Days);

        var display = new BookingDisplayRow(
            bookingId,
            draft.GuestName,
            $"{draft.RoomType} {draft.RoomNumber}",
            draft.CheckIn.ToString("dd MMM yyyy"),
            draft.CheckOut.ToString("dd MMM yyyy"),
            draft.GuestCount.ToString(),
            draft.Total.ToString("C0", CultureInfo.GetCultureInfo("vi-VN")),
            "Pending",
            $"{nights} night(s)");

        Bookings.Insert(0, display);

        OnPropertyChanged(nameof(TotalBookings));
        OnPropertyChanged(nameof(ConfirmedBookings));
        OnPropertyChanged(nameof(PendingBookings));
        OnPropertyChanged(nameof(CancelledBookings));

        RefreshFilteredBookings();
    }

    private void RefreshFilteredBookings()
    {
        var search = SearchText.Trim();

        var filtered = Bookings.Where(booking =>
            (string.IsNullOrWhiteSpace(search) ||
             booking.BookingId.Contains(search, StringComparison.OrdinalIgnoreCase) ||
             booking.Guest.Contains(search, StringComparison.OrdinalIgnoreCase) ||
             booking.Room.Contains(search, StringComparison.OrdinalIgnoreCase)) &&
            (SelectedStatus == "All statuses" ||
             booking.Status.Equals(SelectedStatus, StringComparison.OrdinalIgnoreCase)));

        FilteredBookings.Clear();

        foreach (var booking in filtered)
            FilteredBookings.Add(booking);
    }
}
