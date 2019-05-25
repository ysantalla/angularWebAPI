using Server.Models;
using Server.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services.Interfaces
{
    public interface IAgencyService
    {
         Task<ProcessResult> CreateAsync(Agency model);
         
         Task<ProcessResult<Agency>> RetrieveAsync(long id);

         Task<ProcessResult> UpdateAsync(long id, Agency model);

         Task<ProcessResult> DeleteAsync(long id);

         Task<ProcessResult> RestoreAsync(long id);

         Task<ProcessResult<List<Agency>>> ListAsync(GetListViewModel<AgencyFilter> listModel);
         
         Task<ProcessResult<int>> CountAsync(AgencyFilter filter);
   
    }
}