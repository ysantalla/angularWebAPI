using Server.Models;
using Server.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services.Interfaces
{
    public interface ICitizenshipService
    {
         Task<ProcessResult> CreateAsync(CitizenshipViewModel model);
         Task<ProcessResult> UpdateAsync(CitizenshipViewModel model);
         Task<ProcessResult<Citizenship>> GetByIdAsync(long id);
         Task<ProcessResult<List<Citizenship>>> GetListAsync(string sortOrder, string searchString, int pageIndex,  int pageSize);
         Task<ProcessResult<int>> CountAsync(string searchString);
         Task<ProcessResult> RemoveOrRestoreAsync(long id);
    }
}