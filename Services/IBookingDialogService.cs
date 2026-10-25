using Avalonia.Controls;
using HotelAvalonia.Models;
using System.Threading.Tasks;

namespace HotelAvalonia.Services;

public interface IBookingDialogService
{
    void SetOwner(Window owner);

    Task<bool> ConfirmBookingAsync(BookingDraft draft);
}