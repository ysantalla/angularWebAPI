using Server.Models;
using Server.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services.Interfaces
{
    public interface IProfileService
    {
         Task<ProcessResult<ProfileViewModel>> GetProfileAsync();
         Task<ProcessResult<UserSettingsViewModel>> GetUserSettingsAsync();
         Task<ProcessResult> UpdateUserSettingsAsync(UserSettingsViewModel model);
        
    }
}