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
    public class CountryService : BaseService, ICountryService
    {
        public CountryService(UserManager<ApplicationUser> userManager,
                              IHttpContextAccessor contextAccessor,
                              ApplicationDbContext context)
            : base(userManager, contextAccessor, context)
        {
        }

        public async Task<ProcessResult> CreateAsync(CountryViewModel model)
        {
            Func<Task> action = async () =>
            {

                var countryExist = await context.Countries.Where(x => x.Name == model.Name).CountAsync();

                if (countryExist > 0)
                {
                    throw new InvalidOperationException("Ya existe un país con este nombre");
                }

                var countryEntity = await GetOrCreateEntityAsync(context.Countries, x => x.Id == model.Id);
                var country = countryEntity.result;

                country.Name = model.Name;
                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }


        public async Task<ProcessResult> UpdateAsync(CountryViewModel model)
        {

            Func<Task> action = async () =>
            {
                var countryExist = await context.Countries.Where(x => x.Name == model.Name && x.Id != model.Id).CountAsync();

                if (countryExist > 0)
                {
                    throw new InvalidOperationException("Ya existe un país con este nombre");
                }

                var countryEntity = await GetOrCreateEntityAsync(context.Countries, x => x.Id == model.Id);
                var country = countryEntity.result;

                country.Name = model.Name;
                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> RemoveOrRestoreAsync(long id)
        {
            Func<Task> action = async () =>
            {
                var country = await context.Countries.Where(x => x.Id == id).SingleAsync();

                country.IsDeleted = !country.IsDeleted;
                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<CountryViewModel>> GetByIdAsync(long id)
        {
            Func<Task<CountryViewModel>> action = async () =>
            {
                var result = await context.Countries.Where(x => x.Id == id).FirstAsync();

                var resultView = new CountryViewModel
                {
                    Id = result.Id,
                    Name = result.Name
                };

                return resultView;
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<List<CountryViewModel>>> GetListAsync(string sortOrder, string searchString, int pageIndex,  int pageSize)
        {
            IQueryable<Country> countryIQ = from s in context.Countries
                                            select s;
            
            countryIQ = countryIQ.Where(s => s.IsDeleted == false);

            if (!String.IsNullOrEmpty(searchString))
            {
                countryIQ = countryIQ.Where(s => s.Name.Contains(searchString) && s.IsDeleted == false);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    countryIQ = countryIQ.OrderByDescending(s => s.Name);
                    break;
                case "name_asc":
                    countryIQ = countryIQ.OrderBy(s => s.Name);
                    break;
                case "id_desc":
                    countryIQ = countryIQ.OrderByDescending(s => s.Id);
                    break;
                case "id_asc":
                    countryIQ = countryIQ.OrderBy(s => s.Id);
                    break;
                default:
                    countryIQ = countryIQ.OrderBy(s => s.Name);
                    break;
            }

            var countItems = await countryIQ.CountAsync();

            if (pageIndex != 0 && pageSize != 0) {
                countryIQ = countryIQ.Skip((pageIndex - 1) * pageSize).Take(pageSize);                
            }

            Func<Task<List<CountryViewModel>>> action = async () =>
            {
                var result = await countryIQ.Select(x => new CountryViewModel
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
            IQueryable<Country> countryIQ = from s in context.Countries
                                            select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                countryIQ = countryIQ.Where(s => s.Name.Contains(searchString) && s.IsDeleted == false);
            }           

            Func<Task<int>> action = async () =>
            {
                var countItems = await countryIQ.CountAsync();

                return countItems;
            };

            return await Process.RunAsync(action);
        }

    }
}