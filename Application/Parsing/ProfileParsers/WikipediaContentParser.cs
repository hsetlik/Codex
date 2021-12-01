using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DomainDTOs;

namespace Application.Parsing.ProfileParsers
{
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

        public override async Task<ContentParagraph> GetParagraph(int index)
        {
             if (!HasLoadedHtml)
                await LoadHtml();
            if (loadedHtml == null)
            {

            }
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
            if (!HasLoadedHtml)
                await LoadHtml();
            return new ContentMetadataDto
            {
                ContentName = "dummy content name",
                ContentType = null,
                Language = null,
                VideoUrl = "none",
                AudioUrl = "none",
                ContentUrl = Url
            };
        }
    }

}