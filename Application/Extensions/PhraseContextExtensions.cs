using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using Application.Utilities;
using Domain.DataObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Extensions
{
    public static class PhraseContextExtensions
    {
        public static async Task<Result<Unit>> CreatePhrase(this DataContext context, PhraseCreateQuery dto, string username)
        {
            var profile = await context.UserLanguageProfiles
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Language == dto.Language &&
                p.User.UserName == username);
            if (profile == null)
                return Result<Unit>.Failure($"Could not find profile for {username}");
            

            var phrase = new Phrase
            {
                UserLanguageProfile = profile,
                LanguageProfileId = profile.LanguageProfileId,
                Value = dto.Value.AsPhraseValue(),
                TimesSeen = 0,
                EaseFactor = 2.5f,
                Rating = 0,
                DateTimeDue = DateTime.Now.ToString(),
                SrsIntervalDays = 0.125f,
                CreatedAt = DateTime.Now
            };
            phrase.Translations.Add(new PhraseTranslation
            {
                Value = dto.FirstTranslation,
                PhraseId = phrase.PhraseId,
                Phrase = phrase
            });
            context.Phrases.Add(phrase);

            var success = await context.SaveChangesAsync() > 0;
            if (!success)
                return Result<Unit>.Failure("Could not update database!");
            return Result<Unit>.Success(Unit.Value);
            
        }

        public static async Task<Result<Unit>> DeletePhrase(this DataContext context, Guid phraseId)
        {
            var phrase = await context.Phrases.FindAsync(phraseId);
            if (phrase == null)
                return Result<Unit>.Failure($"Could not find phrase with ID: {phraseId}");
            context.Phrases.Remove(phrase);
            var success = await context.SaveChangesAsync() > 0;
            if (!success)
                return Result<Unit>.Failure($"Could not remove phrase with ID: {phraseId}");
            return Result<Unit>.Success(Unit.Value);
            
        }

        public static async Task<Result<PhraseDto>> GetDetails(this DataContext context, PhraseQuery query, string username)
        {
            var profile = await context.UserLanguageProfiles.Include(p => p.User).FirstOrDefaultAsync(p => p.Language == query.Language && p.User.UserName == username);
            if (profile == null)
                return Result<PhraseDto>.Failure($"Could not find profile in language {query.Language} for user {username}");
            string value = query.Value.AsPhraseValue();
            var phrase = await context.Phrases
                .Include(p => p.Translations)
                .FirstOrDefaultAsync(p => p.LanguageProfileId == profile.LanguageProfileId && p.Value == value);
            if (phrase == null)
                return Result<PhraseDto>.Failure($"Could not find phrase {value} for user {username}");
            return Result<PhraseDto>.Success(new PhraseDto(phrase));

        }
        
    }
}