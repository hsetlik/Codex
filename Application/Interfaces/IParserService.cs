using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.Terms;
using Application.DomainDTOs;

namespace Application.Interfaces
{
    public interface IParserService
    {
        public Task PrepareForContent(string url);

        public Task<int> GetNumParagraphs(string url);

        public Task<List<AbstractTermDto>> AbstractTermsForParagraph(string contentUrl, int paragraphIndex);
        public Task<ContentMetadataDto> GetContentMetadata(string url);
    }
}