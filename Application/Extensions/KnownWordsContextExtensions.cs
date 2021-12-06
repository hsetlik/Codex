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
                return Result<KnownWordsDto>.Failure("Could not find content");
            int total = 0;
            int known = 0;
            var numParagraphs = await parser.GetNumParagraphs(content.ContentUrl);
            var paragraphs = new List<ContentParagraph>();
            for(int i = 0; i < numParagraphs; ++i)
            {
                paragraphs.Add(await parser.GetParagraph(content.ContentUrl, i));
            }
            foreach(var paragraph in paragraphs)
            {
                var knownWords = await context.KnownWordsForParagraph(paragraph, parser, username, content.Language);
                Console.WriteLine($"Paragraph #{paragraph.Index} of {paragraph.ContentUrl} in language {content.Language} for user {username}");
                if (!knownWords.IsSuccess)
                    return Result<KnownWordsDto>.Failure($"Known words not loaded for content: {paragraph.ContentUrl} paragraph #{paragraph.Index}");
                total += knownWords.Value.TotalWords;
                known += knownWords.Value.KnownWords;
            }
            var output = new KnownWordsDto
            {
                TotalWords = total,
                KnownWords = known
            };
            return Result<KnownWordsDto>.Success(output);
        }        
        
        public static async Task<Result<KnownWordsDto>> KnownWordsForParagraph(
            this DataContext context,
            ContentParagraph paragraph, 
            IParserService parser, 
            string username, 
            string language)
        {
            var allTerms = paragraph.Value.Split(' ');
            var tasks = new List<Task<Result<AbstractTermDto>>>();
            foreach(var term in allTerms)
            {
                var dto = new TermDto
                {
                    Value = term,
                    Language = language
                };
                tasks.Add(context.AbstractTermFor(dto, username));
            }
            var termResults = await Task.WhenAll<Result<AbstractTermDto>>(tasks);
            int known = 0;
            int total = 0;
            foreach(var res in termResults)
            {
                if (!res.IsSuccess)
                    return Result<KnownWordsDto>.Failure("Failed to load term");
                total += 1;
                if (res.Value.Rating >= 3)
                    known += 1;
            }
            var output = new KnownWordsDto
            {
                TotalWords = total,
                KnownWords = known
            };
            return Result<KnownWordsDto>.Success(output);
        }


        public static async Task<Result<Unit>> UpdateKnownWords(this DataContext context, UserTerm term, UserTermDetailsDto details)
        {
            var originalRating = term.Rating;
            var profile = await context.UserLanguageProfiles.FindAsync(term.LanguageProfileId);
            if (profile == null)
                return Result<Unit>.Failure("Could not load profile");
            var newTerm = term.UpdatedWith(details);
            if (originalRating < 3 && newTerm.Rating >= 3)
            {
                profile.KnownWords = profile.KnownWords + 1;

            }
            if (originalRating >= 3 && newTerm.Rating < 3)
            {
                profile.KnownWords = profile.KnownWords - 1;
            }
            var success = await context.SaveChangesAsync() > 0;
            if (!success)
                return Result<Unit>.Failure("Could not save changes");
            return Result<Unit>.Success(Unit.Value);
        }


        
    }
}