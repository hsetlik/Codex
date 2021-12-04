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
        
        private HtmlContentParser parser;
        private ContentMetadataDto loadedContent;


        public string CurrentUrl { get; private set; }
        public bool ParserReadyFor(string contentUrl)
        {
            return (loadedContent != null && loadedContent.ContentUrl == contentUrl);
        }
        public ParserService()
        {
            
            
        }


        public async Task PrepareForContent(string url)
        {
            Console.WriteLine($"Preparing parser for content {url}");
            if (!ParserReadyFor(url))
            {
                CurrentUrl = url;
                parser = ContentParserFactory.ParserFor(url);
                loadedContent = await parser.Parse();
            }
            else
                Console.WriteLine($"Parser already prepared for content: {url}");
        }
        public async Task<ContentMetadataDto> GetContentMetadata(string url)
        {
            Console.WriteLine($"Requesting metadata for {url}");
            await PrepareForContent(url);
            return loadedContent;
        }

        public async Task<int> GetNumParagraphs(string contentUrl)
        {
            await PrepareForContent(contentUrl);
            Console.WriteLine($"preparing to count paragraphs for: {contentUrl}");
            return await parser.GetNumParagraphs();
        }

        public async Task<ContentParagraph> GetParagraph(string contentUrl, int index)
        {   
            await PrepareForContent(contentUrl);
            return await parser.GetParagraph(index);
        }
    }
}