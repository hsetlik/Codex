using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DomainDTOs.Translator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.DataObjectHandling;
using Application.DataObjectHandling.Translate;
using Application.DataObjectHandling.Terms;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TranslateController : CodexControllerBase
    {
        [Authorize]
        [HttpPost("getTranslation")]
        public async Task<IActionResult> GetTranslation(TranslatorQuery dto)
        {
            Console.WriteLine($"Requesting translation for {dto.QueryValue} in language {dto.QueryLanguage}");
            return HandleResult(await Mediator.Send(new GetTranslation.Query{Dto = dto}));
        }

        [Authorize]
        [HttpPost("getTranslations")]
        public async Task<IActionResult> GetTranslations(TermDto dto)
        {
            return HandleResult(await Mediator.Send(new GetTranslations.Query{Dto = dto}));
        }
        
    }
}