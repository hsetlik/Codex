using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.Content.Responses;
using Application.DomainDTOs.UserTerm.Queries;
using Application.Interfaces;
using AutoMapper;
using Domain.DataObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Extensions
{
    public class KnownWordsInfo
    {
        public int TotalWords { get; set; }
        public int KnownWords { get; set; }
    }
    public static class ContentDifficultyContextExtensions
    {
        public static async Task<Result<KnownWordsInfo>> GetKnownWordsInString(this DataContext context, Guid profileId, string text, IUserAccessor userAccessor, IDataRepository factory)
        {
            var profile = await context.UserLanguageProfiles.FirstOrDefaultAsync(p => p.LanguageProfileId == profileId);
            if (profile == null)
                return Result<KnownWordsInfo>.Failure($"No profile with ID {profileId}");
            var query = new ElementAbstractTermsQuery
            {
                ContentUrl = "",
                ElementText = text,
                Tag = "raw_text",
                Language = profile.Language
            };
            var terms = await context.GetAbstractTermsForElement(userAccessor, factory, query);
            if (!terms.IsSuccess)
                return Result<KnownWordsInfo>.Failure($"Could not get abstract terms! Error message: {terms.Error}");
            var knownTerms = terms.Value.AbstractTerms.Where(at => at.Rating >= 3).ToList();
            return Result<KnownWordsInfo>.Success(new KnownWordsInfo
            {
                TotalWords = terms.Value.AbstractTerms.Count,
                KnownWords = knownTerms.Count
            });
        }
        public static async Task<Result<ContentDifficultyDto>> GetContentDifficulty(
            this DataContext context, 
            Guid profileId, 
            Guid contentId, 
            IMapper mapper, 
            IParserService parser, 
            IUserAccessor userAccessor, 
            IDataRepository factory)
        {
            var existing = await context.ContentDifficulties.FirstOrDefaultAsync(cd => cd.ContentId == contentId && cd.LanguageProfileId == profileId);
            if (existing != null)
            {
                var output = mapper.Map<ContentDifficultyDto>(existing);
                return Result<ContentDifficultyDto>.Success(output);
            }
            var content = await context.Contents.FirstOrDefaultAsync(c => c.ContentId == contentId);
            if (content == null)
                return Result<ContentDifficultyDto>.Failure($"No content with ID {contentId}");
            
            var profile = await context.UserLanguageProfiles.FirstOrDefaultAsync(p => p.LanguageProfileId == profileId);
            if (profile == null)
                return Result<ContentDifficultyDto>.Failure($"No profile with ID {profileId}");

            var bodyText = await parser.GetHtmlPageBody(content.ContentUrl);
            var knownWordsResult = await context.GetKnownWordsInString(profileId, bodyText, userAccessor, factory);
            if (!knownWordsResult.IsSuccess)
                return Result<ContentDifficultyDto>.Failure($"Could not get known words info! Error message: {knownWordsResult.Error}");
            // create the actual entity
            var difficulty = new ContentDifficulty
            {
                LanguageProfileId = profileId,
                UserLanguageProfile = profile,
                ContentId = content.ContentId,
                Content = content,
                UpdatedAt = DateTime.Now,
                TotalWords = knownWordsResult.Value.TotalWords,
                KnownWords = knownWordsResult.Value.KnownWords
            };
            context.ContentDifficulties.Add(difficulty);
            var success = await context.SaveChangesAsync() > 0;
            if (!success)
                return Result<ContentDifficultyDto>.Failure($"Could not save changes");
            return Result<ContentDifficultyDto>.Success(mapper.Map<ContentDifficultyDto>(difficulty));
        }
        
    }
}