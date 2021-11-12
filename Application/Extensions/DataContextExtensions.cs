using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DataObjectHandling.Terms;
using Domain.DataObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Extensions
{
    public static class DataContextExtensions
    {
        public static async Task<Result<Unit>> CreateTerm(this DataContext context, string Language, string TermValue)
        {
            var term = await context.Terms.FirstOrDefaultAsync(x => x.Language == Language && x.Value == TermValue);
            if (term != null)
            {
                Console.WriteLine("TERM VALUE: " + term.Value + " ALREADY EXISTS" );
                return Result<Unit>.Success(Unit.Value);
            } 

            var newTerm = new Term
            {
                Language = Language,
                Value = TermValue
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
            var term = await context.Terms.FirstOrDefaultAsync(u => u.Language == dto.Language && u.Value == dto.Value);
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
                DateTimeDue = DateTime.Today.ToString()
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
            var term = await _context.Terms
                .FirstOrDefaultAsync(
                    x => x.Language == dto.Language && 
                    x.Value == dto.Value);
                if (term == null) return Result<AbstractTermDto>.Failure("No valid term found!");
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
                if (user == null) return Result<AbstractTermDto>.Failure("User not found!");
                TermDto termDto;
                var userTerm = await _context.UserTerms
                .Include(u => u.Translations)
                .Include(u => u.UserLanguageProfile)
                .FirstOrDefaultAsync(x => x.TermId == term.TermId && x.UserLanguageProfile.UserId == user.Id);
                //whether the userterm exists determines which subclass is created
                if (userTerm != null)
                {
                    //we have a userterm, so we create the UserTermDto subclass
                    var translations = new List<string>();
                    foreach(var t in userTerm.Translations) //NOTE: there's definitely a better C#-ish way to do this
                    {
                        translations.Add(t.Value);
                    }
                    termDto = new UserTermDto
                    {
                        Value = term.Value,
                        Language = term.Language,
                        EaseFactor = userTerm.EaseFactor,
                        SrsIntervalDays = userTerm.SrsIntervalDays,
                        Rating = userTerm.Rating,
                        Translations = translations
                    };
                }
                else
                {
                    termDto = new RawTermDto
                    {
                        Value = term.Value,
                        Language = term.Language
                    };
                }
                var aTermDto = AbstractTermFactory.Generate(termDto);
                return Result<AbstractTermDto>.Success(aTermDto);
        }


        public static async Task<Result<List<AbstractTermDto>>> AbstractTermsFor(this DataContext context, Guid transcriptChunkId, string username)
        {
            var output = new List<AbstractTermDto>();
            var chunk = await context.TranscriptChunks
            .Include(u => u.Transcript)
            .FirstOrDefaultAsync(x => x.TranscriptChunkId == transcriptChunkId);

            var chunkWords = chunk.ChunkText.Split(' ');
            foreach (var word in chunkWords)
            {
                var tDto = new TermDto {
                    Language = chunk.Language,
                    Value = word
                };
                var result = await context.AbstractTermFor(tDto, username);
                output.Add(result.Value);
            }
            return Result<List<AbstractTermDto>>.Success(output);
        }

        public static async Task<Result<Unit>> UpdateTermAbstract(this DataContext context, AbstractTermDto dto, string username)
        {
            var exisitngTerm = await context.Terms
            .FirstOrDefaultAsync(t => t.Language == dto.Language && 
            t.Value == dto.TermValue);
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
            t.Value == dto.TermValue); 

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
                        return Result<Unit>.Failure("UserTerm was not created"); 
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
        
    }
}