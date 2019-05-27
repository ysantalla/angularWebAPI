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
    [Route("api/countries")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<string>), 400)]
    public class CountryController : BaseController
    {
        private readonly ICountryService _CountryService;
        private readonly ILogger<CountryController> _logger;
        
        public CountryController(UserManager<ApplicationUser> userManager, 
                              ICountryService CountryService,
                              ILogger<CountryController> logger) 
            : base(userManager)
        {
            this._CountryService = CountryService;
            this._logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody]Country model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _CountryService.CreateAsync(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Country), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Retrieve([FromRoute] long id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _CountryService.RetrieveAsync(id);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        } 

        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Update([FromRoute] long id, [FromBody]Country model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _CountryService.UpdateAsync(id, model);
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

            var result = await _CountryService.DeleteAsync(id);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }


        [HttpGet]
        [ProducesResponseType(typeof(List<Country>), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> List([FromQuery]GetListViewModel<CountryFilter> listModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _CountryService.ListAsync(listModel);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(result);
            
        }
        
        [HttpGet("count")]
        [ProducesResponseType(typeof(int), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Count(CountryFilter filter)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _CountryService.CountAsync(filter);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
            
        }
    }
}