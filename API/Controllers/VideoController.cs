using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.Video;
using Application.DomainDTOs.Video;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideoController : CodexControllerBase
    {
        [HttpPost("getCaptions")]
        public async Task<IActionResult> GetCaptions(CaptionsQuery dto)
        {
            return HandleResult(await Mediator.Send(new GetCaptions.Query{Dto = dto}));
        }

    }
}