using Server.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Server.Services.Interfaces
{
    public interface IFileService
    {
         Task<ProcessResult<string>> UploadAsync(IFormFile file);
    }
}