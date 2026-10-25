using System;

namespace HotelAvalonia.Models;

public sealed record BookingDraft(
    string GuestName,
    string RoomType,
    string RoomNumber,
    DateTime CheckIn,
    DateTime CheckOut,
    int GuestCount,
    decimal Total);