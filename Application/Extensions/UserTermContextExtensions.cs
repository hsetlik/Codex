using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DataObjectHandling.Terms;
using Application.DataObjectHandling.UserTerms;
using Application.DomainDTOs;
using Application.DomainDTOs.ProfileHistory;
using Application.Interfaces;
using Application.Parsing;
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

        public static async Task<Result<List<UserTermDetailsDto>>> UserTermsCreatedAt(this DataContext context, string language, string username, DateTime date)
        {
            var profile = await context.UserLanguageProfiles
                .Include(u => u.User)
                .FirstOrDefaultAsync(u => u.Language == language && u.User.UserName == username);
            if (profile == null)
                return Result<List<UserTermDetailsDto>>.Failure($"Could not find profile for user {username} and language {language}");
            var matches = await context.UserTerms
            .Include(u => u.Term)
            .Where(u => u.LanguageProfileId == profile.LanguageProfileId && u.CreatedAt.Date == date.Date)
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

        public static async Task<Result<Unit>> CreateDummyUserTerm(this DataContext context, UserTermCreateDto dto, string username, int dateRange=14)
        {
            Console.WriteLine($"Creating dummy userterm for {dto.TermValue}");
            var profile = await context.UserLanguageProfiles.Include(p => p.User).FirstOrDefaultAsync(p => p.User.UserName == username && p.Language == dto.Language);
            if (profile == null)
                return Result<Unit>.Failure($"Could not get profile for {username} with language {dto.Language}");
            var termResult = await context.CreateAndGetTerm(dto.Language, dto.TermValue);
            if (!termResult.IsSuccess)
                return Result<Unit>.Failure($"Could not get term!: Error message{termResult.Error}");
            var term = termResult.Value;
            var existingTerm = context.UserTerms
                .FirstOrDefaultAsync(u => u.LanguageProfileId == profile.LanguageProfileId && 
                u.TermId == term.TermId);
            if (existingTerm != null)
            {
                Console.WriteLine($"UserTerm for {term.NormalizedValue} already exists!");
                return Result<Unit>.Success(Unit.Value);
            }
            var r = new Random();
            var dateOffset = r.NextDouble() * dateRange;
            var createTime = DateTime.Now.AddDays(dateOffset * -1.0f);

            int timesSeen = (int)(r.NextDouble() * 5);
            int rating = (int)(r.NextDouble() * 5);
            float intervalDays = (float)r.NextDouble() * 4.0f;


            var userTerm = new UserTerm
            {
                LanguageProfileId = profile.LanguageProfileId,
                UserLanguageProfile = profile,
                TermId = term.TermId,
                Term = term,
                NormalizedTermValue = term.NormalizedValue,
                Translations = 
                {
                    new UserTermTranslation
                    {
                        Value = dto.FirstTranslation
                    }
                },
                TimesSeen = timesSeen,
                EaseFactor = 2.5f,
                Rating = rating,
                SrsIntervalDays = intervalDays,
                CreatedAt = createTime,
                DateTimeDue = createTime.ToString()
            };

            context.UserTerms.Add(userTerm);

            var success = await context.SaveChangesAsync() > 0;
            if (!success)
                return Result<Unit>.Failure("Could not create userTerm!");
            Console.WriteLine($"Created userTerm for {dto.TermValue} and user {username}");
            return Result<Unit>.Success(Unit.Value);
        }
    }
}