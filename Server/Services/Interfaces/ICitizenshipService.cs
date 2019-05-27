using Server.Models;
using Server.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services.Interfaces
{
    public interface ICitizenshipService
    {
         Task<ProcessResult> CreateAsync(Citizenship model);
         
         Task<ProcessResult<Citizenship>> RetrieveAsync(long id);

         Task<ProcessResult> UpdateAsync(long id, Citizenship model);

         Task<ProcessResult> DeleteAsync(long id);

         Task<ProcessResult<List<Citizenship>>> ListAsync(GetListViewModel<CitizenshipFilter> listModel);
         
         Task<ProcessResult<int>> CountAsync(CitizenshipFilter filter);
   
    }
}