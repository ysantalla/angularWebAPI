using Server;
using Server.Models;
using Server.Enums;
using Server.Services.Interfaces;
using Server.ViewModels;
using Server.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Server.Services
{
    public class ProfileService : BaseService, IProfileService
    {
        public ProfileService(UserManager<ApplicationUser> userManager, 
                           IHttpContextAccessor contextAccessor, 
                           ApplicationDbContext context) 
            : base(userManager, contextAccessor, context)
        {
        }

        public async Task<ProcessResult<ProfileViewModel>> GetProfileAsync()
        {
            Func<Task<ProfileViewModel>> action = async () => 
            {
                var user = await this.userManager.FindByIdAsync(CurrentUser.Id.ToString());

                var userSettings =  new ProfileViewModel
                                    {
                                        Name = user.NormalizedUserName,
                                        UserName = user.UserName
                                    };

                return userSettings;
            };
            
            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<UserSettingsViewModel>> GetUserSettingsAsync()
        {
            Func<Task<UserSettingsViewModel>> action = async () => 
            {
                var user = await this.userManager.FindByIdAsync(CurrentUser.Id.ToString());

                Console.WriteLine(user);

                var userSettings =  new UserSettingsViewModel
                                    {
                                        UserName = user.UserName
                                    };

                return userSettings;
            };
            
            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> UpdateUserSettingsAsync(UserSettingsViewModel model)
        {
            Func<Task> action = async () =>
            {
                var user = await this.userManager.FindByIdAsync(CurrentUser.Id.ToString());
                
                user.NormalizedUserName = model.UserName.ToUpper();
                user.UserName = model.UserName;

                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }
    }
}