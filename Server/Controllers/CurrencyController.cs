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
    public class CurrencyController : BaseController
    {
        private readonly ICurrencyService _CurrencyService;
        private readonly ILogger<CurrencyController> _logger;
        
        public CurrencyController(UserManager<ApplicationUser> userManager, 
                              ICurrencyService CurrencyService,
                              ILogger<CurrencyController> logger) 
            : base(userManager)
        {
            this._CurrencyService = CurrencyService;
            this._logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody]CurrencyViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _CurrencyService.CreateAsync(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }

        [HttpPatch]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Update([FromBody]CurrencyViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _CurrencyService.UpdateAsync(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> RemoveOrRestore([FromBody]CurrencyIdViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _CurrencyService.RemoveOrRestoreAsync(model.Id);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<CurrencyViewModel>), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> List(CurrencyFilterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _CurrencyService.GetListAsync(model.sortOrder, model.searchString, model.pageIndex, model.pageSize);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            // var countItems = await _CurrencyService.CountAsync(model.searchString);

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

            var result = await _CurrencyService.CountAsync("");
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            var countItems = await _CurrencyService.CountAsync("");
            
            return Ok(result);
            
        }

        [HttpGet]
        [ProducesResponseType(typeof(CurrencyViewModel), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(CurrencyIdViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _CurrencyService.GetByIdAsync(model.Id);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        } 
    }
}