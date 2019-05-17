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
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> CreateCountry([FromBody]CountryViewModel model)
        {
            var result = await _countryService.CreateOrUpdateAsync(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }

        
        
    }
}