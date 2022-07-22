using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.FeedHandling;
using Application.DomainDTOs.Feed.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedController : CodexControllerBase
    {
        [Authorize]
        [HttpPost("getFeed")]
        public async Task<IActionResult> GetFeed(FeedQuery dto)
        {
            return HandleResult(await Mediator.Send(new GetFeed.Query{Dto = dto}));
        }
    }
}