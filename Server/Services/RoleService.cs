using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Server.Services.Interfaces;
using Server.Models;
using Server.ViewModels;

namespace Server.Services
{
    public class RoleService : BaseService, IRoleService
    {
        public RoleService(UserManager<ApplicationUser> userManager,
                              IHttpContextAccessor contextAccessor,
                              ApplicationDbContext context)
            : base(userManager, contextAccessor, context)
        {
        }

        public async Task<ProcessResult<List<RoleViewModel>>> GetListAsync(string sortOrder, string searchString, int pageIndex,  int pageSize)
        {
            var roleIQ = from s in context.Roles
                                            select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                roleIQ = roleIQ.Where(s => s.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    roleIQ = roleIQ.OrderByDescending(s => s.Name);
                    break;
                case "name_asc":
                    roleIQ = roleIQ.OrderBy(s => s.Name);
                    break;
                case "id_desc":
                    roleIQ = roleIQ.OrderByDescending(s => s.Id);
                    break;
                case "id_asc":
                    roleIQ = roleIQ.OrderBy(s => s.Id);
                    break;
                default:
                    roleIQ = roleIQ.OrderBy(s => s.Name);
                    break;
            }

            var countItems = await roleIQ.CountAsync();

            if (pageIndex != 0 && pageSize != 0) {
                roleIQ = roleIQ.Skip((pageIndex - 1) * pageSize).Take(pageSize);                
            }

            Func<Task<List<RoleViewModel>>> action = async () =>
            {
                var result = await roleIQ.Select(x => new RoleViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();

                return result;
            };

            return await Process.RunAsync(action, countItems);
        }

        public async Task<ProcessResult<int>> CountAsync(string searchString = "")
        {
            var roleIQ = from s in context.Roles
                                            select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                roleIQ = roleIQ.Where(s => s.Name.Contains(searchString));
            }           

            Func<Task<int>> action = async () =>
            {
                var countItems = await roleIQ.CountAsync();

                return countItems;
            };

            return await Process.RunAsync(action);
        }

        
    }
}