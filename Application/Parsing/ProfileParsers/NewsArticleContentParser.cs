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