using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling;
using Application.DataObjectHandling.Terms;
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

        //should go to: UserTerm/createUserTerm
        [Authorize]
        [HttpPost("createUserTerm")]
        public async Task<IActionResult> CreateUserTerm(UserTermCreateDto dto)
        {
            var tDto =  TermConverter.TermDtoFor(dto);
            var result = HandleResult(await Mediator.Send(new TermCreate.Command
            {TermCreateDto = tDto}));
            return HandleResult(await Mediator.Send(new UserTermCreate.Command
            {termCreateDto = dto}));
        }

        [Authorize]
        [HttpPost("createTerm")]
        public async Task<IActionResult> CreateTerm(TermDto dto)
        {
            return HandleResult(await Mediator.Send(new TermCreate.Command
            {TermCreateDto = dto}));
        }

        [Authorize]
        [HttpGet("getUserTerm")]

        //should go to: /UserTerm/getUserTerm
        public async Task<IActionResult> GetUserTerm(TermDto dto)
        {
            return HandleResult(await Mediator.Send(new UserTermDetails.Query
            {
                TermDto = dto
            }));
        }

        //should go to: /Term/popularTranslationsFor
        [Authorize]
        [HttpGet("popularTranslationsFor")]
        public async Task<IActionResult> PopularTranslationsFor(TranslationDto dto)
        {
            return HandleResult(await Mediator.Send(new PopularTranslationsList.Query
            {
                TranslationDto = dto
            }));
        }

        //should go to: /UserTerm/addTranslation
        [Authorize]
        [HttpPost("addTranslation")]
        public async Task<IActionResult> AddTranslation(AddTranslationDto dto)
        {
            return HandleResult(await Mediator.Send(new UserTermAddTranslation.Command{AddTranslationDto = dto}));
        }

        [Authorize]
        [HttpGet("getTranslations")]

        public async Task<IActionResult> GetTranslations(GetTranslationsDto dto)
        {
            return HandleResult(await Mediator.Send(new UserTermListTranslations.Query{GetTranslationsDto = dto}));
        }
    }
}