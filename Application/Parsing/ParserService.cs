using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.Terms;
using Application.DomainDTOs;
using Application.DomainDTOs.Content;
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
        public async Task PrepareForContent(string url)
        {
            CurrentUrl = url;
            IsParserReady = false;
            parser = null;
            loadedContent = null;
            parser = HtmlContentParser.ParserFor(url);
            loadedContent = await parser.Parse();
            IsParserReady = true;
        }
        public async Task<ContentMetadataDto> GetContentMetadata(string url)
        {
            if (loadedContent != null && IsParserReady)
                return loadedContent;
            IsParserReady = false;
            await PrepareForContent(url);
            return loadedContent;
        }

        public Task<int> GetNumParagraphs(string url)
        {
            throw new NotImplementedException();
        }

        public Task<ContentParagraph> GetParagraph(string contentUrl, int index)
        {
            throw new NotImplementedException();
        }
    }
}