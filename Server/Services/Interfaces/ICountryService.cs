using Server.Models;
using Server.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services.Interfaces
{
    public interface ICountryService
    {
         Task<ProcessResult> CreateAsync(CountryViewModel model);
         Task<ProcessResult> UpdateAsync(CountryViewModel model);
         Task<ProcessResult<CountryViewModel>> GetByIdAsync(long id);
         Task<ProcessResult<List<CountryViewModel>>> GetListAsync(string sortOrder, string searchString, int pageIndex,  int pageSize);
         Task<ProcessResult<int>> CountAsync(string searchString);
         Task<ProcessResult> RemoveOrRestoreAsync(long id);
    }
}