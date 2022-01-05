using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.Collections;
using Application.DomainDTOs.Collection.Queries;
using Application.DomainDTOs.Collection.Responses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CollectionController : CodexControllerBase
    {
        [HttpPost("createCollection")]
        public async Task<IActionResult> CreateCollection(CreateCollectionQuery dto)
        {
            return HandleResult(await Mediator.Send(new CreateCollection.Command{Dto = dto}));
        }

        [HttpPost("deleteCollection")]
        public async Task<IActionResult> DeleteCollection(CollectionIdQuery dto)
        {
            return HandleResult(await Mediator.Send(new DeleteCollection.Command{Dto = dto}));
        }

        [HttpPost("getCollection")]
        public async Task<IActionResult> GetCollection(CollectionIdQuery dto)
        {
            return HandleResult(await Mediator.Send(new GetCollection.Query{Dto = dto}));
        }

        [HttpPost("updateCollection")]
        public async Task<IActionResult> UpdateCollection(CollectionDto dto)
        {
            return HandleResult(await Mediator.Send(new UpdateCollection.Command{Dto = dto}));
        }

        [HttpPost("collectionsForLanguage")]
        public async Task<IActionResult> CollectionsForLanguage(CollectionsForLanguageQuery dto)
        {
            return HandleResult(await Mediator.Send(new CollectionsForLanguage.Query{Dto = dto}));
        }




 
    }
}