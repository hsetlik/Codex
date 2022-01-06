using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.Phrase;
using Application.DomainDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhraseController : CodexControllerBase
    {
        [Authorize]
        [HttpPost("createPhrase")]
        public async Task<IActionResult> CreatePhrase(PhraseCreateQuery dto)
        {
            return HandleResult(await Mediator.Send(new CreatePhrase.Command{Dto = dto}));
        }

        [Authorize]
        [HttpPost("getPhraseDetails")]
        public async Task<IActionResult> GetPhraseDetails(PhraseQuery dto)
        {
            return HandleResult(await Mediator.Send(new GetPhraseDetails.Query{Dto = dto}));
        }

        [Authorize]
        [HttpPost("deletePhrase")]
        public async Task<IActionResult> DeletePhrase(PhraseIdQuery dto)
        {
            return HandleResult(await Mediator.Send(new DeletePhrase.Command{Dto = dto}));
        }

        [Authorize]
        [HttpPost("updatePhrase")]
        public async Task<IActionResult> UpdatePhrase(PhraseDto dto)
        {
            return HandleResult(await Mediator.Send(new UpdatePhrase.Command{Dto = dto}));
        }
        


        
    }
}