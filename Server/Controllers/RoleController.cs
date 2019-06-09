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
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger<RoleController> _logger;
        
        public RoleController(UserManager<ApplicationUser> userManager,
                              RoleManager<ApplicationRole> roleManager,
                              IRoleService roleService,
                              ILogger<RoleController> logger) 
            : base(userManager)
        {
            this._roleService = roleService;
            this._roleManager = roleManager;
            this._logger = logger;
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

            var role = await _roleManager.FindByIdAsync(model.Id.ToString());

            if (role != null && role.Id != model.Id)
                return BadRequest("Error ya existe un rol con este Id");          

            if (role == null)
                return BadRequest("No se encuentra el rol");   

            role.Name = model.Name; 

            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveOrRestore(RoleIdViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); 
            
            var role = await _roleManager.FindByIdAsync(model.Id.ToString());     

            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
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

    
        [HttpGet]
        [ProducesResponseType(typeof(List<RoleViewModel>), 200)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> List(RoleFilterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _roleService.GetListAsync(model.sortOrder, model.searchString, model.pageIndex, model.pageSize);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            var countItems = await _roleService.CountAsync(model.searchString);

            return Ok(result);
            
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(int), 200)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Count()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _roleService.CountAsync("");
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            var countItems = await _roleService.CountAsync("");
            
            return Ok(result);
            
        } 
    }
}