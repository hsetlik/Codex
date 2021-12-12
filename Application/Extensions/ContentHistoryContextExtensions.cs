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
        public static async Task<Result<ContentHistory>> ContentHistoryFor(this DataContext context, string username, string contentUrl)
        {
            var content = await context.Contents.FirstOrDefaultAsync(c => c.ContentUrl == contentUrl);
            if (content == null)
                return Result<ContentHistory>.Failure($"Content not found for {contentUrl}");
            var langProfile = await context.UserLanguageProfiles
            .Include(p => p.User)
            .Include(p => p.ContentHistories)
            .FirstOrDefaultAsync(p => p.Language == content.Language && p.User.UserName == username);
            if (langProfile == null)
                return Result<ContentHistory>.Failure("No matching profile found");
            //check if an appropriate history already exists and return it
            var existingHistory = langProfile.ContentHistories.FirstOrDefault(h => h.ContentUrl == contentUrl);
            if (existingHistory != null)
                return Result<ContentHistory>.Success(existingHistory);
            var history = new ContentHistory
            {
                LanguageProfileId = langProfile.LanguageProfileId,
                UserLanguageProfile = langProfile,
                ContentUrl = content.ContentUrl
            };
            context.ContentHistories.Add(history);
            var success = await context.SaveChangesAsync() > 0;
            if (!success)
                return Result<ContentHistory>.Failure("Could not update database");
            return Result<ContentHistory>.Success(history);
        }

        public static async Task<Result<ContentViewRecord>> LatestRecordFor(this DataContext context, string username, string contentUrl)
        {
            var historyResult = await context.ContentHistoryFor(username, contentUrl);
            if (!historyResult.IsSuccess)
                return Result<ContentViewRecord>.Failure($"Could not get history! Error Message: {historyResult.Error}");
            var records = await context.ContentViewRecords
            .Where(r => r.ContentHistoryId == historyResult.Value.ContentHistoryId)
            .ToListAsync();
            if (records == null || records.Count < 1) 
                return Result<ContentViewRecord>.Failure($"No view records for user {username} at {contentUrl}");
            var newestRecord = records.Aggregate((r1, r2) => r1.AccessedOn > r2.AccessedOn ? r1 : r2);
            return Result<ContentViewRecord>.Success(newestRecord);
        }
  
    }
}