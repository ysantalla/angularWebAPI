using Server.Models;
using Server.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services.Interfaces
{
    public interface IReservationService : ICrudService<Reservation, ReservationFilter>
    {
         Task<ProcessResult> AddGuestAsync(long reservationId, long guestId);

         Task<ProcessResult> RemoveGuestAsync(long reservationId, long guestId);
    }
}