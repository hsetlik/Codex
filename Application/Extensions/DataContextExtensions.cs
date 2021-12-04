using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Application.Core;
using Application.DataObjectHandling.Terms;
using Application.DataObjectHandling.UserTerms;
using Application.DomainDTOs;
using Application.DomainDTOs.Content;
using Application.DomainDTOs.ContentViewRecord;
using Application.DomainDTOs.UserLanguageProfile;
using Application.Utilities;
using AutoMapper;
using Domain.DataObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Extensions
{
    public static class DataContextExtensions
    {
        public static async Task<Result<Unit>> CreateTerm(this DataContext context, string language, string termValue)
        {
            var normValue = termValue.AsTermValue();
            var term = await context.Terms.FirstOrDefaultAsync(x => x.Language == language && x.NormalizedValue == normValue);
            if (term != null)
            {
                Console.WriteLine("TERM VALUE: " + term.NormalizedValue + " ALREADY EXISTS" );
                return Result<Unit>.Success(Unit.Value);
            } 
            var newTerm = new Term
            {
                Language = language,
                NormalizedValue = normValue
            };
            context.Terms.Add(newTerm);
            var result = await context.SaveChangesAsync() > 0;
            if(!result) return Result<Unit>.Failure("Term could not be found or created");
            return Result<Unit>.Success(Unit.Value);
        }

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
                TimesSeen = dto.TimesSeen
            };

            uTerm.Translations = uTerm.GetAsTranslations(dto.Translations);

            context.UserTerms.Add(uTerm);
            var success = await context.SaveChangesAsync() > 0;
            if (!success)
                return Result<Unit>.Failure("Changes not saved!");
            return Result<Unit>.Success(Unit.Value);
        }

        public static async Task<Result<AbstractTermDto>> AbstractTermFor(this DataContext context, string term, UserLanguageProfile userLangProfile)
        {
            string termValue = term.AsTermValue();
            var userTerm = await context.UserTerms
            .Include(u => u.Term)
            .FirstOrDefaultAsync(u => u.LanguageProfileId == userLangProfile.LanguageProfileId &&
            u.Term.NormalizedValue == termValue);
            AbstractTermDto output;
             var trailing = StringUtilityMethods.GetTrailing(term);
             if (userTerm != null) 
             {
                 //HasUserTerm = true
                 output = new AbstractTermDto
                 {
                    TrailingCharacters = trailing,
                    Language = userLangProfile.Language,
                    HasUserTerm = true,
                    EaseFactor = userTerm.EaseFactor,
                    SrsIntervalDays = userTerm.SrsIntervalDays,
                    Rating = userTerm.Rating,
                    Translations = userTerm.GetTranslationStrings(),
                    UserTermId = userTerm.UserTermId,
                    TimesSeen = userTerm.TimesSeen
                 };
             }
             else
             {
                 output = new AbstractTermDto
                 {
                    TrailingCharacters = trailing,
                    Language = userLangProfile.Language,
                    HasUserTerm = false,
                    EaseFactor = 0.0f,
                    SrsIntervalDays = 0,
                    Rating = 0,
                    Translations = new List<string>(),
                    TimesSeen = 0
                 };
             }
             //set this back to the original case-sensitive
             output.TermValue = term;
             return Result<AbstractTermDto>.Success(output);
        }

        public static async Task<Result<AbstractTermDto>> AbstractTermFor(this DataContext context, TermDto dto, string username)
        {
            // 1. Get the term
            var normValue = dto.Value.AsTermValue();
            var term = await context.Terms.FirstOrDefaultAsync(
                t => t.Language == dto.Language &&
                t.NormalizedValue == normValue);
            if (term == null)
                return Result<AbstractTermDto>.Failure("No matching term");
            // 2. Get the UserLanguageProfile
            var profile = await context.UserLanguageProfiles
            .Include(p => p.User)
            .FirstOrDefaultAsync(t => t.Language == dto.Language && t.User.UserName == username);
           if (term == null)
                return Result<AbstractTermDto>.Failure("No matching profile");
            
            // 3. Check for a UserTerm
            var userTerm = await context.UserTerms
            .Include(t => t.Translations)
            .FirstOrDefaultAsync(u => u.TermId == term.TermId &&
             u.LanguageProfileId == profile.LanguageProfileId);
             AbstractTermDto output;
             var trailing = StringUtilityMethods.GetTrailing(dto.Value);
             if (userTerm != null) 
             {
                 //HasUserTerm = true
                 output = new AbstractTermDto
                 {
                    TrailingCharacters = trailing,
                    Language = dto.Language,
                    HasUserTerm = true,
                    EaseFactor = userTerm.EaseFactor,
                    SrsIntervalDays = userTerm.SrsIntervalDays,
                    Rating = userTerm.Rating,
                    Translations = userTerm.GetTranslationStrings(),
                    UserTermId = userTerm.UserTermId,
                    TimesSeen = userTerm.TimesSeen
                 };
             }
             else
             {
                 output = new AbstractTermDto
                 {
                    TrailingCharacters = trailing,
                    Language = dto.Language,
                    HasUserTerm = false,
                    EaseFactor = 0.0f,
                    SrsIntervalDays = 0,
                    Rating = 0,
                    Translations = new List<string>(),
                    TimesSeen = 0
                 };
             }
             //! Don't forget to set the TermValue back to the case-sensitive original
             output.TermValue = dto.Value;
            return Result<AbstractTermDto>.Success(output);
        }

        public static async Task<Result<Unit>> UpdateTermAbstract(this DataContext context, AbstractTermDto dto, string username)
        {
            var exisitngTerm = await context.Terms
            .FirstOrDefaultAsync(t => t.Language == dto.Language && 
            t.NormalizedValue == dto.TermValue);
            if (exisitngTerm == null)
            {
                var result = await context.CreateTerm(dto.Language, dto.TermValue);
                if (!result.IsSuccess)
                {
                    return Result<Unit>.Failure("No term could be found or created");
                }
            }
            //reload the exisiting term
            exisitngTerm = await context.Terms
            .FirstOrDefaultAsync(t => t.Language == dto.Language && 
            t.NormalizedValue == dto.TermValue); 

            if (dto.HasUserTerm)
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == username);
                var exisitngUserTerm = await context.UserTerms
                .FirstOrDefaultAsync(u => u.TermId == exisitngTerm.TermId &&
                 u.UserLanguageProfile.UserId == user.Id);
                if (exisitngUserTerm == null)
                {
                    var result = await context.CreateUserTerm(dto.AsUserTerm(), username);
                    if (!result.IsSuccess)
                        return Result<Unit>.Failure("UserTerm was not created");
                }
                else
                {
                    var userTermDto = dto.AsUserTerm();
                    userTermDto.UserTermId = exisitngUserTerm.UserTermId;
                    var result = await context.UpdateUserTerm(dto.AsUserTerm());
                    if (!result.IsSuccess)
                        return Result<Unit>.Failure("UserTerm was not updated"); 
                } 
            }
            return Result<Unit>.Success(Unit.Value);
        }

        public static async Task<Result<Unit>> SetLastStudiedLanguage(this DataContext context, string iso, string username)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null) return Result<Unit>.Failure("Invalid username");
            user.LastStudiedLanguage = iso;
            var success = await context.SaveChangesAsync() > 0;
            if (!success)
                return Result<Unit>.Failure("Changes not saved");
            return Result<Unit>.Success(Unit.Value);
        }

        public static async Task<Result<Unit>> EnsureTermsForContent(this DataContext context, Guid contentId)
        {  
            var content = await context.Contents.FindAsync(contentId);
            // TODO    
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

        public static async Task<Result<Unit>> RecordContentView(this DataContext context, Guid contentId, string username)
        {
            var content = await context.Contents.FindAsync(contentId);
            if (content == null)
                return Result<Unit>.Failure("No matching content found");
            var profile = await context.UserLanguageProfiles
            .Include(p => p.User)
            .Include(u => u.ContentHistory)
            .FirstOrDefaultAsync(t => t.Language == content.Language && t.User.UserName == username);
            if (profile == null)
                return Result<Unit>.Failure("No Profile found");

            var record = new ContentViewRecord
            {
                ContentId = content.ContentId,
                UserLanguageProfile = profile,
                AccessedOn = DateTime.Now
            };
            profile.ContentHistory = profile.ContentHistory.AppendRecord(record);
            var success = await context.SaveChangesAsync() > 0;
            if(!success)
                return Result<Unit>.Failure("Could not save changes");
            return Result<Unit>.Success(Unit.Value);
        }

        public static async Task<Result<List<ContentViewRecordDto>>> AllViewRecords(this DataContext context, Guid contentId)
        {
            var matchingRecords = await context.ContentViewRecords
            .Include(c => c.UserLanguageProfile)
            .ThenInclude(p => p.User)
            .Where(u => u.ContentId == contentId)
            .ToListAsync();
            if (matchingRecords == null)
                return Result<List<ContentViewRecordDto>>.Failure("No matching records found");
            var output = new List<ContentViewRecordDto>();
            foreach(var rec in matchingRecords)
            {
                var dto = new ContentViewRecordDto
                {
                    ContentId = contentId,
                    AccessedOn = rec.AccessedOn,
                    Username = rec.UserLanguageProfile.User.UserName
                };
                output.Add(dto);
            }
            return Result<List<ContentViewRecordDto>>.Success(output);
        }
         public static async Task<Result<Unit>> AddContentTag(this DataContext context, ContentTagDto dto)
         {
            var content = await context.Contents
            .Include(c => c.ContentTags)
            .FirstOrDefaultAsync(t => t.ContentId == dto.ContentId);
            if(content == null)
                return Result<Unit>.Failure("No matching content");
            var tag = new ContentTag
            {
                Content = content,
                ContentId = content.ContentId,
                TagValue = dto.TagValue
            };
            content.ContentTags.Add(tag);
            var success = await context.SaveChangesAsync() > 0;
            if (! success)
                return Result<Unit>.Failure("Could not save changes");
            return Result<Unit>.Success(Unit.Value);
         }

        public static async Task<Result<List<ContentTagDto>>> GetContentTags(this DataContext context, Guid contentId)
        {
            var output = new List<ContentTagDto>();
            var tags = await context.ContentTags.Where(tag => tag.ContentId == contentId).ToListAsync();
            if (tags == null)
                return Result<List<ContentTagDto>>.Failure("No matching tags found");
            foreach(var tag in tags)
            {
                output.Add(new ContentTagDto
                {
                    ContentId = contentId,
                    TagValue = tag.TagValue
                });
            }
            return Result<List<ContentTagDto>>.Success(output);
        }

        public static async Task<Result<List<ContentMetadataDto>>> GetContentsWithTag(this DataContext context, string tagValue)
        {
            var contents = await context.ContentTags
            .Include(u => u.Content)
            .Where(u => u.TagValue == tagValue)
            .ToListAsync();
            if (contents == null)
                return Result<List<ContentMetadataDto>>.Failure("Could not find matching tags");
            var dict = new Dictionary<string, ContentMetadataDto>();
            foreach(var tag in contents)
            {
                var dto = new ContentMetadataDto 
                {
                    VideoUrl = tag.Content.VideoUrl ,
                    AudioUrl = tag.Content.AudioUrl,
                    ContentType = tag.Content.ContentType,
                    ContentName = tag.Content.ContentName,
                    Language = tag.Content.Language,
                    ContentUrl = tag.Content.ContentUrl,
                    ContentId = tag.ContentId
                };
                dict[tag.TagValue] = dto;
            }
            var list = dict.Values.ToList();
            return Result<List<ContentMetadataDto>>.Success(list);
        }

        public static async Task<Result<DomainDTOs.ContentMetadataDto>> GetContentAtUrl(this DataContext context, string url, IMapper mapper)
        {
            var content = await context.Contents.FirstOrDefaultAsync(c => c.ContentUrl == url);
            
            if (content == null)
                return Result<DomainDTOs.ContentMetadataDto>.Failure("Could not load content");
            var value = mapper.Map<DomainDTOs.ContentMetadataDto>(content);
            return Result<DomainDTOs.ContentMetadataDto>.Success(value);
        }

        public static async Task<Result<KnownWordsDto>> GetKnownWords(this DataContext context, Guid contentId, string username)
        {
            //TODO 
            var allTerms = await context.UserTerms
            .Include(u => u.UserLanguageProfile)
            .ThenInclude(t => t.User)
            .Where(u => u.UserLanguageProfile.User.UserName == username)
            .ToListAsync();
            int total = 0;
            int known = 0;
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