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
        public async Task<IActionResult> GetUserTerm(TermDto dto)
        {
            return HandleResult(await Mediator.Send(new UserTermDetails.Query
            {
                TermDto = dto
            }));
        }
    }
}