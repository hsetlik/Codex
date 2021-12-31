using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling;
using Application.DataObjectHandling.Terms;
using Application.DataObjectHandling.UserTerms;
using Application.DomainDTOs;
using Application.DomainDTOs.UserLanguageProfile;
using Domain.DataObjects;
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
        public async Task<IActionResult> CreateUserTerm(UserTermCreateQuery dto)
        {
            return HandleResult(await Mediator.Send(new UserTermCreate.Command{termCreateDto = dto}));
        }

        [Authorize]
        [HttpPost("addTranslation")]
        public async Task<IActionResult> AddTranslation(AddTranslationQuery dto)
        {
            return HandleResult(await Mediator.Send(new UserTermAddTranslation.Command{AddTranslationDto = dto}));
        }

        [Authorize]
        [HttpPost("getTranslations")]
        public async Task<IActionResult> GetTranslations(UserTermIdQuery dto)
        {
            return HandleResult(await Mediator.Send(new UserTermListTranslations.Query{GetTranslationsDto = dto}));
        }

        [Authorize]
        [HttpPost("getUserTerm")]
        public async Task<IActionResult> GetUserTerm(TermDto dto)
        {
            return HandleResult(await Mediator.Send(new UserTermDetails.Query{TermDto = dto}));
        }
       
        [Authorize]
        [HttpPost("deleteUserTerm")]
        public async Task<IActionResult> DeleteUserTerm(TermDto dto)
        {
            return HandleResult(await Mediator.Send(new UserTermDelete.Command{Dto = dto}));
        }

        [Authorize]
        [HttpPost("getStarred")]
        public async Task<IActionResult> GetStarred(ProfileIdQuery dto)
        {
            return HandleResult(await Mediator.Send(new GetStarred.Query{Dto = dto}));
        }
/*
        [Authorize]
        [HttpPost("answerUserTerm")]
        public async Task<IActionResult> AnswerUserTerm(UserTermAnswerDto dto)
        {
            return HandleResult(await Mediator.Send(new AnswerUserTerm.Command{Dto = dto}));
        }
*/

        [Authorize]
        [HttpPost("updateUserTerm")]
        public async Task<IActionResult> UpdateUserTerm(UserTermDetailsDto dto)
        {
            return HandleResult(await Mediator.Send(new UpdateUserTerm.Command{Dto = dto}));
        }

        [Authorize]
        [HttpPost("deleteTranslation")]
        public async Task<IActionResult> DeleteTranslation(ChildTranslationQuery dto)
        {
            return HandleResult(await Mediator.Send(new UserTermDeleteTranslation.Command{Dto = dto}));
        }      
    }
}