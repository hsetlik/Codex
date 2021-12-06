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
using Application.Interfaces;
using Application.Parsing;
using Application.Utilities;
using AutoMapper;
using Domain.DataObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Extensions
{
    public static class AbstractTermExtensions
    {
        
        public static async Task<Result<AbstractTermsFromParagraph>> AbstractTermsForParagraph(
            this DataContext context, 
            string contentUrl,
            int index,
            IParserService parser,
            string username)
            {
                //we need the appropriate LanguageProfileId so we need to get the associated content language,
                //so we can get it from the Content's metadata in the dbContext
                var metadataResult = await parser.GetContentMetadata(contentUrl);
                if (metadataResult == null)
                    return Result<AbstractTermsFromParagraph>.Failure($"Could not load content metadata for URL: {contentUrl}");
                var language = metadataResult.Language;
                var profile = await context.UserLanguageProfiles.Include(u => u.User).FirstOrDefaultAsync(
                    p => p.User.UserName == username &&
                    p.Language == language);
                if (profile == null)
                    return Result<AbstractTermsFromParagraph>.Failure($"Could not load content metadata for URL: {contentUrl}");
                var paragraph = await parser.GetParagraph(contentUrl, index);
                var terms = paragraph.Value.Split(' ');
                var abstractTermTasks = new List<Task<Result<AbstractTermDto>>>();
                for(int i = 0; i < terms.Count(); ++i)
                {
                    var term = terms[i];
                    var abstractTerm = context.AbstractTermFor(term, profile);
                    abstractTermTasks.Add(abstractTerm);
                }
                var abstractTermResults = await Task.WhenAll<Result<AbstractTermDto>>(abstractTermTasks);
                var abstractTerms = new List<AbstractTermDto>();
                for (int i = 0; i < abstractTermResults.Length; ++i)
                {
                    if(!abstractTermResults[i].IsSuccess)
                        return Result<AbstractTermsFromParagraph>.Failure("Could not load term");
                    abstractTermResults[i].Value.IndexInChunk = i;
                    abstractTerms.Add(abstractTermResults[i].Value);
                }
                var output = new AbstractTermsFromParagraph
                {
                    ContentUrl = contentUrl,
                    Index = index,
                    AbstractTerms = abstractTerms
                };
                return Result<AbstractTermsFromParagraph>.Success(output);
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
    }
}