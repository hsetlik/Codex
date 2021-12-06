using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.Contents;
using Application.DomainDTOs;
using Application.DomainDTOs.Content;
using Application.DomainDTOs.ContentViewRecord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContentController : CodexControllerBase
    {
        [HttpPost("ensureContentTerms")]
        public async Task<IActionResult> EnsureContentTerms(ContentIdDto Dto)
        {
            return HandleResult(await Mediator.Send(new EnsureContentTerms.Command{Dto = Dto}));
        }

        [HttpPost("getLanguageContents")]
        public async Task<IActionResult> GetLanguageContents(LanguageNameDto dto)
        {
            return HandleResult(await Mediator.Send(new GetLanguageContents.Query{Dto = dto}));
        }

        [HttpPost("addContentTag")]
        public async Task<IActionResult> AddContentTag(ContentTagDto dto)
        {
            return HandleResult(await Mediator.Send(new AddContentTag.Command{Dto = dto}));
        }

        [HttpPost("getContentTags")]
        public async Task<IActionResult> GetContentTags(ContentIdDto dto)
        {
            return HandleResult(await Mediator.Send(new GetContentTags.Query{Dto = dto}));
        }

        [HttpPost("getContentsWithTag")]
        public async Task<IActionResult> GetContentsWithTag(TagValueDto dto)
        {
            return HandleResult(await Mediator.Send(new GetContentsWithTag.Command{Dto = dto}));
        }


        [Authorize]
        [HttpPost("abstractTermsForParagraph")]
        public async Task<IActionResult> AbstractTermsForParagraph(ParagraphQueryDto dto)
        {
            return HandleResult(await Mediator.Send(new AbstractTermsForParagraph.Query{Dto = dto}));
        }

        [HttpPost("importContent")]
        public async Task<IActionResult> ImportContent(ContentUrlDto dto)
        {
            Console.WriteLine($"Recieved import request for: {dto.ContentUrl}");
            return HandleResult(await Mediator.Send(new ImportContent.Query{Dto = dto}));
        }


        public class ContentNameDto
        {
            public string ContentName { get; set; }
        }

        [Authorize]
        [HttpPost("getContentWithName")]
        public async Task<IActionResult> GetContentWithName(ContentNameDto dto)
        {
            return HandleResult(await Mediator.Send(new GetContentWithName.Query{ContentName= dto.ContentName}));
        }


        [Authorize]
        [HttpPost("getContentWithId")]
        public async Task<IActionResult> GetContentWithId(ContentIdDto dto)
        {
            return HandleResult(await Mediator.Send(new GetContentWithId.Query{Dto = dto}));
        }

        [Authorize]
        [HttpPost("getKnownWordsForContent")]
        public async Task<IActionResult> GetKnownWordsForContent(ContentIdDto dto)
        {
            return HandleResult(await Mediator.Send(new GetKnownWordsForContent.Query{ContentId = dto.ContentId}));
        }



    }
}