using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using HtmlAgilityPack;
using MediatR;

namespace Application.Parsing
{
    public abstract class AbstractScraper
    {
        public string Url { get; private set; }
        protected HtmlDocument loadedHtml = null;

        protected bool contentsLoaded = false;

        public bool ContentsLoaded {get {return contentsLoaded; }}

        public AbstractScraper(string url)
        {
            this.Url = url;
        }

        protected async Task LoadHtml()
        {
            var web = new HtmlWeb();
            Console.WriteLine($"Loading HTML for: {Url}");
            loadedHtml =  await web.LoadFromWebAsync(Url);
        }
        
        //Abstract methods to correspond with the IParserService methods (and ultimately endpoints)
        public abstract ContentMetadataDto GetMetadata();
        public abstract int GetNumSections();
        public abstract ContentSection GetSection(int index);

        // do the actual scraping in here
        public abstract Task PrepareAsync();

        
        public static ScraperProfile ProfileFor(string url)
        {
            Console.WriteLine($"Getting scraper for content: {url}");
            if (url.Contains("wikipedia"))
            {
                Console.WriteLine($"Found wikipedia page at: {url}");
                return ScraperProfile.Wikipedia;
            }
            else
            {
                return ScraperProfile.NewsArticle;
            }
        }   
    }
    
}