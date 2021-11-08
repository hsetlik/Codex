using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.UserLanguageProfiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : CodexControllerBase
    {
        [HttpPost("createProfile")]
        public async Task<IActionResult> CreateProfile(UserLanguageProfileDto profileDto)
        {
            return HandleResult(await Mediator.Send(new UserLanguageProfileCreate.Command{LanguageId = profileDto.Language}));
        }

        [Authorize]
        [HttpGet("getUserProfiles")]
        public async Task<IActionResult> GetProfiles()
        {
            return HandleResult(await Mediator.Send(new UserLanguageProfileList.Query()));
        }
    }
}