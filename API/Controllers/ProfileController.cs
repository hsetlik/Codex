using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.UserLanguageProfiles;
using Application.DataObjectHandling.UserTerms;
using Application.DomainDTOs;
using Application.DomainDTOs.UserLanguageProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : CodexControllerBase
    {
        [HttpPost("createProfile")]
        public async Task<IActionResult> CreateProfile(LanguageNameDto profileDto)
        {
            return HandleResult(await Mediator.Send(new UserLanguageProfileCreate.Command{LanguageId = profileDto.Language}));
        }


        [Authorize]
        [HttpPost("getDueNow")]
        public async Task<IActionResult> GetDueNow(LanguageNameDto profileDto)
        {
            return HandleResult(await Mediator.Send(new GetDueNow.Command{Dto = profileDto}));
        }

        [Authorize]
        [HttpGet("getUserProfiles")]
        public async Task<IActionResult> GetProfiles()
        {
            return HandleResult(await Mediator.Send(new UserLanguageProfileList.Query()));
        }

        [Authorize]
        [HttpPost("getProfileDetails")]
        public async Task<IActionResult> GetProfileDetails(ProfileIdDto dto)
        {
            return HandleResult(await Mediator.Send(new UserLanguageProfileDetails.Query{Dto = dto}));
        }
    }
}