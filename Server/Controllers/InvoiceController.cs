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
    [Route("api/invoices")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<string>), 400)]
    public class InvoiceController : BaseController
    {
        private readonly IInvoiceService _InvoiceService;
        private readonly ILogger<InvoiceController> _logger;
        
        public InvoiceController(UserManager<ApplicationUser> userManager, 
                              IInvoiceService InvoiceService,
                              ILogger<InvoiceController> logger) 
            : base(userManager)
        {
            this._InvoiceService = InvoiceService;
            this._logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody]Invoice model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _InvoiceService.CreateAsync(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Invoice), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Retrieve([FromRoute] long id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _InvoiceService.RetrieveAsync(id);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        } 

        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Update([FromRoute] long id, [FromBody]Invoice model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _InvoiceService.UpdateAsync(id, model);
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

            var result = await _InvoiceService.DeleteAsync(id);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }


        [HttpGet]
        [ProducesResponseType(typeof(List<Invoice>), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> List([FromQuery]GetListViewModel<InvoiceFilter> listModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _InvoiceService.ListAsync(listModel);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(result);
            
        }
        
        [HttpGet("count")]
        [ProducesResponseType(typeof(int), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Count(InvoiceFilter filter)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _InvoiceService.CountAsync(filter);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
            
        }
    }
}