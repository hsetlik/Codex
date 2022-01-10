using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.Terms;
using Application.DomainDTOs;
using Application.Parsing;

namespace Application.Interfaces
{
    public interface IParserService
    {
        Task<ContentSection> GetSection(string contentUrl, int index);
        Task<ContentMetadataDto> GetContentMetadata(string url);
        Task<List<ContentSection>> GetAllSections(string contentUrl);
        Task<AbstractScraper> GetScraper(string url);
        Task<string> GetRawHtml(string url);

    }
}