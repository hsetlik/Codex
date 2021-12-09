using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.Terms;
using Application.DomainDTOs;
using Application.DomainDTOs.Content;
using Application.Interfaces;

namespace Application.Parsing
{
    public class ParserService : IParserService
    {
        
        private AbstractScraper scraper = null;


        public string CurrentUrl { get; private set; }
        public ParserService()
        {
            
            
        }
        private async Task EnsureLoaded(string url)
        {
            if (scraper == null)
                scraper = ContentScraperFactory.ScraperFor(url);
            if (!scraper.ContentsLoaded || scraper.Url != url)
                await scraper.PrepareAsync();
        }

        public async Task<int> GetNumSections(string url)
        {
            await EnsureLoaded(url);
            return scraper.GetNumSections();
        }

        public async Task<ContentSection> GetSection(string contentUrl, int index)
        {
           await EnsureLoaded(contentUrl);
           return scraper.GetSection(index);
        }

        public async Task<ContentMetadataDto> GetContentMetadata(string url)
        {
            await EnsureLoaded(url);
            return scraper.GetMetadata();
        }
    }
}