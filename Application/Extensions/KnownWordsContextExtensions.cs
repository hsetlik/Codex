using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Core;
using Application.DataObjectHandling.Terms;
using Application.DataObjectHandling.UserTerms;
using Application.DomainDTOs.UserLanguageProfile;
using Application.Interfaces;
using Application.Parsing;
using Domain.DataObjects;
using MediatR;
using Persistence;

namespace Application.Extensions
{
    public static class KnownWordsContextExtensions
    {
        //KNOWN WORDS
        public static async Task<Result<KnownWordsDto>> GetKnownWords(this DataContext context, Guid contentId, string username, IParserService parser)
        {
            var content = await context.Contents.FindAsync(contentId);
            if (content == null)
                return Result<KnownWordsDto>.Failure($"Could not load content with id: {contentId}");
            int total = 0;
            int known = 0;
            for(int i = 0; i < content.NumSections; ++i)
            {
                var section =  await parser.GetSection(content.ContentUrl, i);
                if (section == null)
                    return Result<KnownWordsDto>.Failure($"Could not load section {i} from URL {content.ContentUrl}");
                var knownResult = await context.KnownWordsForSection(section, parser, username);
                if (!knownResult.IsSuccess)
                    return Result<KnownWordsDto>.Failure($"Failed to get known words! Error message: {knownResult.Error}");
                total += knownResult.Value.TotalWords;
                known += knownResult.Value.KnownWords;
            }
            return Result<KnownWordsDto>.Success(new KnownWordsDto
            {
                TotalWords = total,
                KnownWords = known
            });
        }        
        
        public static async Task<Result<KnownWordsDto>> KnownWordsForSection(
            this DataContext context,
            ContentSection section, 
            IParserService parser, 
            string username)
        {
           var chunk = await context.AbstractTermsForSection(section.ContentUrl, section.Index, parser, username);
           if (!chunk.IsSuccess)
           {
               return Result<KnownWordsDto>.Failure($"Could not get abstract terms! Error message: {chunk.Error}");
           }
           var terms = chunk.Value.AbstractTerms;
           int known = 0;
           int total = terms.Count;
           foreach(var term in terms)
           {
               if(term.Rating >= 3)
                ++known;
           }
           return Result<KnownWordsDto>.Success(new KnownWordsDto
           {
               TotalWords = total,
               KnownWords = known
           });
        }

        //public static async Task<bool> TermKnown(this DataContext context, Guid lanuageProfileId, )

    }
}