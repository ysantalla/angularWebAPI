using Server.Models;
using Server.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services.Interfaces
{
    public interface IGuestService
    {
         Task<ProcessResult> CreateAsync(GuestViewModel model);
         Task<ProcessResult> UpdateAsync(GuestViewModel model);
         Task<ProcessResult<Guest>> GetByIdAsync(long id);
         Task<ProcessResult<List<Guest>>> GetListAsync(string sortOrder, string searchString, long countryID, long citizenshipID, int pageIndex,  int pageSize);
         Task<ProcessResult<int>> CountAsync(string searchString, long countryID, long citizenshipID);
         Task<ProcessResult> RemoveOrRestoreAsync(long id);
    }
}