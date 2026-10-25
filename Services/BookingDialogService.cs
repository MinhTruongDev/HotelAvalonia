using Avalonia.Controls;
using HotelAvalonia.Models;
using System;
using System.Threading.Tasks;

namespace HotelAvalonia.Services;

public sealed class BookingDialogService : IBookingDialogService
{
    private Window? _owner;

    public void SetOwner(Window owner)
    {
        _owner = owner;
    }

    public Task<bool> ConfirmBookingAsync(BookingDraft draft)
    {
        if (_owner is null)
        {
            throw new InvalidOperationException(
                "The confirmation dialog owner has not been initialized.");
        }

        var dialog = new BookingConfirmationWindow(draft);

        return dialog.ShowDialog<bool>(_owner);
    }
}