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
    public class CitizenshipService : BaseService, ICitizenshipService
    {
        public CitizenshipService(UserManager<ApplicationUser> userManager,
                              IHttpContextAccessor contextAccessor,
                              ApplicationDbContext context)
            : base(userManager, contextAccessor, context)
        {
        }

        public async Task<ProcessResult> CreateAsync(CitizenshipViewModel model)
        {
            Func<Task> action = async () =>
            {

                var citizenshipExist = await context.Citizenships.Where(x => x.Name == model.Name).CountAsync();

                if (citizenshipExist > 0)
                {
                    throw new InvalidOperationException("Ya existe una ciudadanía con este nombre");
                }

                var citizenshipEntity = await GetOrCreateEntityAsync(context.Citizenships, x => x.Id == model.Id);
                var citizenship = citizenshipEntity.result;

                citizenship.Name = model.Name;
                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }


        public async Task<ProcessResult> UpdateAsync(CitizenshipViewModel model)
        {

            Func<Task> action = async () =>
            {
                var citizenshipExist = await context.Citizenships.Where(x => x.Name == model.Name && x.Id != model.Id).CountAsync();

                if (citizenshipExist > 0)
                {
                    throw new InvalidOperationException("Ya existe una ciudadanía con este nombre");
                }

                var citizenshipEntity = await GetOrCreateEntityAsync(context.Citizenships, x => x.Id == model.Id);
                var citizenship = citizenshipEntity.result;

                citizenship.Name = model.Name;
                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> RemoveOrRestoreAsync(long id)
        {
            Func<Task> action = async () =>
            {
                var citizenship = await context.Citizenships.Where(x => x.Id == id).SingleAsync();

                citizenship.IsDeleted = !citizenship.IsDeleted;
                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<Citizenship>> GetByIdAsync(long id)
        {
            Func<Task<Citizenship>> action = async () =>
            {
                var result = await context.Citizenships.Where(x => x.Id == id).FirstAsync();

                var resultView = new Citizenship
                {
                    Id = result.Id,
                    Name = result.Name
                };

                return resultView;
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<List<Citizenship>>> GetListAsync(string sortOrder, string searchString, int pageIndex,  int pageSize)
        {
            IQueryable<Citizenship> citizenshipIQ = from s in context.Citizenships select s;
            
            citizenshipIQ = citizenshipIQ.Where(s => s.IsDeleted == false);

            if (!String.IsNullOrEmpty(searchString))
            {
                citizenshipIQ = citizenshipIQ.Where(s => s.Name.Contains(searchString) && s.IsDeleted == false);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    citizenshipIQ = citizenshipIQ.OrderByDescending(s => s.Name);
                    break;
                case "name_asc":
                    citizenshipIQ = citizenshipIQ.OrderBy(s => s.Name);
                    break;
                case "id_desc":
                    citizenshipIQ = citizenshipIQ.OrderByDescending(s => s.Id);
                    break;
                case "id_asc":
                    citizenshipIQ = citizenshipIQ.OrderBy(s => s.Id);
                    break;
                default:
                    citizenshipIQ = citizenshipIQ.OrderBy(s => s.Name);
                    break;
            }

            var countItems = await citizenshipIQ.CountAsync();

            if (pageIndex != 0 && pageSize != 0) {
                citizenshipIQ = citizenshipIQ.Skip((pageIndex - 1) * pageSize).Take(pageSize);                
            }

            Func<Task<List<Citizenship>>> action = async () =>
            {
                var result = await citizenshipIQ.Select(x => new Citizenship
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsDeleted = x.IsDeleted
                }).ToListAsync();

                return result;
            };

            return await Process.RunAsync(action, countItems);
        }

        public async Task<ProcessResult<int>> CountAsync(string searchString = "")
        {
            IQueryable<Citizenship> citizenshipIQ = from s in context.Citizenships select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                citizenshipIQ = citizenshipIQ.Where(s => s.Name.Contains(searchString) && s.IsDeleted == false);
            }           

            Func<Task<int>> action = async () =>
            {
                var countItems = await citizenshipIQ.CountAsync();

                return countItems;
            };

            return await Process.RunAsync(action);
        }

    }
}