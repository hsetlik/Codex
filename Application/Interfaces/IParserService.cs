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
        public Task<int> GetNumSections(string url);
        public Task<ContentSection> GetSection(string contentUrl, int index);
        public Task<ContentMetadataDto> GetContentMetadata(string url);
        public Task<List<ContentSection>> GetAllSections(string contentUrl);
        public Task<AbstractScraper> GetScraper(string url);
    }
}