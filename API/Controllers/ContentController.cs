using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.Contents;
using Application.DataObjectHandling.Transcripts;
using Application.DomainDTOs;
using Application.DomainDTOs.Content;
using Application.DomainDTOs.ContentViewRecord;
using Microsoft.AspNetCore.Mvc;
using static Application.DataObjectHandling.Contents.EnsureContentTerms;
using static Application.DataObjectHandling.Contents.GetChunksForContent;
using static Application.DataObjectHandling.Contents.GetContentHeader;
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

        [HttpPost("getChunksForContent")]
        public async Task<IActionResult> GetChunksForContent(ContentIdDto dto)
        {
            return HandleResult(await Mediator.Send(new GetChunksForContent.Command{Dto = dto}));
        }
        
        [HttpPost("getChunkIdsForContent")]
        public async Task<IActionResult> GetChunksIdsForContent(ContentIdDto dto)
        {
            return HandleResult(await Mediator.Send(new GetChunkIdsForContent.Query{Dto = dto}));
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
    }
}