using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.Phrase;
using Domain.DataObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Extensions
{
    public static class PhraseContextExtensions
    {
        public static async Task<Result<Unit>> CreatePhrase(this DataContext context, PhraseCreateDto dto, string username)
        {
            var profile = await context.UserLanguageProfiles
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Language == dto.AbstractTerms[0].Language &&
                p.User.UserName == username);
            if (profile == null)
                return Result<Unit>.Failure($"Could not find profile for {username}");
            
            string phraseValue = "";
            foreach(var term in dto.AbstractTerms)
            {
                phraseValue += term.TermValue;
                if (term != dto.AbstractTerms.Last())
                    phraseValue += ' ';
            }

            var phrase = new Phrase
            {
                UserLanguageProfile = profile,
                LanguageProfileId = profile.LanguageProfileId,
                Value = phraseValue,
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
        
    }
}