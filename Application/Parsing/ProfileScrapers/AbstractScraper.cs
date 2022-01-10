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
        public abstract List<ContentSection> GetAllSections();
        public abstract string GetHtmlString();

        public List<List<string>> GetWordLists(int size = 50, int maximum = 1000)
        {
            var output = new List<List<string>>();
            var allTerms = new List<string>();
            var sections = GetAllSections();
            // get all the words into one list
            foreach(var section in sections)
            {
                var sectionTerms = section.Body.Split(' ');
                allTerms.AddRange(sectionTerms);
            }
            // cut allTerms down to size as needed
            var surplus = allTerms.Count - maximum;
            if (surplus > 0)
                allTerms.RemoveRange(maximum, surplus);
            // slice up into lists
            while(allTerms.Count > 0)
            {
                if (allTerms.Count > size)
                {
                    output.Add(allTerms.Take(size).ToList());
                    allTerms.RemoveRange(0, size);
                }
                else
                {
                    var cpy = new List<string>(allTerms);
                    output.Add(cpy);
                    allTerms.Clear();
                }
            }
            return output;
        }
        // do the actual scraping in here: needs to populate the ContentStorage object from the URL
        public abstract Task PrepareAsync();

        
        public static ScraperProfile ProfileFor(string url)
        {
            Console.WriteLine($"Getting scraper for content: {url}");
            if (url.Contains("wikipedia"))
            {
                return ScraperProfile.Wikipedia;
            }
            else if (url.Contains("youtube"))
            {
                return ScraperProfile.Youtube;
            }
            else
            {
                return ScraperProfile.NewsArticle;
            }
        }   
    }
    
}