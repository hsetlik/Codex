using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.ContentCollections;
using Application.DomainDTOs;
using Application.DomainDTOs.ContentCollection.Queries;
using Application.DomainDTOs.ContentCollection.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContentCollectionController : CodexControllerBase
    {
        [Authorize]
        [HttpPost("createContentCollection")]
        public async Task<IActionResult> CreateContentCollection(CreateCollectionDto dto)
        {
            return HandleResult(await Mediator.Send(new CreateContentCollection.Command{Dto = dto}));
        }

        [Authorize]
        [HttpPost("getContentCollection")]
        public async Task<IActionResult> GetContentCollection(CollectionIdQuery dto)
        {
            return HandleResult(await Mediator.Send(new GetContentCollection.Query{Dto = dto}));
        }
        
        [Authorize]
        [HttpPost("updateContentCollection")]
        public async Task<IActionResult> UpdateContentCollection(ContentCollectionDto dto)
        {
            return HandleResult(await Mediator.Send(new UpdateContentCollection.Command{Dto = dto}));
        }

        [Authorize]
        [HttpPost("deleteContentCollection")]
        public async Task<IActionResult> DeleteContentCollection(CollectionIdQuery dto)
        {
            Console.WriteLine($"Delete controller called for: {dto.ContentCollectionId}");
            return HandleResult(await Mediator.Send(new DeleteContentCollection.Command{Dto = dto}));
        }
    }
}