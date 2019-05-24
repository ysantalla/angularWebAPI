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
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ILogger<RoleController> _logger;

        private readonly UserManager<ApplicationUser> _userManager;
        
        public UserController(UserManager<ApplicationUser> userManager,
                              IUserService userService,
                              ILogger<RoleController> logger) 
            : base(userManager)
        {
            this._userService = userService;
            this._userManager = userManager;
            this._logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody]UserViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);   

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
                return BadRequest("Error ya existe un usuario con este correo electrónico");

            var newUser = new ApplicationUser 
            {
                Email = model.Email,
                UserName = model.Email,
                Firstname = model.Firstname,
                Lastname = model.Lastname             
            };            

            var result = await _userManager.CreateAsync(newUser, model.Password);  

            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }

        [HttpPatch]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody]UserViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);  

            var user = await _userManager.FindByIdAsync(model.Id.ToString());

            if (user != null && user.Id != model.Id)
                return BadRequest("Error ya existe un usuario con este Id");          

            if (user == null)
                return BadRequest("No se encuentra el rol");   

            user.Email = model.Email;
            user.Firstname = model.Firstname;
            user.Lastname = model.Lastname;             

            if (model.Password != null) {
                var newPassword = _userManager.PasswordHasher.HashPassword(user, model.Password);
                user.PasswordHash = newPassword;
            }            

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveOrRestore(UserIdViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); 
            
            var user = await _userManager.FindByIdAsync(model.Id.ToString());     

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(RoleViewModel), 200)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(UserIdViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);   

            var result = await _userManager.FindByIdAsync(model.Id.ToString());
            
            return Ok(result);
        }

    
        [HttpGet]
        [ProducesResponseType(typeof(List<RoleViewModel>), 200)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> List(RoleFilterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _userService.GetListAsync(model.sortOrder, model.searchString, model.pageIndex, model.pageSize);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            var countItems = await _userService.CountAsync(model.searchString);

            return Ok(result);
            
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(int), 200)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Count()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _userService.CountAsync("");
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            var countItems = await _userService.CountAsync("");
            
            return Ok(result);
            
        } 
    }
}