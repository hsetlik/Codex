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
                Console.WriteLine($"Scraper preparation for {url} took {watch.ElapsedMilliseconds} ms");
            }
        }
            public async Task<ContentSection> GetSection(string contentUrl, int index)
        {
           Console.WriteLine($"Getting section {index} of content: {contentUrl}"); 
           await EnsureLoaded(contentUrl);
           return scraper.GetSection(index);
        }

        public async Task<ContentMetadataDto> GetContentMetadata(string url)
        {
            Console.WriteLine($"Metadata requested for {url}");
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

        public async Task<string> GetRawHtml(string url)
        {
            Console.WriteLine($"Getting html for: {url}");
            await EnsureLoaded(url);
            return scraper.GetHtmlString();
        }

        public async Task<ContentPageHtml> GetHtml(string url)
        {
            await EnsureLoaded(url);
            return new ContentPageHtml
            {
                ContentUrl = url,
                Html = scraper.GetHtmlString(),
                Stylesheets = new List<StylesheetFile>()
            };
        }
    }
}