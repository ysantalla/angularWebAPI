using Server.Models;
using Server.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services.Interfaces
{
    public interface ICrudService<T, T_FILTER>
    {
         Task<ProcessResult> CreateAsync(T model);
         
         Task<ProcessResult<T>> RetrieveAsync(long id);

         Task<ProcessResult> UpdateAsync(long id, T model);

         Task<ProcessResult> DeleteAsync(long id);

         Task<ProcessResult<List<T>>> ListAsync(GetListViewModel<T_FILTER> listModel);
         
         Task<ProcessResult<int>> CountAsync(T_FILTER filter);
   
    }
}