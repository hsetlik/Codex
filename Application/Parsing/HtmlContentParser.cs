using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using HtmlAgilityPack;

namespace Application.Parsing
{
    public abstract class HtmlContentParser
    {
        public string Url { get; private set; }
        protected HtmlDocument loadedHtml = null;
        public bool HasLoadedHtml {get {return !(loadedHtml == null);}}

        public HtmlContentParser(string url)
        {
            this.Url = url;
        }

        protected async Task LoadHtml()
        {
            var web = new HtmlWeb();
            Console.WriteLine($"Loading HTML for: {Url}");
            loadedHtml =  await web.LoadFromWebAsync(Url);
        }
        
        public async Task<ContentMetadataDto> Parse()
        {
            return await GetMetadata();
        }
        //Abstract methods to correspond with the IParserService methods (and ultimately endpoints)
        public abstract Task<ContentMetadataDto> GetMetadata();
        public abstract Task<int> GetNumParagraphs();
        public abstract Task<ContentParagraph> GetParagraph(int index);
        

        public static ParserProfile ProfileFor(string url)
        {
            Console.WriteLine($"Getting parser for content: {url}");
            if (url.Contains("wikipedia"))
            {
                Console.WriteLine($"Found wikipedia page at: {url}");
                return ParserProfile.Wikipedia;
            }
            else
            {
                return ParserProfile.NewsArticle;
            }
        }   
    }
    
}