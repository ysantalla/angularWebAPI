using System;
using System.Threading.Tasks;
using Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Server.Data
{
    public static class Seed
    {
        public static async Task CreateRoles(IServiceProvider serviceProvider, IConfiguration Configuration)
        {
            // Adding customs roles
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "Admin", "Manager" };
            IdentityResult roleResult;
            
            foreach (var roleName in roleNames)
            {
                // creating the roles and seeding them to the database
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    ApplicationRole role = new ApplicationRole();
                    role.Name = roleName;
                    roleResult = await RoleManager.CreateAsync(role);
                }
            }
            
            // Creating a super user who could maintain the web app
            var poweruser = new ApplicationUser
            {
                UserName = Configuration["SuperUser:username"],
                Email = Configuration["SuperUser:email"],
                Firstname = Configuration["SuperUser:firstname"],
                Lastname = Configuration["SuperUser:lastname"]
            };
            
            string userPassword = Configuration["SuperUser:password"];

            var user = await UserManager.FindByEmailAsync(Configuration["SuperUser:email"]);

            if (user == null)
            {
                var createPowerUser = await UserManager.CreateAsync(poweruser, userPassword);
                if (createPowerUser.Succeeded)
                {
                    // Here we assign the new user the "Admin" role 
                    await UserManager.AddToRoleAsync(poweruser, "Admin");
                }
            }
        }
    }
}