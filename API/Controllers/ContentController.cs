using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.ContentRecords;
using Application.DataObjectHandling.Contents;
using Application.DomainDTOs;
using Application.DomainDTOs.Content;
using Application.DomainDTOs.Content.Queries;
using Application.DomainDTOs.UserLanguageProfile;
using Application.DomainDTOs.UserTerm.Queries;
using Application.Parsing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContentController : CodexControllerBase
    {
        
        [HttpPost("getLanguageContents")]
        public async Task<IActionResult> GetLanguageContents(LanguageNameQuery dto)
        {
            return HandleResult(await Mediator.Send(new GetLanguageContents.Query{Dto = dto}));
        }

        [HttpPost("addContentTag")]
        public async Task<IActionResult> AddContentTag(ContentTagDto dto)
        {
            return HandleResult(await Mediator.Send(new AddContentTag.Command{Dto = dto}));
        }

        [HttpPost("getContentTags")]
        public async Task<IActionResult> GetContentTags(ContentIdQuery dto)
        {
            return HandleResult(await Mediator.Send(new GetContentTags.Query{Dto = dto}));
        }

        [HttpPost("getContentsWithTag")]
        public async Task<IActionResult> GetContentsWithTag(ContentTagQuery dto)
        {
            return HandleResult(await Mediator.Send(new GetContentsWithTag.Command{Dto = dto}));
        }


        [HttpPost("importContent")]
        public async Task<IActionResult> ImportContent(CreateContentQuery dto)
        {
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
        public async Task<IActionResult> GetContentWithId(ContentIdQuery dto)
        {
            return HandleResult(await Mediator.Send(new GetContentWithId.Query{Dto = dto}));
        }

        [Authorize]
        [HttpPost("getKnownWordsForContent")]
        public async Task<IActionResult> GetKnownWordsForContent(ContentIdQuery dto)
        {
            return HandleResult(await Mediator.Send(new GetKnownWordsForContent.Query{ContentId = dto.ContentId}));
        }

        [Authorize]
        [HttpPost("deleteContent")]
        public async Task<IActionResult> DeleteContent(ContentUrlQuery dto)
        {
            return HandleResult(await Mediator.Send(new DeleteContent.Command{Dto = dto}));
        }
        [Authorize]
        [HttpPost("getBookmark")]
        public async Task<IActionResult> GetBookmark(ContentUrlQuery dto)
        {
            return HandleResult(await Mediator.Send(new GetBookmark.Query{Dto = dto}));
        }
        [Authorize]
        [HttpPost("viewContent")]
        public async Task<IActionResult> ViewContent(SectionQuery dto)
        {
            return HandleResult(await Mediator.Send(new ViewContent.Command{Dto = dto}));
        }

        [Authorize]
        [HttpPost("abstractTermsForElement")]
        public async Task<IActionResult> AbstractTermsForElement(ElementAbstractTermsQuery dto)
        {
            return HandleResult(await Mediator.Send(new AbstractTermsForElement.Query{Dto = dto}));
        }

        //TODO: get rid of this
        [Authorize]
        [HttpPost("getSectionMetadata")]
        public async Task<IActionResult> GetSectionMetadata(SectionQuery dto)
        {
            return HandleResult(await Mediator.Send(new GetSectionMetadata.Query{Dto = dto}));
        }

        // also get rid of this
        [Authorize]
        [HttpPost("getSectionAtMs")]
        public async Task<IActionResult> GetSectionAtMs(SectionAtMsQuery dto)
        {
            return HandleResult(await Mediator.Send(new GetSectionAtMs.Query{Dto = dto}));
        }

        [Authorize]
        [HttpPost("saveContent")]
        public async Task<IActionResult> SaveContent(SaveContentQuery dto)
        {
            return HandleResult(await Mediator.Send(new SaveContent.Command{Dto = dto}));
        }

        [Authorize]
        [HttpPost("unsaveContent")]
        public async Task<IActionResult> UnsaveContent(SaveContentQuery dto)
        {
            return HandleResult(await Mediator.Send(new UnsaveContent.Command{Dto = dto}));
        }

        [Authorize]
        [HttpPost("getSavedContents")]
        public async Task<IActionResult> GetSavedContents(ProfileIdQuery dto)
        {
            return HandleResult(await Mediator.Send(new GetSavedContents.Query{Dto = dto}));
        }


    }
}