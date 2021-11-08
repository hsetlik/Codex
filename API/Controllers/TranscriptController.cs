using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.DataObjectHandling.Transcripts;

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
    }
}