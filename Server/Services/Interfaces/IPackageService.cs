using Server.Models;
using Server.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services.Interfaces
{
    public interface IPackageService
    {
         Task<ProcessResult> CreateAsync(Package model);
         
         Task<ProcessResult<Package>> RetrieveAsync(long id);

         Task<ProcessResult> UpdateAsync(long id, Package model);

         Task<ProcessResult> DeleteAsync(long id);

         Task<ProcessResult<List<Package>>> ListAsync(GetListViewModel<PackageFilter> listModel);
         
         Task<ProcessResult<int>> CountAsync(PackageFilter filter);
   
    }
}