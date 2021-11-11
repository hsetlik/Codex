using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling;
using Application.DataObjectHandling.Terms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TermController : CodexControllerBase
    {
        [Authorize]
        [HttpPost("createTerm")]
        public async Task<IActionResult> CreateTerm(TermDto dto)
        {
            return HandleResult(await Mediator.Send(new TermCreate.Command
            {TermCreateDto = dto}));
        }

          //should go to: /Term/popularTranslationsFor
        [Authorize]
        [HttpGet("popularTranslationsFor")]
        public async Task<IActionResult> PopularTranslationsFor(TermDto dto)
        {
            return HandleResult(await Mediator.Send(new PopularTranslationsList.Query
            {
                Dto = dto
            }));
        }

        [Authorize]
        [HttpGet("getAbstractTerm")]
        public async Task<IActionResult> GetAbstractTerm(TermDto dto)
        {
            return HandleResult(await Mediator.Send(new GetAbstractTerm.Query{Dto = dto}));
        }
    } 
}