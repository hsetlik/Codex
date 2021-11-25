using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using API.Services;
using Domain.DataObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.DTOs;
using Application.DataObjectHandling.UserLanguageProfiles;
using Application.DomainDTOs;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<CodexUser> _userManager;
        private readonly SignInManager<CodexUser> _signInManager;
        private readonly TokenService _tokenService;
        
        //==================================================================
        public AccountController(UserManager<CodexUser> userManager, 
        SignInManager<CodexUser> signInManager, 
        TokenService tokenService)
        {
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        //==================================================================

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            // 1. make sure that the email and username are not already in use
            if(await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
            {
                ModelState.AddModelError("email", "Email is Already in use");
                return ValidationProblem();
            }
            if(await _userManager.Users.AnyAsync(x => x.UserName == registerDto.Username))
            {
                ModelState.AddModelError("username", "Username is taken");
                return ValidationProblem();
            }

            var user = new CodexUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Username,
                NativeLanguage = registerDto.NativeLanguage,
                UserLanguageProfiles = new List<UserLanguageProfile>(),
                LastStudiedLanguage = "none"
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                return CreateUserObject(user);
            }
            return BadRequest("Could not register user");
        }

        //==================================================================
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto login)
        {
            CodexUser user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null)
            {
                return Unauthorized();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);

            if(result.Succeeded)
            {
                return CreateUserObject(user);
            }
            return Unauthorized();
        }

        //==================================================================
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            Console.WriteLine("Getting current user...");
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            return CreateUserObject(user);
        }

        //==================================================================
        [Authorize]
        [HttpPost("setLastStudiedLanguage")]
        public async Task<ActionResult<UserDto>> SetLastStudiedLanguage(LanguageNameDto dto)
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            if (user == null)
            {
                return Unauthorized();
            }
            user.LastStudiedLanguage = dto.Language;
            var success = await _userManager.UpdateAsync(user) == IdentityResult.Success;
            if (!success)
                return BadRequest("User manager could not update");
            return CreateUserObject(user);
        } 
        //==================================================================
        private UserDto CreateUserObject(CodexUser user)
        {
            return new UserDto
            {
                DisplayName = user.DisplayName,
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                LastStudiedLanguage = user.LastStudiedLanguage
            };
        }
    }
}