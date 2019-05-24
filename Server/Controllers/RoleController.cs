using Server.Models;
using Server.ViewModels;
using Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<string>), 400)]
    public class RoleController : BaseController
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
                
        public RoleController(UserManager<ApplicationUser> userManager,
                                RoleManager<ApplicationRole> roleManager
                                )
            : base (userManager)
        {
            this._roleManager = roleManager;
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody]RoleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);   

            var newRole = new ApplicationRole 
            {
                Name = model.Name           
            };    

            var role = await _roleManager.FindByNameAsync(model.Name);
            if (role != null)
                return BadRequest("Error ya existe un rol con este nombre");

            var result = await _roleManager.CreateAsync(newRole);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }

        [HttpPatch]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody]RoleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);  

            var role = await _roleManager.FindByNameAsync(model.Name);

            if (role != null && role.Id != model.Id)
                return BadRequest("Error ya existe un rol con este nombre");          

            var newRole = new ApplicationRole 
            {
                Name = model.Name           
            };       

            var result = await _roleManager.UpdateAsync(newRole);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveOrRestore([FromBody]RoleIdViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); 
            
            var roleId = new ApplicationRole 
            {
                Id = model.Id           
            };                       

            var result = await _roleManager.DeleteAsync(roleId);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<RoleViewModel>), 200)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> List(RoleFilterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _roleManager.FindByNameAsync(model.searchString);
            return Ok(result);           

        }

        [HttpGet]
        [ProducesResponseType(typeof(RoleViewModel), 200)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(RoleIdViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);   

            var result = await _roleManager.FindByIdAsync(model.Id.ToString());
            
            return Ok(result);
        }

    }
}