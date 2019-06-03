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
    [Route("api/reservations")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<string>), 400)]
    public class ReservationController : BaseController
    {
        private readonly IReservationService _ReservationService;
        private readonly ILogger<ReservationController> _logger;
        
        public ReservationController(UserManager<ApplicationUser> userManager, 
                              IReservationService ReservationService,
                              ILogger<ReservationController> logger) 
            : base(userManager)
        {
            this._ReservationService = ReservationService;
            this._logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody]Reservation model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _ReservationService.CreateAsync(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Reservation), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Retrieve([FromRoute] long id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _ReservationService.RetrieveAsync(id);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        } 

        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Update([FromRoute] long id, [FromBody]Reservation model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _ReservationService.UpdateAsync(id, model);
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

            var result = await _ReservationService.DeleteAsync(id);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }


        [HttpGet]
        [ProducesResponseType(typeof(List<Reservation>), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> List([FromQuery]GetListViewModel<ReservationFilter> listModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _ReservationService.ListAsync(listModel);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(result);
            
        }
        
        [HttpGet("count")]
        [ProducesResponseType(typeof(int), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Count(ReservationFilter filter)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _ReservationService.CountAsync(filter);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
            
        }

        [HttpPut("{id}/guests/{guestId}")]
        [ProducesResponseType(typeof(int), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> AddGuest([FromRoute]long id, [FromRoute]long guestId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _ReservationService.AddGuestAsync(id, guestId);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }

        [HttpDelete("{id}/guests/{guestId}")]
        [ProducesResponseType(typeof(int), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> RemoveGuest([FromRoute]long id, [FromRoute]long guestId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _ReservationService.RemoveGuestAsync(id, guestId);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }
    }
}