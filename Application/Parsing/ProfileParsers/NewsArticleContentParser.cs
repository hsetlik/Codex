using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DomainDTOs;

namespace Application.Parsing.ProfileParsers
{
       // NewsArticle
    public class NewsArticleContentParser : HtmlContentParser
    {
        public NewsArticleContentParser(string url) : base(url)
        {

        }

        public override async Task<int> GetNumSections()
        {
            if (loadedHtml == null)
                await LoadHtml();
            return loadedHtml.DocumentNode.SelectNodes("//body/p").Count;
        }

        public override async Task<ContentSection> GetSection(int index)
        {
            if (loadedHtml == null)
                await LoadHtml();
            var section = loadedHtml.DocumentNode.Descendants("p").ElementAt(index);
            return new ContentSection
            {
                ContentUrl = Url,
                Index = index,
                Value = section.GetDirectInnerText()
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

        public override Task ParseToContent()
        {
            throw new NotImplementedException();
        }
    }

}