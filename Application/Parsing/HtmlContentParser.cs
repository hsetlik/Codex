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
        public static async Task<ContentMetadataDto> ParseToContent(string url)
        {
            var parser = ContentParserFactory.ParserFor(url);
            return await parser.Parse();
        }

        public HtmlContentParser(string url)
        {
            this.Url = url;
        }

        protected async Task LoadHtml()
        {
            var web = new HtmlWeb();
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
            if (url.Contains("wikipedia"))
            {
                Console.WriteLine("detected wikipedia page");
                return ParserProfile.Wikipedia;
            }
            else
            {
                return ParserProfile.NewsArticle;
            }
        }   
    }
    //Subclasses for parsing each profile
    
}