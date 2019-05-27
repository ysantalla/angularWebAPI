using Server.Models;
using Server.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services.Interfaces
{
    public interface ICurrencyService
    {
         Task<ProcessResult> CreateAsync(Currency model);
         
         Task<ProcessResult<Currency>> RetrieveAsync(long id);

         Task<ProcessResult> UpdateAsync(long id, Currency model);

         Task<ProcessResult> DeleteAsync(long id);

         Task<ProcessResult<List<Currency>>> ListAsync(GetListViewModel<CurrencyFilter> listModel);
         
         Task<ProcessResult<int>> CountAsync(CurrencyFilter filter);
   
    }
}