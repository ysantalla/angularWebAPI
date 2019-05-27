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
    [Route("api/citizenhips")]
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
        public async Task<IActionResult> Create([FromBody]Citizenship model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _CitizenshipService.CreateAsync(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Citizenship), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Retrieve([FromRoute] long id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _CitizenshipService.RetrieveAsync(id);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        } 

        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Update([FromRoute] long id, [FromBody]Citizenship model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _CitizenshipService.UpdateAsync(id, model);
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

            var result = await _CitizenshipService.DeleteAsync(id);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }


        [HttpGet]
        [ProducesResponseType(typeof(List<Citizenship>), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> List([FromQuery]GetListViewModel<CitizenshipFilter> listModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _CitizenshipService.ListAsync(listModel);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(result);
            
        }
        
        [HttpGet("count")]
        [ProducesResponseType(typeof(int), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Count(CitizenshipFilter filter)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _CitizenshipService.CountAsync(filter);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
            
        }
    }
}