using Server.Models;
using Server.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Services.Interfaces
{
    public interface ITagService
    {
         Task<ProcessResult<List<TagViewModel>>> GetPopularAsync(int count);
    }
}