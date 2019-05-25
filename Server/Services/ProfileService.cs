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

                var roles = await this.userManager.GetRolesAsync(user);

                var userView =  new ProfileViewModel
                                    {
                                        Email = user.Email,
                                        UserName = user.UserName,
                                        Firstname = user.Firstname,
                                        Lastname = user.Lastname,
                                        Roles = roles.ToList()
                                    };

                return userView;
            };
            
            return await Process.RunAsync(action);
        }

    }
}