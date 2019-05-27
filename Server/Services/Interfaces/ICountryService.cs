using Server.Models;
using Server.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services.Interfaces
{
    public interface ICountryService
    {
         Task<ProcessResult> CreateAsync(Country model);
         
         Task<ProcessResult<Country>> RetrieveAsync(long id);

         Task<ProcessResult> UpdateAsync(long id, Country model);

         Task<ProcessResult> DeleteAsync(long id);

         Task<ProcessResult<List<Country>>> ListAsync(GetListViewModel<CountryFilter> listModel);
         
         Task<ProcessResult<int>> CountAsync(CountryFilter filter);
   
    }
}