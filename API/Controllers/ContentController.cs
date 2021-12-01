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
using static Application.DataObjectHandling.Contents.GetContentHeader;

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

        [HttpGet("getContentHeader")]
        public async Task<IActionResult> GetContentHeader(GetContentHeaderDto dto)
        {
            return HandleResult(await Mediator.Send(new GetContentHeader.Query{Dto = dto}));
        }

        [HttpPost("getLanguageContents")]
        public async Task<IActionResult> GetLanguageContents(LanguageNameDto dto)
        {
            return HandleResult(await Mediator.Send(new GetLanguageContents.Query{Dto = dto}));
        }

        [HttpPost("recordContentView")]
        public async Task<IActionResult> RecordContentView(ContentIdDto Dto)
        {
            return HandleResult(await Mediator.Send(new RecordContentView.Command{ContentId = Dto.ContentId}));
        }  

        [HttpPost("getContentViewRecords")]
        public async Task<IActionResult> GetContentViewRecords(ContentIdDto Dto)
        {
            return HandleResult(await Mediator.Send(new GetContentViewRecords.Query{ContentId = Dto.ContentId}));
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

        [HttpPost("getKnownWordsForContent")]
        public async Task<IActionResult> GetKnownWordsForContent(ContentIdDto dto)
        {
            return HandleResult(await Mediator.Send(new GetKnownWordsForContent.Query{ContentId = dto.ContentId}));
        }

        [HttpPost("parseUrlIntoContent")]
        public async Task<IActionResult> ParseUrlIntoContent(ContentUrlDto dto)
        {
            return HandleResult(await Mediator.Send(new ContentParseRequest.Query{Url = dto.Url}));
        }

        [Authorize]
        [HttpPost("abstractTermsForParagraph")]
        public async Task<IActionResult> AbstractTermsForParagraph(ParagraphQueryDto dto)
        {
            return HandleResult(await Mediator.Send(new AbstractTermsForParagraph.Query{Dto = dto}));
        }
    }
}