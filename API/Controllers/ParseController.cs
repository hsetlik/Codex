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
        public async Task<IActionResult> GetContentMetadata(ContentUrlDto dto)
        {
            return HandleResult(await Mediator.Send(new GetContentMetadata.Query{Dto = dto}));
        }

        [HttpPost("getSection")]
        public async Task<IActionResult> GetSection(SectionQueryDto dto)
        {
            return HandleResult(await Mediator.Send(new GetSection.Query{Dto = dto}));
        }        
    }
}