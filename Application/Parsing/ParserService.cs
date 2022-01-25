using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.Terms;
using Application.DomainDTOs;
using Application.DomainDTOs.Content;
using Application.DomainDTOs.Content.Responses;
using Application.Interfaces;

namespace Application.Parsing
{
    public class ParserService : IParserService
    {
        
        private AbstractScraper scraper = null;


        public string CurrentUrl { get; private set; }

        private async Task EnsureLoaded(string url)
        {
            if (scraper == null || scraper.Url != url)
            {
                scraper = ContentScraperFactory.ScraperFor(url);
            }
            if (!scraper.ContentsLoaded)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                await scraper.PrepareAsync();
                watch.Stop();
            }
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

        public async Task<List<ContentSection>> GetAllSections(string contentUrl)
        {
            await EnsureLoaded(contentUrl);
            return scraper.GetAllSections();
        }

        public async Task<AbstractScraper> GetScraper(string url)
        {
            await EnsureLoaded(url);
            return scraper;
        }
        public async Task<ContentPageHtml> GetHtml(string url)
        {
            await EnsureLoaded(url);
            return scraper.GetPageHtml();
        }

        public async Task<string> GetHtmlPageBody(string url)
        {
            await EnsureLoaded(url);
            return scraper.GetPageText();
        }
    }
}