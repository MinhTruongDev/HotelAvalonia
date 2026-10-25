using Avalonia.Controls;
using HotelAvalonia.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace HotelAvalonia.Services;

public sealed class NewBookingWindowService : INewBookingWindowService
{
    private Window? _owner;

    public void SetOwner(Window owner)
    {
        _owner = owner;
    }

    public async Task<BookingDraft?> ShowAsync()
    {
        if (_owner is null)
        {
            throw new InvalidOperationException(
                "The owner window has not been configured.");
        }

        var dialog = App.Services
            .GetRequiredService<NewBookingWindow>();

        return await dialog.ShowDialog<BookingDraft?>(_owner);
    }
}