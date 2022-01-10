using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.Contents;
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
        public async Task<IActionResult> GetContentMetadata(ContentUrlQuery dto)
        {
            return HandleResult(await Mediator.Send(new GetContentMetadata.Query{Dto = dto}));
        }

        [HttpPost("getSection")]
        public async Task<IActionResult> GetSection(SectionQuery dto)
        {
            return HandleResult(await Mediator.Send(new GetSection.Query{Dto = dto}));
        }        

        [HttpGet("getRawHtml/{id}")]
        public async Task<IActionResult> GetRawHtml(Guid id)
        {
            return HandleResult(await Mediator.Send(new GetContentHtml.Query{Dto = new ContentIdQuery{ContentId = id}}));
        } 
    }
}