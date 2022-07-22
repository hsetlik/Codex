using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.Term;
using Application.Interfaces;
using Application.Utilities;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Extensions
{
    public static class TermSearchExtensions
    {
        public static async Task<Result<TermSearchResult>> FirstContaining(this DataContext context, IParserService parser, string termValue, string language)
        {
            // grab all the URLs for this language
            List<string> urls = await context.Contents.Where(c => c.Language == language).Select(c => c.ContentUrl).ToListAsync();
            string normValue = termValue.AsTermValue().ToUpper();
            foreach(string url in urls)
            {
                var metadata = await parser.GetContentMetadata(url);
                for(int i = 0; i < metadata.NumSections; ++i)
                {
                    //TODO: move all this to ContextFactory and do this w/ multithreading
                    var section = await parser.GetSection(url, i);
                    for(int n = 0; n < section.TextElements.Count; ++n)
                    {
                        if (section.TextElements[n].ElementText.ToUpper().Contains(normValue))
                        {
                            return Result<TermSearchResult>.Success(new TermSearchResult
                            {
                                QueryValue = termValue,
                                Language = language,
                                ContentSection = section,
                                MatchElementIndex = n
                            });
                        }
                    }
                }
            }
            return Result<TermSearchResult>.Failure($"No contents containing term {termValue}");
        }
        
    }
}