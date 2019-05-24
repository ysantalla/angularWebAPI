using Server.Models;
using Server.Services.Interfaces;
using Server.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<string>), 400)]
    public class GuestController : BaseController
    {
        private readonly IGuestService _GuestService;
        private readonly ILogger<GuestController> _logger;
        
        public GuestController(UserManager<ApplicationUser> userManager, 
                              IGuestService GuestService,
                              ILogger<GuestController> logger) 
            : base(userManager)
        {
            this._GuestService = GuestService;
            this._logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody]GuestViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _GuestService.CreateAsync(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }

        [HttpPatch]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Update([FromBody]GuestViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _GuestService.UpdateAsync(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> RemoveOrRestore([FromBody]GuestIdViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _GuestService.RemoveOrRestoreAsync(model.Id);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GuestViewModel>), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> List(GuestFilterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _GuestService.GetListAsync(model.sortOrder, model.searchString, model.countryID, model.citizenshipID, model.pageIndex, model.pageSize);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(result);
            
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(int), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Count(GuestFilterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _GuestService.CountAsync(model.searchString, model.countryID, model.citizenshipID);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
            
        }

        [HttpGet]
        [ProducesResponseType(typeof(GuestViewModel), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(GuestIdViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _GuestService.GetByIdAsync(model.Id);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        } 
    }
}