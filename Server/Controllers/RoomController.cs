using System;
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
    [Route("api/rooms")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<string>), 400)]
    public class RoomController : BaseController
    {
        private readonly IRoomService _RoomService;
        private readonly ILogger<RoomController> _logger;
        
        public RoomController(UserManager<ApplicationUser> userManager, 
                              IRoomService RoomService,
                              ILogger<RoomController> logger)
            : base(userManager)
        {
            this._RoomService = RoomService;
            this._logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody]Room model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _RoomService.CreateAsync(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Room), 200)]
        [Authorize(Roles = "Admin, Manager")]
        [AllowAnonymous]
        public async Task<IActionResult> Retrieve([FromRoute] long id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _RoomService.RetrieveAsync(id);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        } 

        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Update([FromRoute] long id, [FromBody]Room model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _RoomService.UpdateAsync(id, model);
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

            var result = await _RoomService.DeleteAsync(id);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }


        [HttpGet]
        [ProducesResponseType(typeof(List<Room>), 200)]
        [Authorize(Roles = "Admin, Manager")]
        [AllowAnonymous]
        public async Task<IActionResult> List([FromQuery]GetListViewModel<RoomFilter> listModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _RoomService.ListAsync(listModel);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(result);
            
        }
        
        [HttpGet("count")]
        [ProducesResponseType(typeof(int), 200)]
        [Authorize(Roles = "Admin, Manager")]
        [AllowAnonymous]
        public async Task<IActionResult> Count(RoomFilter filter)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _RoomService.CountAsync(filter);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
            
        }

        [HttpGet("free")]
        [ProducesResponseType(typeof(List<FreeRoom>), 200)]
        [Authorize(Roles = "Admin, Manager")]
        [AllowAnonymous]
        public async Task<IActionResult> List([FromQuery]DateTime initialDate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _RoomService.ListFreeRoomsAsync(initialDate);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(result);
        }
    }
}