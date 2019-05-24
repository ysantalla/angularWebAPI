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
    public class GuestService : BaseService, IGuestService
    {
        public GuestService(UserManager<ApplicationUser> userManager,
                              IHttpContextAccessor contextAccessor,
                              ApplicationDbContext context)
            : base(userManager, contextAccessor, context)
        {
        }

        public async Task<ProcessResult> CreateAsync(GuestViewModel model)
        {
            Func<Task> action = async () =>
            {

                var GuestExist = await context.Guests.Where(x => x.Identification == model.Identification).CountAsync();

                if (GuestExist > 0)
                {
                    throw new InvalidOperationException("Ya existe un invitado con esa identificación");
                }

                var GuestEntity = await GetOrCreateEntityAsync(context.Guests, x => x.Id == model.Id);
                var Guest = GuestEntity.result;

                Guest.Name = model.Name;
                Guest.Phone = model.Phone;
                Guest.Identification = model.Identification;
                Guest.Birthday = model.Birthday;
                Guest.CountryID = model.CountryID;
                Guest.CitizenshipID = model.CitizenshipID;

                await context.SaveChangesAsync();
            };
            return await Process.RunAsync(action);
        }


        public async Task<ProcessResult> UpdateAsync(GuestViewModel model)
        {

            Func<Task> action = async () =>
            {
                var GuestExist = await context.Guests.Where(x => x.Name == model.Name && x.Id != model.Id).CountAsync();

                if (GuestExist > 0)
                {
                    throw new InvalidOperationException("Ya existe un invitado con esa identificación");
                }

                var GuestEntity = await GetOrCreateEntityAsync(context.Guests, x => x.Id == model.Id);
                var Guest = GuestEntity.result;

                Guest.Name = model.Name;
                Guest.Phone = model.Phone;
                Guest.Identification = model.Identification;
                Guest.Birthday = model.Birthday;
                Guest.CountryID = model.CountryID;
                Guest.CitizenshipID = model.CitizenshipID;

                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> RemoveOrRestoreAsync(long id)
        {
            Func<Task> action = async () =>
            {
                var Guest = await context.Guests.Where(x => x.Id == id).SingleAsync();

                Guest.IsDeleted = !Guest.IsDeleted;
                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<Guest>> GetByIdAsync(long id)
        {
            Func<Task<Guest>> action = async () =>
            {
                var result = await context.Guests.Where(x => x.Id == id).FirstAsync();

                var resultView = new Guest
                {
                    Id = result.Id,
                    Name = result.Name,
                    Phone = result.Phone,
                    Identification = result.Identification,
                    Birthday = result.Birthday,
                    CountryID = result.CountryID,
                    CitizenshipID = result.CitizenshipID
                };

                return resultView;
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<List<Guest>>> GetListAsync(string sortOrder, string searchString, long countryID, long citizenshipID,  int pageIndex,  int pageSize)
        {
            IQueryable<Guest> GuestIQ = from s in context.Guests select s;
            
            GuestIQ = GuestIQ.Where(s => s.IsDeleted == false);

            if (!String.IsNullOrEmpty(searchString))
            {
                GuestIQ = GuestIQ.Where(s => s.Name.Contains(searchString) && s.IsDeleted == false);
            }
            if (countryID != 0) {
                GuestIQ = GuestIQ.Where(s => s.CountryID == countryID);
            }
            if (citizenshipID != 0) {
                GuestIQ = GuestIQ.Where(s => s.CitizenshipID == citizenshipID);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    GuestIQ = GuestIQ.OrderByDescending(s => s.Name);
                    break;
                case "name_asc":
                    GuestIQ = GuestIQ.OrderBy(s => s.Name);
                    break;
                case "id_desc":
                    GuestIQ = GuestIQ.OrderByDescending(s => s.Id);
                    break;
                case "id_asc":
                    GuestIQ = GuestIQ.OrderBy(s => s.Id);
                    break;
                default:
                    GuestIQ = GuestIQ.OrderBy(s => s.Name);
                    break;
            }

            var countItems = await GuestIQ.CountAsync();

            if (pageIndex != 0 && pageSize != 0) {
                GuestIQ = GuestIQ.Skip((pageIndex - 1) * pageSize).Take(pageSize);                
            }

            Func<Task<List<Guest>>> action = async () =>
            {
                var result = await GuestIQ.Select(x => new Guest
                {
                    Id = x.Id,
                    Name = x.Name,
                    Phone = x.Phone,
                    Identification = x.Identification,
                    Birthday = x.Birthday,
                    CountryID = x.CountryID,
                    CitizenshipID = x.CitizenshipID,

                    IsDeleted = x.IsDeleted
                }).ToListAsync();

                return result;
            };

            return await Process.RunAsync(action, countItems);
        }

        public async Task<ProcessResult<int>> CountAsync(string searchString = "", long countryID = 0, long citizenshipID = 0)
        {
            IQueryable<Guest> GuestIQ = from s in context.Guests select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                GuestIQ = GuestIQ.Where(s => s.Name.Contains(searchString) && s.IsDeleted == false);
            }
            if (countryID != 0) {
                GuestIQ = GuestIQ.Where(s => s.CountryID == countryID);
            }
            if (citizenshipID != 0) {
                GuestIQ = GuestIQ.Where(s => s.CitizenshipID == citizenshipID);
            }       

            Func<Task<int>> action = async () =>
            {
                var countItems = await GuestIQ.CountAsync();

                return countItems;
            };

            return await Process.RunAsync(action);
        }

    }
}