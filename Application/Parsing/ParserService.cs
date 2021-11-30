using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.Terms;
using Application.DomainDTOs;
using Application.Interfaces;

namespace Application.Parsing
{
    public class ParserService : IParserService
    {
        public bool IsParserReady { get; private set; }
        private HtmlContentParser parser = null;
        private ContentMetadataDto loadedContent = null;
        public string CurrentUrl { get; private set; }
        public ParserService()
        {
            IsParserReady = false;
        }
        public async void PrepareForContent(string url)
        {
            CurrentUrl = url;
            IsParserReady = false;
            parser = null;
            loadedContent = null;
            parser = HtmlContentParser.ParserFor(url);
            loadedContent = await parser.Parse();
            IsParserReady = true;
        }

        public async Task<int> GetNumParagraphs()
        {
            return await parser.GetNumParagraphs();
        }

        public Task<List<AbstractTermDto>> AbstractTermsForParagraph(string contentUrl, int paragraphIndex)
        {
            throw new NotImplementedException();
        }
    }
}