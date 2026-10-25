namespace HotelAvalonia.Models;

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

    public BookingDisplayRow(
        string bookingId,
        string guest,
        string room,
        string checkIn,
        string checkOut,
        string guests,
        string total,
        string status,
        string guestType)
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
