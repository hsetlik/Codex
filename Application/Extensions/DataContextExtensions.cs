using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Application.Core;
using Application.DataObjectHandling.Terms;
using Application.DataObjectHandling.Transcripts;
using Application.DataObjectHandling.UserTerms;
using Application.DomainDTOs;
using Application.Utilities;
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
            var normValue = StringUtilityMethods.AsTermValue(termValue); 
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

        public static async Task<Result<AbstractTermDto>> AbstractTermFor(this DataContext _context, TermDto dto, string username)
        {
            var fullString = dto.Value;
            string splitExp = @"([^\p{P}^\s]+)"; 
            var match = Regex.Match(fullString, splitExp);
            if(!match.Success)
                return Result<AbstractTermDto>.Failure("No valid word characers!");
            var wordWithCase = match.Value;
            var user = await _context.Users.Include(u => u.UserLanguageProfiles).FirstOrDefaultAsync(x => x.UserName == username);
                var profile = await _context.UserLanguageProfiles.FirstOrDefaultAsync(
                    x => x.UserId == user.Id &&
                    x.Language == dto.Language);
                if (profile == null) return Result<AbstractTermDto>.Failure("No associated profile found");
                Console.WriteLine("Language profile found");
        var profileId = profile.LanguageProfileId;
        var parsedTerm = StringUtilityMethods.AsTermValue(dto.Value);
        Console.WriteLine($"Parsed term is: {parsedTerm}");
        Console.WriteLine($"Language profile ID is: {profileId}");
        var userTerm = await _context.UserTerms
        .Include(u => u.Term)
        .Include(u => u.Translations)
        .FirstOrDefaultAsync(
                    x => x.LanguageProfileId == profileId &&
                    x.Term.NormalizedValue == parsedTerm);
        if (wordWithCase != fullString)
                Console.WriteLine("Trailing characters found for word: " + wordWithCase);
            var termIdentifier = wordWithCase.ToUpper();
            var term = await _context.Terms
                .FirstOrDefaultAsync(
                    x => x.Language == dto.Language && 
                    x.NormalizedValue == termIdentifier);
            if (term == null) return Result<AbstractTermDto>.Failure("No valid term found for " + dto.Value);
            //whether the userterm exists determines which subclass is created
            TermDto termDto;
            if (userTerm != null)
            {
            //we have a userterm, so we create the UserTermDto subclass
                var translations = new List<string>();
                foreach(var t in userTerm.Translations) //NOTE: there's definitely a better C#-ish way to do this
                {
                    translations.Add(t.Value);
                }
                //NOTE: Value should always be the case-sensitive version of the term w/o punctuation or whitespace, NOT normalized string
                termDto = new UserTermDto
                {
                    Value = wordWithCase,
                    Language = term.Language,
                    EaseFactor = userTerm.EaseFactor,
                    SrsIntervalDays = userTerm.SrsIntervalDays,
                    Rating = userTerm.Rating,
                    Translations = translations,
                    UserTermId = userTerm.UserTermId,
                    TimesSeen = userTerm.TimesSeen
                };
            }
            else
            {
                termDto = new RawTermDto
                {
                    Value = wordWithCase,
                    Language = term.Language
                };
            }
            var aTermDto = AbstractTermFactory.Generate(termDto);
            //don't forget to add trailing characters as necessary before returning
            if (fullString.Length > wordWithCase.Length)
            {
                Console.WriteLine("Trailing characters found");
                aTermDto.TrailingCharacters = fullString.Substring(wordWithCase.Length);
            }
            else
                aTermDto.TrailingCharacters = "";
            return Result<AbstractTermDto>.Success(aTermDto);
    }

        public static async Task<Result<List<AbstractTermDto>>> AbstractTermsFor(this DataContext context, Guid transcriptChunkId, string username)
        {
            var output = new List<AbstractTermDto>();
            var chunk = await context.TranscriptChunks
            .Include(u => u.Transcript)
            .FirstOrDefaultAsync(x => x.TranscriptChunkId == transcriptChunkId);
            if (chunk == null)
                return Result<List<AbstractTermDto>>.Failure("No matching chunk found");
            var chunkWords = chunk.ChunkText.Split(' ');
            for(int i = 0; i < chunkWords.Length; ++i)
            {
                var dto = new TermDto
                {
                    Value = chunkWords[i],
                    Language = chunk.Language
                };
                var aTerm = await context.AbstractTermFor(dto, username);
                //add the index so react can have unique identifiers
                if (aTerm.IsSuccess)
                {
                    aTerm.Value.IndexInChunk = i;
                    output.Add(aTerm.Value);
                }
                    
            }
            return Result<List<AbstractTermDto>>.Success(output);
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
            var content = await context.Contents
                .Include(u => u.Transcript)
                .ThenInclude(t => t.TranscriptChunks)
                .FirstOrDefaultAsync( u => u.ContentId == contentId);
                if (content == null) return Result<Unit>.Failure("Content not found");
                var tChunks = content.Transcript.TranscriptChunks;
                int wordIndex = 0;
                foreach(var chunk in tChunks)
                {
                    string splitExp = @"([^\p{P}^\s]+)"; 
                    var match = Regex.Match(chunk.ChunkText, splitExp);
                    var words = new List<string>();
                    while (match.Success)
                    {
                        Console.WriteLine(match.Value);
                        words.Add(match.Value);
                        match = match.NextMatch();
                    }
                    foreach(var word in words)
                    {
                        var result = await context.CreateTerm(content.Language, word);
                        if (!result.IsSuccess) return Result<Unit>.Failure("Term for " + word + " could not be created at index " + wordIndex.ToString());
                        ++wordIndex;
                    }
                }
                return Result<Unit>.Success(Unit.Value);
        }

        public static async Task<Result<Unit>> CreateContent(this DataContext context, ContentCreateDto dto)
        {
            var transcriptDto = new CreateTranscriptDto
            {
                Language = dto.Language,
                FullText = dto.FullText
            };
            var transcript =  await transcriptDto.CreateTranscriptFrom(context);
            if (!transcript.IsSuccess)
                return Result<Unit>.Failure("Could not create content transcript");
              var content = new Content
            {
                ContentName = dto.ContentName,
                ContentType = dto.ContentType,
                Language = dto.Language,
                DateAdded = DateTime.Now.ToString(),
                VideoUrl = dto.VideoUrl,
                AudioUrl = dto.AudioUrl,
                Transcript = transcript.Value
            };
            context.Contents.Add(content);
            var success = await context.SaveChangesAsync() > 0;
            if (!success)
                return Result<Unit>.Failure("Could not save content object");
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
    }
}