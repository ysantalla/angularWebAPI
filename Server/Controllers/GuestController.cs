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
    [Route("api/guests")]
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
        public async Task<IActionResult> Create([FromBody]Guest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _GuestService.CreateAsync(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Guest), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Retrieve([FromRoute] long id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _GuestService.RetrieveAsync(id);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        } 

        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Update([FromRoute] long id, [FromBody]Guest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _GuestService.UpdateAsync(id, model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _GuestService.DeleteAsync(id);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }


        [HttpGet]
        [ProducesResponseType(typeof(List<Guest>), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> List([FromQuery]GetListViewModel<GuestFilter> listModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _GuestService.ListAsync(listModel);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(result);
            
        }
        
        [HttpGet("count")]
        [ProducesResponseType(typeof(int), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Count(GuestFilter filter)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _GuestService.CountAsync(filter);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
            
        }
    }
}