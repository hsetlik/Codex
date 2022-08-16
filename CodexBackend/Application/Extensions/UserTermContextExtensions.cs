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
using Application.Utilities;
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
            .Include(u => u.UserLanguageProfile)
            .FirstOrDefaultAsync(u => u.UserTermId == dto.UserTermId);
            if (userTerm == null) return Result<Unit>.Failure("No matching user term");
            userTerm.Rating = dto.Rating;
            userTerm.SrsIntervalDays = dto.SrsIntervalDays;
            userTerm.TimesSeen = userTerm.TimesSeen + 1;
            // first, remove any translations that are no longer in the list
            foreach(var tran in userTerm.Translations)
            {
                if (!dto.Translations.Any(v => v == tran.UserValue))
                {
                    userTerm.Translations.Remove(tran);
                }
            } 
            // now, create any new translations as necessary
            foreach(var tran in dto.Translations)
            {
                if (!userTerm.Translations.Any(r => r.UserValue == tran))
                {
                    userTerm.Translations.Add(new Translation
                    {
                        TermValue = userTerm.TermValue,
                        TermLanguage = userTerm.Language,
                        UserValue = tran,
                        UserLanguage = userTerm.UserLanguageProfile.UserLanguage,
                        UserTermId = userTerm.UserTermId,
                        UserTerm = userTerm
                    });
                }
            }            
            var success = await context.SaveChangesAsync() > 0;
            if (!success) return Result<Unit>.Failure("Context could not be updated!");
            return Result<Unit>.Success(Unit.Value);
        }

        public static async Task<Result<Unit>> CreateUserTerm(this DataContext context, UserTermDto dto, string username)
        {
            var userProfile = await context.UserLanguageProfiles
            .Include(u => u.User)
            .FirstOrDefaultAsync(u => u.Language == dto.Language && u.User.UserName == username);
            string normValue = dto.Value.AsTermValue().ToUpper();

            var uTerm = new UserTerm
            {
                UserLanguageProfile = userProfile,
                Language = userProfile.Language,
                Rating = dto.Rating,
                SrsIntervalDays = dto.SrsIntervalDays,
                EaseFactor = dto.EaseFactor,
                DateTimeDue = DateTime.Today,
                TimesSeen = dto.TimesSeen,
                TermValue = normValue,
                CreatedAt = DateTime.Now.ToUniversalTime(),
                Translations = new List<Translation>(),
                Starred = false
            };
            //now add the translations
            foreach(var t in dto.Translations)
            {
                uTerm.Translations.Add(new Translation
                {
                    TermLanguage = uTerm.Language,
                    TermValue = normValue,
                    UserLanguage = userProfile.UserLanguage,
                    UserValue = t,
                    UserTermId = uTerm.UserTermId,
                    UserTerm = uTerm
                });
            }
            context.UserTerms.Add(uTerm);
            var success = await context.SaveChangesAsync() > 0;
            if (!success)
                return Result<Unit>.Failure("Changes not saved!");
            return Result<Unit>.Success(Unit.Value);
        }

        public static async Task<Result<List<UserTermDetailsDto>>> UserTermsDueNow(this DataContext context, LanguageNameQuery dto, string username)
        {
            var output = new List<UserTermDetailsDto>();
            var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
                return Result<List<UserTermDetailsDto>>.Failure("User not found!");
            var profile = await context.UserLanguageProfiles
            .Include(u => u.UserTerms)
            .FirstOrDefaultAsync(u => u.Language == dto.Language && u.UserId == user.Id);
            if (profile == null)
                return Result<List<UserTermDetailsDto>>.Failure("Profile not found!");
            var currentTime = DateTime.Now.ToUniversalTime();
            var mapper = MapperFactory.GetDefaultMapper();
            foreach(var term in profile.UserTerms)
            {
                if (currentTime > term.DateTimeDue)
                {
                    output.Add(mapper.Map<UserTermDetailsDto>(term));
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
            .Where(u => u.LanguageProfileId == profile.LanguageProfileId && u.CreatedAt.Date == date.Date)
            .ToListAsync();

            if (matches == null || matches.Count < 1)
                return Result<List<UserTermDetailsDto>>.Failure($"No matches found");
            var output = new List<UserTermDetailsDto>();
            var mapper = MapperFactory.GetDefaultMapper();
            foreach(var match in matches)
            {
                output.Add(mapper.Map<UserTermDetailsDto>(match));
            }
            return Result<List<UserTermDetailsDto>>.Success(output);
        }
        public static async Task<Result<List<UserTermDetailsDto>>> GetStarred(this DataContext context, Guid langProfileId)
        {
            var mapper = MapperFactory.GetDefaultMapper();
            
            var starredTerms = await context.UserTerms.Where(u => u.LanguageProfileId == langProfileId && u.Starred).ToListAsync();
            if (starredTerms == null)
                return Result<List<UserTermDetailsDto>>.Failure("Could not load starred terms!");
            var starredDtos = starredTerms.Select(u => mapper.Map<UserTermDetailsDto>(u)).ToList();
            return Result<List<UserTermDetailsDto>>.Success(starredDtos);
        }
        public static async Task<Result<Unit>> CreateDummyUserTerm(this DataContext context, UserTermCreateQuery dto, string username, int dateRange=14)
        {
            var profile = await context.UserLanguageProfiles.Include(p => p.User).FirstOrDefaultAsync(p => p.User.UserName == username && p.Language == dto.Language);
            if (profile == null)
                return Result<Unit>.Failure($"Could not get profile for {username} with language {dto.Language}");
            string normValue = dto.TermValue.AsTermValue().ToUpper();
            
            var r = new Random();
            var dateOffset = r.NextDouble() * dateRange;
            var createTime = DateTime.Now.ToUniversalTime().AddDays(dateOffset * -1.0f);

            int timesSeen = (int)(r.NextDouble() * 5);
            int rating = (int)(r.NextDouble() * 5);
            if (rating >= 3)
                profile.KnownWords = profile.KnownWords + 1;
            float intervalDays = (float)r.NextDouble() * 4.0f;
            var userTerm = new UserTerm
            {
                LanguageProfileId = profile.LanguageProfileId,
                UserLanguageProfile = profile,
                Language = profile.Language,
                TermValue = normValue,
                Translations = 
                {
                    new Translation
                    {
                        TermValue = normValue,
                        TermLanguage = profile.Language,
                        UserValue = dto.FirstTranslation,
                        UserLanguage = profile.UserLanguage
                    }
                },
                TimesSeen = timesSeen,
                EaseFactor = 2.5f,
                Rating = rating,
                SrsIntervalDays = intervalDays,
                CreatedAt = createTime,
                DateTimeDue = createTime.AddDays(rating),
                Starred = false
            };

            context.UserTerms.Add(userTerm);

            var success = await context.SaveChangesAsync() > 0;
            if (!success)
                return Result<Unit>.Failure("Could not create userTerm!");
            var updateResult = await context.AddRecord(new DomainDTOs.UserLanguageProfile.LanguageProfileDto
            {
                Language = dto.Language,
                LanguageProfileId = profile.LanguageProfileId,
                KnownWords = profile.KnownWords
            }, createTime);
            if (!updateResult.IsSuccess)
                return Result<Unit>.Failure($"Could not update history! Error message: {updateResult.Error}");
            return Result<Unit>.Success(Unit.Value);
        }
    }
}