using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.Contents;
using Application.DataObjectHandling.Transcripts;
using Microsoft.AspNetCore.Mvc;
using static Application.DataObjectHandling.Contents.EnsureContentTerms;
using static Application.DataObjectHandling.Transcripts.ContentCreate;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContentController : CodexControllerBase
    {
        [HttpPost("createContent")]
        public async Task<IActionResult> CreateContent(ContentCreateDto Dto)
        {
            return HandleResult(await Mediator.Send(new ContentCreate.Command{Dto = Dto}));
        }

        [HttpPost("ensureContentTerms")]
        public async Task<IActionResult> EnsureContentTerms(EnsureTermsDto Dto)
        {
            return HandleResult(await Mediator.Send(new EnsureContentTerms.Command{Dto = Dto}));
        }
    }
}