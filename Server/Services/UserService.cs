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
    public class UserService : BaseService, IUserService
    {
        public UserService(UserManager<ApplicationUser> userManager,
                              IHttpContextAccessor contextAccessor,
                              ApplicationDbContext context)
            : base(userManager, contextAccessor, context)
        {
        }

        public async Task<ProcessResult<List<UserViewModel>>> GetListAsync(string sortOrder, string searchString, int pageIndex,  int pageSize)
        {
            var userIQ = from s in context.Users
                                            select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                userIQ = userIQ.Where(s => s.Firstname.Contains(searchString) ||
                         s.Lastname.Contains(searchString) ||
                         s.Email.Contains(searchString)
                );
            }

            switch (sortOrder)
            {
                case "firstame_desc":
                    userIQ = userIQ.OrderByDescending(s => s.Firstname);
                    break;
                case "firstname_asc":
                    userIQ = userIQ.OrderBy(s => s.Firstname);
                    break;
                case "lastname_desc":
                    userIQ = userIQ.OrderByDescending(s => s.Lastname);
                    break;
                case "lastname_asc":
                    userIQ = userIQ.OrderBy(s => s.Firstname);
                    break;
                case "email_desc":
                    userIQ = userIQ.OrderByDescending(s => s.Lastname);
                    break;
                case "email_asc":
                    userIQ = userIQ.OrderBy(s => s.Firstname);
                    break;
                case "id_desc":
                    userIQ = userIQ.OrderByDescending(s => s.Id);
                    break;
                case "id_asc":
                    userIQ = userIQ.OrderBy(s => s.Id);
                    break;
                default:
                    userIQ = userIQ.OrderBy(s => s.Firstname);
                    break;
            }

            var countItems = await userIQ.CountAsync();

            if (pageIndex != 0 && pageSize != 0) {
                userIQ = userIQ.Skip((pageIndex - 1) * pageSize).Take(pageSize);                
            }

            Func<Task<List<UserViewModel>>> action = async () =>
            {
                var result = await userIQ.Select(x => new UserViewModel
                {
                    Id = x.Id,
                    Firstname = x.Firstname,
                    Lastname = x.Lastname,
                    Email = x.Email
                }).ToListAsync();

                return result;
            };

            return await Process.RunAsync(action, countItems);
        }

        public async Task<ProcessResult<int>> CountAsync(string searchString = "")
        {
            var userIQ = from s in context.Users
                                            select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                userIQ = userIQ.Where(s => s.Firstname.Contains(searchString));
            }           

            Func<Task<int>> action = async () =>
            {
                var countItems = await userIQ.CountAsync();

                return countItems;
            };

            return await Process.RunAsync(action);
        }

        
    }
}