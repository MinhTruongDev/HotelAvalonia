namespace HotelAvalonia.Models;

public sealed class BookingRow
{
    public string BookingId { get; init; } = "";
    public string Guest { get; init; } = "";
    public string Room { get; init; } = "";
    public string CheckIn { get; init; } = "";
    public string CheckOut { get; init; } = "";
    public string Guests { get; init; } = "";
    public string Total { get; init; } = "";
    public string Status { get; init; } = "";
}
