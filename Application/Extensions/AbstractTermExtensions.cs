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
        

        public static async Task<Result<AbstractTermDto>> AbstractTermFor(this DataContext context, TermDto dto, string username)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Console.WriteLine($"");
            // 1. Get the term
            var normValue = dto.Value.AsTermValue();
            var term = await context.Terms.FirstOrDefaultAsync(
                t => t.Language == dto.Language &&
                t.NormalizedValue == normValue);
            if (term == null)
            {
                //if the term doesn't exist yet, just create it
                var createTermResult = await context.CreateTerm(dto.Language, dto.Value);
                if (!createTermResult.IsSuccess)
                    return Result<AbstractTermDto>.Failure($"Could not create term! Error message: {createTermResult.Error}");
                term = await context.Terms.FirstOrDefaultAsync(
                t => t.Language == dto.Language &&
                t.NormalizedValue == normValue);
                if (term == null)
                    return Result<AbstractTermDto>.Failure($"Could not find created term!");
            }
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