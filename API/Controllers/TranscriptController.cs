using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.Transcripts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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