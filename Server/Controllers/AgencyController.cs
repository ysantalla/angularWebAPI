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
    [Route("api/agencies")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<string>), 400)]
    public class AgencyController : BaseController
    {
        private readonly IAgencyService _AgencyService;
        private readonly ILogger<AgencyController> _logger;
        
        public AgencyController(UserManager<ApplicationUser> userManager, 
                              IAgencyService AgencyService,
                              ILogger<AgencyController> logger) 
            : base(userManager)
        {
            this._AgencyService = AgencyService;
            this._logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody]Agency model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _AgencyService.CreateAsync(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Agency), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Retrieve([FromRoute] long id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _AgencyService.RetrieveAsync(id);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        } 

        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Update([FromRoute] long id, [FromBody]Agency model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _AgencyService.UpdateAsync(id, model);
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

            var result = await _AgencyService.DeleteAsync(id);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }


        [HttpGet]
        [ProducesResponseType(typeof(List<Agency>), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> List([FromQuery]GetListViewModel<AgencyFilter> listModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _AgencyService.ListAsync(listModel);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(result);
            
        }
        
        [HttpGet("count")]
        [ProducesResponseType(typeof(int), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Count(AgencyFilter filter)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _AgencyService.CountAsync(filter);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
            
        }
    }
}