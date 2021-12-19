using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.Phrase;
using Application.DomainDTOs.Phrase;
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
        public async Task<IActionResult> CreatePhrase(PhraseCreateDto dto)
        {
            return HandleResult(await Mediator.Send(new CreatePhrase.Command{Dto = dto}));
        }
        
    }
}