using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DataObjectHandling.Terms;
using Application.DataObjectHandling.UserTerms;
using Application.DomainDTOs;
using Application.DomainDTOs.ProfileHistory;
using Domain.DataObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Extensions
{
    public  static class UserTermContextExtensions
    {
        public static async Task<Result<Unit>> UpdateUserTerm(this DataContext context, UserTermDto dto)
        {
            var userTerm = await context.UserTerms
            .Include(t => t.Translations)
            .FirstOrDefaultAsync(u => u.UserTermId == dto.UserTermId);
            if (userTerm == null) return Result<Unit>.Failure("No matching user term");
            userTerm.Rating = dto.Rating;
            userTerm.SrsIntervalDays = dto.SrsIntervalDays;
            userTerm.TimesSeen = userTerm.TimesSeen + 1;
            userTerm.UpdateTranslations(dto.Translations);

            var success = await context.SaveChangesAsync() > 0;
            if (!success) return Result<Unit>.Failure("Context could not be updated!");
            return Result<Unit>.Success(Unit.Value);
        }

        public static async Task<Result<Unit>> CreateUserTerm(this DataContext context, UserTermDto dto, string username)
        {
            var term = await context.Terms.FirstOrDefaultAsync(u => u.Language == dto.Language && u.NormalizedValue == dto.Value);
            var userProfile = await context.UserLanguageProfiles
            .Include(u => u.User)
            .FirstOrDefaultAsync(u => u.Language == dto.Language && u.User.UserName == username);

            if (term == null)
                return Result<Unit>.Failure("no term found");
            var uTerm = new UserTerm
            {
                Term = term,
                UserLanguageProfile = userProfile,
                Rating = dto.Rating,
                SrsIntervalDays = dto.SrsIntervalDays,
                EaseFactor = dto.EaseFactor,
                DateTimeDue = DateTime.Today.ToString(),
                TimesSeen = dto.TimesSeen,
                NormalizedTermValue = term.NormalizedValue,
                CreatedAt = DateTime.Now
            };

            uTerm.Translations = uTerm.GetAsTranslations(dto.Translations);

            context.UserTerms.Add(uTerm);
            var success = await context.SaveChangesAsync() > 0;
            if (!success)
                return Result<Unit>.Failure("Changes not saved!");
            return Result<Unit>.Success(Unit.Value);
        }

        public static async Task<Result<List<UserTermDetailsDto>>> UserTermsDueNow(this DataContext context, LanguageNameDto dto, string username)
        {
            var output = new List<UserTermDetailsDto>();
            var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
                return Result<List<UserTermDetailsDto>>.Failure("User not found!");
            var profile = await context.UserLanguageProfiles
            .Include(u => u.UserTerms)
            .ThenInclude(t => t.Term) // we need to return the term value in the response body
            .FirstOrDefaultAsync(u => u.Language == dto.Language && u.UserId == user.Id);
            if (profile == null)
                return Result<List<UserTermDetailsDto>>.Failure("Profile not found!");
            var currentTime = DateTime.Now;
            foreach(var term in profile.UserTerms)
            {
                var termDue = DateTime.Parse(term.DateTimeDue);
                if (currentTime > termDue)
                {
                    output.Add(term.GetUserTermDetailsDto());
                }
            };
            return Result<List<UserTermDetailsDto>>.Success(output);
        }

        public static async Task<Result<List<UserTermDetailsDto>>> UserTermsCreatedAt(this DataContext context, string language, string username, DateQuery date)
        {
            var profile = await context.UserLanguageProfiles
                .Include(u => u.User)
                .FirstOrDefaultAsync(u => u.Language == language && u.User.UserName == username);
            if (profile == null)
                return Result<List<UserTermDetailsDto>>.Failure($"Could not find profile for user {username} and language {language}");
            var matches = await context.UserTerms
            .Include(u => u.Term)
            .Where(u => u.LanguageProfileId == profile.LanguageProfileId && date.Matches(u.CreatedAt))
            .ToListAsync();

            if (matches == null || matches.Count < 1)
                return Result<List<UserTermDetailsDto>>.Failure($"No matches found");
            var output = new List<UserTermDetailsDto>();
            foreach(var match in matches)
            {
                output.Add(match.GetUserTermDetailsDto());
            }
            return Result<List<UserTermDetailsDto>>.Success(output);
        }
    }
}