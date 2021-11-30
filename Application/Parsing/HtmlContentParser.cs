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
            var parser = ParserFor(url);
            return await parser.Parse();
        }

        public static HtmlContentParser ParserFor(string url)
        {
            var profile = ProfileFor(url);
            if (profile == ParserProfile.Wikipedia)
            {
                return new WikipediaContentParser(url);
            }
            else
            {
                return new NewsArticleContentParser(url);
            }
        }
        public HtmlContentParser(string url)
        {
            this.Url = url;
        }

        protected async Task LoadHtml()
        {
            var web = new HtmlWeb();
            var doc =  await web.LoadFromWebAsync(Url);
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
                return ParserProfile.Wikipedia;
            }
            else
            {
                return ParserProfile.NewsArticle;
            }
        }   
    }
    //Subclasses for parsing each profile

    //Wikipedia
    public class WikipediaContentParser : HtmlContentParser
    {
        public WikipediaContentParser(string url) : base(url)
        {
        }

        public override Task<int> GetNumParagraphs()
        {
            throw new NotImplementedException();
        }

        public override Task<ContentParagraph> GetParagraph(int index)
        {
            throw new NotImplementedException();
        }

        public override async Task<ContentMetadataDto> GetMetadata()
        {
            if (loadedHtml == null)
                await LoadHtml();
            return new ContentMetadataDto
            {


            };

        }
    }
    // NewsArticle
    public class NewsArticleContentParser : HtmlContentParser
    {
        public NewsArticleContentParser(string url) : base(url)
        {

        }

        public override async Task<int> GetNumParagraphs()
        {
            if (loadedHtml == null)
                await LoadHtml();
            return loadedHtml.DocumentNode.SelectNodes("//body/p").Count;
        }

        public override async Task<ContentParagraph> GetParagraph(int index)
        {
            if (loadedHtml == null)
                await LoadHtml();
            var paragraph = loadedHtml.DocumentNode.Descendants("p").ElementAt(index);
            return new ContentParagraph
            {
                ContentUrl = Url,
                Index = index,
                Value = paragraph.GetDirectInnerText()
            };
        }

        public override async Task<ContentMetadataDto> GetMetadata()
        {
            if (loadedHtml == null)
                await LoadHtml();
            return new ContentMetadataDto
            {

            };

        }

    }
    
}