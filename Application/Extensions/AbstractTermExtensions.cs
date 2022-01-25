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
using Application.DomainDTOs.UserLanguageProfile;
using Application.DomainDTOs.UserTerm.Queries;
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
    using TaskMap =  Dictionary<int, Task<Result<AbstractTermDto>>>;
    public static class AbstractTermExtensions
    {
        
        public static async Task<Result<AbstractTermDto>> AbstractTermFor(this DataContext context, TermDto dto, string username)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            // 1. Get the term
            var normValue = dto.Value.AsTermValue();
            // 2. Get the UserLanguageProfile
            var profile = await context.UserLanguageProfiles
            .Include(p => p.User)
            .FirstOrDefaultAsync(t => t.Language == dto.Language && t.User.UserName == username);
            // 3. Check for a UserTerm
            var userTerm = await context.UserTerms
            .Include(t => t.Translations)
            .FirstOrDefaultAsync(u => u.NormalizedTermValue == normValue &&
             u.LanguageProfileId == profile.LanguageProfileId);
             AbstractTermDto output;
             var trailing = StringUtilityMethods.GetTrailing(dto.Value);
             if (userTerm != null) 
             {
                 //HasUserTerm = true
                 output = new AbstractTermDto
                 {
                    Language = dto.Language,
                    HasUserTerm = true,
                    EaseFactor = userTerm.EaseFactor,
                    SrsIntervalDays = userTerm.SrsIntervalDays,
                    Rating = userTerm.Rating,
                    Translations = userTerm.Translations.Select(r => r.UserValue).ToList(),
                    UserTermId = userTerm.UserTermId,
                    TimesSeen = userTerm.TimesSeen,
                    Starred = userTerm.Starred
                 };
             }
             else
             {
                 output = new AbstractTermDto
                 {
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
            if (dto.HasUserTerm)
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == username);
                var exisitngUserTerm = await context.UserTerms
                .FirstOrDefaultAsync(u => u.NormalizedTermValue == dto.TermValue.AsTermValue() &&
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
        public static async Task<Result<ElementAbstractTerms>> GetAbstractTermsForElement(this DataContext context, IUserAccessor userAccessor, IDataRepository factory, ElementAbstractTermsQuery query)
        {
            var terms = new List<AbstractTermDto>();
            string text = query.ElementText.WithoutSquareBrackets();
            //Console.WriteLine($"TEXT IS: {text}");
            var words = text.Split(null).ToList();
            words = words.TakeWhile(w => Regex.IsMatch(w, @"[^\s+]")).ToList();
            var wordDict = new Dictionary<int, string>();
            for(int i = 0; i < words.Count; ++i)
            {
                wordDict[i] = words[i];
            }
            var taskMap = new TaskMap();

            Parallel.ForEach(wordDict, word => 
            {
                taskMap[word.Key] = factory.GetAbstractTerm(new TermDto{Value = word.Value, Language = query.Language}, userAccessor.GetUsername());
            });
            foreach(var t in taskMap)
            {
                var term = await t.Value;
                if (term.IsSuccess)
                {
                    term.Value.IndexInChunk = t.Key;
                    terms.Add(term.Value);
                }
            }
            terms = terms.OrderBy(t => t.IndexInChunk).ToList();
            var output = new ElementAbstractTerms
                {
                ElementText = query.ElementText,
                Tag = query.Tag,
                AbstractTerms = terms
                };
            return Result<ElementAbstractTerms>.Success(output);
        }
    }
}