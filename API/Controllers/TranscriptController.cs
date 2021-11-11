using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.DataObjectHandling.Transcripts;
using Application.DataObjectHandling.Terms;
using static Application.DataObjectHandling.Transcripts.GetAbstractTermsForChunk;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TranscriptController : CodexControllerBase
    {
        [Authorize]
        [HttpPost("createTranscript")]
        public async Task<IActionResult> CreateTranscript(CreateTranscriptDto dto)
        {
            return HandleResult(await Mediator.Send(new CreateTranscript.Command{CreateTranscriptDto = dto}));
        }

        [Authorize]
        [HttpPost("getAbstractTermsForChunk")]
        public async Task<IActionResult> GetAbstractTermsForChunk(TranscriptChunkDto dto)
        {
            return HandleResult(await Mediator.Send(new GetAbstractTermsForChunk.Query{Dto = dto}));
        }
    }
}