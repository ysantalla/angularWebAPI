using Server.Models;
using Server.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services.Interfaces
{
    public interface IGuestService
    {
         Task<ProcessResult> CreateAsync(Guest model);
         
         Task<ProcessResult<Guest>> RetrieveAsync(long id);

         Task<ProcessResult> UpdateAsync(long id, Guest model);

         Task<ProcessResult> DeleteAsync(long id);

         Task<ProcessResult> RestoreAsync(long id);

         Task<ProcessResult<List<Guest>>> ListAsync(GetListViewModel<GuestFilter> listModel);
         
         Task<ProcessResult<int>> CountAsync(GuestFilter filter);
   
    }
}