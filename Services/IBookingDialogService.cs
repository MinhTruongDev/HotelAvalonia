using HotelAvalonia.Models;
using System.Threading.Tasks;

namespace HotelAvalonia.Services;

public interface IBookingDialogService
{
    Task<bool> ConfirmBookingAsync(BookingDraft draft);
}