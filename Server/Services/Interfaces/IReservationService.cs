using Server.Models;
using Server.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services.Interfaces
{
    public interface IReservationService
    {
         Task<ProcessResult> CreateAsync(Reservation model);
         
         Task<ProcessResult<Reservation>> RetrieveAsync(long id);

         Task<ProcessResult> UpdateAsync(long id, Reservation model);

         Task<ProcessResult> DeleteAsync(long id);

         Task<ProcessResult> RestoreAsync(long id);

         Task<ProcessResult<List<Reservation>>> ListAsync(GetListViewModel<ReservationFilter> listModel);
         
         Task<ProcessResult<int>> CountAsync(ReservationFilter filter);
   
    }
}