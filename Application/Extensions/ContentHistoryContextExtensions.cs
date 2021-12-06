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
    public static class ContentHistoryContextExtensions
    {
        public static async Task<Result<ContentHistory>> GetHistoryFor(this DataContext context, string contentUrl, string username)
        { 
            var history = await context.ContentHistories
            .Include(h => h.UserLanguageProfile)
            .ThenInclude(h => h.User)
            .FirstOrDefaultAsync(h => h.ContentUrl == contentUrl &&
            h.UserLanguageProfile.User.UserName == username);
            if (history == null)
                return Result<ContentHistory>.Failure($"No content history found for content {contentUrl} for user {username}");
            return Result<ContentHistory>.Success(history);
        }

        public static async Task<Result<Unit>> AddContentView(this DataContext context, string contentUrl, string username, IParserService parser)
        {
            var existingHistory = await context.GetHistoryFor(contentUrl, username);
            while (!existingHistory.IsSuccess)
            {
                //create a history object on the database
                existingHistory = await context.CreateContentHistory(contentUrl, username, parser);
            }

            var record = new ContentViewRecord
            {
                ContentHistory = existingHistory.Value,
                ContentHistoryId = existingHistory.Value.ContentHistoryId,
                ContentUrl = contentUrl,
                AccessedOn = DateTime.Now
            };

            context.ContentViewRecords.Add(record);
            var success = await context.SaveChangesAsync() > 0;
            if (!success)
                return Result<Unit>.Failure("Could not update database");
            return Result<Unit>.Success(Unit.Value);
        }

        public static async Task<Result<ContentHistory>> CreateContentHistory(this DataContext context, 
        string contentUrl, 
        string username, 
        IParserService parser)
        {
            var existingHistory = await context.ContentHistories
            .Include(c => c.UserLanguageProfile)
            .ThenInclude(c => c.User)
            .FirstOrDefaultAsync(c => c.ContentUrl == contentUrl &&
            c.UserLanguageProfile.User.UserName == username);
            if (existingHistory != null)
            {
                return Result<ContentHistory>.Failure("History already exists");
            }
            // 1. Find the language of the content
            var contentMetadata = await parser.GetContentMetadata(contentUrl);
            if (contentMetadata == null)
                return Result<ContentHistory>.Failure("Could not retreive content metadata");
            // 2. Find the matching ULP
            var profile = await context.UserLanguageProfiles
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Language == contentMetadata.Language && 
            p.User.UserName == username);
            if (profile == null)
                return Result<ContentHistory>.Failure("No matching profile!");
            
            var history = new ContentHistory
            {
                UserLanguageProfile = profile,
                LanguageProfileId = profile.LanguageProfileId,
                ContentViewRecords = new List<ContentViewRecord>(),
                ContentUrl = contentUrl
            };

            context.ContentHistories.Add(history);
            var success = await context.SaveChangesAsync() > 0;
            if (!success)
                return Result<ContentHistory>.Failure("Could not update database");
            return Result<ContentHistory>.Success(history);            
        }
  
    }
}