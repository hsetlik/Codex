using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling;
using Application.DataObjectHandling.Terms;
using Application.DataObjectHandling.UserTerms;
using Application.DomainDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserTermController : CodexControllerBase
    {
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

        [Authorize]
        [HttpGet("getUserTerm")]
        public async Task<IActionResult> GetUserTerm(TermDto dto)
        {
            return HandleResult(await Mediator.Send(new UserTermDetails.Query
            {
                TermDto = dto
            }));
        }

        [Authorize]
        [HttpPost("answerUserTerm")]
        public async Task<IActionResult> AnswerUserTerm(UserTermAnswerDto dto)
        {
            return HandleResult(await Mediator.Send(new AnswerUserTerm.Command{Dto = dto}));
        }
    }
}