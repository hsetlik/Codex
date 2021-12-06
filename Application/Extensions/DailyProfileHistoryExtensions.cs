using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.ContentHistory;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Extensions
{
    public static class DailyProfileHistoryExtensions
    {
        public static async Task<Result<DailyKnownWordsDto>> GetKnownWordsForDay(
            this DataContext context, 
            DateTime dateTime, 
            string language, 
            string username)
        {
            // 1. find a matching userlanguageprofile
            var profile = await context.UserLanguageProfiles
                .Include(p => p.User)
                .Include(p => p.DailyProfileHistory)
                .FirstOrDefaultAsync(p => p.Language == language && p.User.UserName == username);
            if (profile == null)
                return Result<DailyKnownWordsDto>.Failure("Language profile not found");
            var date = dateTime.Date;
            // 2. Select by date and profile ID
            var knownWords = await context.DailyKnownWords.FirstOrDefaultAsync(
                d => d.Date.Date == date && 
                profile.DailyProfileHistory.DailyProfileHistoryId == d.DailyProfileHistoryId);
            if (knownWords == null)
                return Result<DailyKnownWordsDto>.Failure("Known words entry not found");
            var output = new DailyKnownWordsDto
            {
                Value = knownWords.KnownWords,
                Date = date
            };
            return Result<DailyKnownWordsDto>.Success(output);
        }

        public static async Task<Result<DailyKnownWordsDto>> GetKnownWordsForDay(
            this DataContext context,
            DateTime date,
            Guid langProfileId
        )
        {
            var langProfile = await context.UserLanguageProfiles
                .Include(t => t.DailyProfileHistory)
                .FirstOrDefaultAsync(p => p.LanguageProfileId == langProfileId);
            if (langProfile == null)
                return Result<DailyKnownWordsDto>.Failure("Profile not found");
            var knownWords = await context.DailyKnownWords
                .FirstOrDefaultAsync(w => w.DailyProfileHistory.LanguageProfileId == langProfileId &&
                w.Date.Date == date.Date);
            if (knownWords == null)
                return Result<DailyKnownWordsDto>.Failure("Could not load known words");
            var output = new DailyKnownWordsDto
            {
                Value = knownWords.KnownWords,
                Date = date
            };
            return Result<DailyKnownWordsDto>.Success(output);
        }
    }
}