using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.UserLanguageProfiles;
using Application.DataObjectHandling.UserTerms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LanguageController : CodexControllerBase
    {
        [HttpPost("createProfile")]
        public async Task<IActionResult> CreateProfile(UserLanguageProfileDto profileDto)
        {
            return HandleResult(await Mediator.Send(new UserLanguageProfileCreate.Command{LanguageId = profileDto.Language}));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetProfiles()
        {
            return HandleResult(await Mediator.Send(new UserLanguageProfileList.Query()));
        }

        [Authorize]
        [HttpPost("createUserTerm")]
        public async Task<IActionResult> CreateUserTerm(TermCreateDto termCreateDto)
        {
            Console.WriteLine(termCreateDto.TermId);
            Console.WriteLine(termCreateDto.FirstTranslation);
            Console.WriteLine(termCreateDto.UserTermId);

            return HandleResult(await Mediator.Send(new UserTermCreate.Command
            {
                TermId = termCreateDto.TermId,
                FirstTranslation = termCreateDto.FirstTranslation,
                UserTermId = termCreateDto.UserTermId
            }));
        }
    }
}