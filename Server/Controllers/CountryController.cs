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
    public class CountryController : BaseController
    {
        private readonly ICountryService _countryService;
        private readonly ILogger<CountryController> _logger;
        
        public CountryController(UserManager<ApplicationUser> userManager, 
                              ICountryService countryService,
                              ILogger<CountryController> logger) 
            : base(userManager)
        {
            this._countryService = countryService;
            this._logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody]CountryViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _countryService.CreateAsync(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody]CountryViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _countryService.UpdateAsync(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveOrRestore([FromBody]CountryIdViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _countryService.RemoveOrRestoreAsync(model.Id);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<CountryViewModel>), 200)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> List(CountryFilterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _countryService.GetListAsync(model.sortOrder, model.searchString, model.pageIndex, model.pageSize);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            var countItems = await _countryService.CountAsync(model.searchString);

            return Ok(result);
            
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(int), 200)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Count()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _countryService.CountAsync("");
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            var countItems = await _countryService.CountAsync("");
            
            return Ok(result);
            
        }

        [HttpGet]
        [ProducesResponseType(typeof(CountryViewModel), 200)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(CountryIdViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _countryService.GetByIdAsync(model.Id);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        } 
    }
}