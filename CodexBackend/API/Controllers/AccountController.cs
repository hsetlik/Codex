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
using Application.DTOs;
using Application.DataObjectHandling.Account;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : CodexControllerBase
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

            var profile =  new UserLanguageProfile
            {
                Language = registerDto.StudyLanguage,
                User = user,
                KnownWords = 0,
                UserLanguage = user.NativeLanguage 
            };

            user.UserLanguageProfiles.Add(profile);

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
            Console.WriteLine($"Logging in: {login.Email}, {login.Password}");
            CodexUser user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null)
            {
                return Unauthorized();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);

            if(result.Succeeded)
            {
                Console.WriteLine($"Login succeded");
                return CreateUserObject(user);
            }
            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("getUsernameAvailable")]
        public async Task<ActionResult<UsernameAvailableDto>> GetUsernameAvailable(UsernameDto dto)
        {
            return HandleResult(await Mediator.Send(new GetUsernameAvailable.Query{Dto = dto}));
        }

        //==================================================================
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            return CreateUserObject(user);
        }

        //==================================================================
        [Authorize]
        [HttpPost("setLastStudiedLanguage")]
        public async Task<ActionResult<UserDto>> SetLastStudiedLanguage(LanguageNameQuery dto)
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
                LastStudiedLanguage = user.LastStudiedLanguage,
                NativeLanguage = user.NativeLanguage
            };
        }
    }
}