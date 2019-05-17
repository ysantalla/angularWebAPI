using Server.Models;
using Server.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services.Interfaces
{
    public interface ICountryService
    {
         Task<ProcessResult> CreateOrUpdateAsync(CountryViewModel model);
         Task<ProcessResult<List<CountryViewModel>>> GetListAsync(long countryId);
         Task<ProcessResult> RemoveOrRestoreAsync(long id);
    }
}