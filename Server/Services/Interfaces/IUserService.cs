using Server.Models;
using Server.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services.Interfaces
{
    public interface IUserService
    {
         Task<ProcessResult<List<UserViewModel>>> GetListAsync(string sortOrder, string searchString, int pageIndex,  int pageSize);
         Task<ProcessResult<int>> CountAsync(string searchString);

        
    }
}