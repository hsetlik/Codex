using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.Parse;
using Application.DomainDTOs.Content;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParseController : CodexControllerBase
    {
        [HttpPost("getContentMetadata")]
        public async Task<IActionResult> GetContentMetadata(string contentUrl)
        {
            return HandleResult(await Mediator.Send(new GetContentMetadata.Query{Url = contentUrl}));
        }

        [HttpPost("getParagraphCount")]
        public async Task<IActionResult> GetParagraphCount(string contentUrl)
        {
            return HandleResult(await Mediator.Send(new GetParagraphCount.Query{ContentUrl = contentUrl}));
        }

        [HttpPost("getParagraph")]
        public async Task<IActionResult> GetParagraph(ParagraphQueryDto dto)
        {
            return HandleResult(await Mediator.Send(new GetParagraph.Query{Dto = dto}));
        }        
    }
}