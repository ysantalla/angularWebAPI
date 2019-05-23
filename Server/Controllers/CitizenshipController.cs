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
    public class CitizenshipController : BaseController
    {
        private readonly ICitizenshipService _CitizenshipService;
        private readonly ILogger<CitizenshipController> _logger;
        
        public CitizenshipController(UserManager<ApplicationUser> userManager, 
                              ICitizenshipService CitizenshipService,
                              ILogger<CitizenshipController> logger) 
            : base(userManager)
        {
            this._CitizenshipService = CitizenshipService;
            this._logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody]CitizenshipViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _CitizenshipService.CreateAsync(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }

        [HttpPatch]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Update([FromBody]CitizenshipViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _CitizenshipService.UpdateAsync(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> RemoveOrRestore([FromBody]CitizenshipIdViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _CitizenshipService.RemoveOrRestoreAsync(model.Id);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<CitizenshipViewModel>), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> List(CitizenshipFilterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _CitizenshipService.GetListAsync(model.sortOrder, model.searchString, model.pageIndex, model.pageSize);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            // var countItems = await _CitizenshipService.CountAsync(model.searchString);

            return Ok(result);
            
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(int), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Count()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _CitizenshipService.CountAsync("");
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            var countItems = await _CitizenshipService.CountAsync("");
            
            return Ok(result);
            
        }

        [HttpGet]
        [ProducesResponseType(typeof(CitizenshipViewModel), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(CitizenshipIdViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _CitizenshipService.GetByIdAsync(model.Id);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        } 
    }
}