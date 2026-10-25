using Avalonia.Controls;
using HotelAvalonia.Models;
using System.Threading.Tasks;

namespace HotelAvalonia.Services;

public interface INewBookingWindowService
{
    void SetOwner(Window owner);

    Task<BookingDraft?> ShowAsync();
}