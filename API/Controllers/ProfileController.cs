using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.ProfileHistory;
using Application.DataObjectHandling.UserLanguageProfiles;
using Application.DataObjectHandling.UserTerms;
using Application.DomainDTOs;
using Application.DomainDTOs.ProfileHistory;
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

        [Authorize]
        [HttpPost("deleteProfile")]
        public async Task<IActionResult> DeleteProfile(LanguageNameDto dto)
        {
            return HandleResult(await Mediator.Send(new UserLanguageProfileDelete.Command{Dto = dto}));
        } 

        [Authorize]
        [HttpPost("allUserTerms")]
        public async Task<IActionResult> AllUserTerms(LanguageNameDto dto)
        {
            return HandleResult(await Mediator.Send(new AllUserTerms.Query{Dto = dto}));
        }


        [Authorize]
        [HttpPost("userTermsFromDate")]
        public async Task<IActionResult> UserTermsFromDate(LanguageDateQuery dto)
        {
            return HandleResult(await Mediator.Send(new GetUserTermsFromDate.Query{Dto = dto}));
        }

        [Authorize]
        [HttpPost("getMetricGraph")]
        public async Task<IActionResult> GetMetricGraph(MetricGraphQuery dto)
        {
            return HandleResult(await Mediator.Send(new GetMetricGraph.Query{Dto = dto}));
        }

        [Authorize]
        [HttpPost("updateHistory")]
        public async Task<IActionResult> UpdateHistory(ProfileIdDto dto)
        {
            return HandleResult(await Mediator.Send(new UpdateHistory.Command{Dto = dto}));
        }
    }
}