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
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IProfileService _profileService;
        
        public AccountController(UserManager<ApplicationUser> userManager,
                                IProfileService profileService,
                                ITokenService tokenService)
            : base (userManager)
        {
            this._userManager = userManager;
            this._tokenService = tokenService;
            this._profileService = profileService;
        }


        // TODO: NLog thinks that |Authorization was successful for user: (null)|
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> SignIn([FromBody] SignInViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("Correo electrónico no encontrado");

            var result = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!result)
                return BadRequest("Contraseña incorrecta");
            
            var roles = await _userManager.GetRolesAsync(user);
            
            return Ok(_tokenService.Generate(user, roles.ToList()));
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> SignUp([FromBody] SignUpViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newUser = new ApplicationUser 
            {
                UserName = model.Username,
                Email = model.Email,
                Firstname = model.Firstname,
                Lastname = model.Lastname             
            };            

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            ApplicationUser user = await _userManager.FindByEmailAsync(newUser.Email);
            var roles = await _userManager.GetRolesAsync(user);
            
            return Ok(_tokenService.Generate(user, roles.ToList()));
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ProfileViewModel), 200)]
        public async Task<IActionResult> Profile()
        {

            var result = await _profileService.GetProfileAsync();
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(result.Value);
        }

    }
}