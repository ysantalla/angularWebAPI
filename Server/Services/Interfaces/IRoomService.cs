using Server.Models;
using Server.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services.Interfaces
{
    public interface IRoomService
    {
         Task<ProcessResult> CreateAsync(Room model);
         
         Task<ProcessResult<Room>> RetrieveAsync(long id);

         Task<ProcessResult> UpdateAsync(long id, Room model);

         Task<ProcessResult> DeleteAsync(long id);

         Task<ProcessResult<List<Room>>> ListAsync(GetListViewModel<RoomFilter> listModel);
         
         Task<ProcessResult<int>> CountAsync(RoomFilter filter);
   
    }
}