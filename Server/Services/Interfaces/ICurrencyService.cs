using Server.Models;
using Server.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services.Interfaces
{
    public interface ICurrencyService
    {
         Task<ProcessResult> CreateAsync(CurrencyViewModel model);
         Task<ProcessResult> UpdateAsync(CurrencyViewModel model);
         Task<ProcessResult<Currency>> GetByIdAsync(long id);
         Task<ProcessResult<List<Currency>>> GetListAsync(string sortOrder, string searchString, int pageIndex,  int pageSize);
         Task<ProcessResult<int>> CountAsync(string searchString);
         Task<ProcessResult> RemoveOrRestoreAsync(long id);
    }
}