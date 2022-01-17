using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.ProfileHistory;
using Application.DomainDTOs.ProfileHistory.DailyData;
using Application.DomainDTOs.UserLanguageProfile;
using Domain.DataObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Extensions
{
    public static class DailyProfileHistoryExtensions
    {
        public static async Task<Result<DailyProfileRecord>> GetClosestRecord(this DataContext context, Guid langProfileId, DateTime time)
        {
            var recordOnDay = await context.DailyProfileRecords.FirstOrDefaultAsync(p => p.LanguageProfileId == langProfileId && p.CreatedAt.Date == time.Date);
            if (recordOnDay != null)
            {
                return Result<DailyProfileRecord>.Success(recordOnDay);
            }
            var allRecords = await context.DailyProfileRecords.Where(r => r.LanguageProfileId == langProfileId).ToListAsync();
            if (allRecords == null)
                return Result<DailyProfileRecord>.Failure($"No records found with profile {langProfileId}");
            var closest = allRecords.OrderBy(r => Math.Abs(time.Millisecond - r.CreatedAt.Millisecond)).First();

            return Result<DailyProfileRecord>.Success(closest);
        }

        public static async Task<Result<DailyDataPoint>> GetDataPoint(this DataContext context, DataPointQuery query)
        {
            if (query.MetricName == "KnownWords")
            {
                var record = await context.GetClosestRecord(query.LanguageProfileId, query.DateTime);
                if (!record.IsSuccess)
                    return Result<DailyDataPoint>.Failure($"Could not get record! Error message: {record.Error}");
                var knownWords = record.Value.KnownWords;
                return Result<DailyDataPoint>.Success(new KnownWordsDataPoint(query.LanguageProfileId, query.DateTime, knownWords));
            }
            else if (query.MetricName == "NumUserTerms")
            {
                var numUserTerms = await context.UserTerms.Where(u => u.CreatedAt.CompareTo(query.DateTime) < 1).CountAsync();
                return Result<DailyDataPoint>.Success(new NumUserTermsDataPoint(query.LanguageProfileId, query.DateTime, numUserTerms));
            }
            else if (query.MetricName == "WordsRead")
            {
                var record = await context.GetClosestRecord(query.LanguageProfileId, query.DateTime);
                if (!record.IsSuccess)
                    return Result<DailyDataPoint>.Failure($"Could not get record! Error message: {record.Error}");
                var wordsRead = record.Value.WordsRead;
                return Result<DailyDataPoint>.Success(new WordsReadDataPoint(query.LanguageProfileId, query.DateTime, wordsRead));

            }
            else if (query.MetricName == "SecondsListened")
            {
                var record = await context.GetClosestRecord(query.LanguageProfileId, query.DateTime);
                if (!record.IsSuccess)
                    return Result<DailyDataPoint>.Failure($"Could not get record! Error message: {record.Error}");
                var secondsListened = record.Value.SecondsListened;
                return Result<DailyDataPoint>.Success(new SecondsListenedDataPoint(query.LanguageProfileId, query.DateTime, secondsListened));
            }
            else
            {
                return Result<DailyDataPoint>.Failure($"Could not create data point for metric {query.MetricName}");
            }
        }

        public static async Task<Result<MetricGraph>> GetMetricGraph(this DataContext context, MetricGraphQuery query)
        {
            var points = new List<DailyDataPoint>();
            var queries = query.GetQueries();
            foreach(var q in queries)
            {
                var result = await context.GetDataPoint(q);
                if(!result.IsSuccess)
                    return Result<MetricGraph>.Failure($"Could not load data point! Error message: {result.Error}");
                points.Add(result.Value);
            }
            return Result<MetricGraph>.Success(new MetricGraph
            {
                MetricName = query.MetricName,
                LanguageProfileId = query.LanguageProfileId,
                Start = query.Start,
                End = query.End,
                DataPoints = points
            });
        }


        public static async Task<Result<Unit>> UpdateHistory(this DataContext context, ProfileIdQuery dto)
        {
                var profile = await context.UserLanguageProfiles
                    .Include(p => p.DailyProfileHistory)
                    .FirstOrDefaultAsync(p => p.LanguageProfileId == dto.LanguageProfileId);
                if (profile == null)
                    return Result<Unit>.Failure($"Could not get profile: {dto.LanguageProfileId}");

                // check for an existing record from today
                var existingRecord = await context.DailyProfileRecords
                .FirstOrDefaultAsync(r => r.LanguageProfileId == dto.LanguageProfileId && r.CreatedAt.Date == DateTime.Now.Date);
                // if we find one, remove it from the context
                if(existingRecord != null)
                    context.DailyProfileRecords.Remove(existingRecord);
                var record = new DailyProfileRecord
                {
                    DailyProfileHistoryId = profile.DailyProfileHistory.DailyProfileHistoryId,
                    DailyProfileHistory = profile.DailyProfileHistory,
                    LanguageProfileId = dto.LanguageProfileId,
                    UserLanguageProfile = profile,
                    CreatedAt = DateTime.Now,
                    KnownWords = profile.KnownWords
                };
                context.DailyProfileRecords.Add(record);
                var success = await context.SaveChangesAsync() > 0;
                if (!success)
                    return Result<Unit>.Failure($"Could not save changes!");
                return Result<Unit>.Success(Unit.Value);
        }

        public static async Task<Result<Unit>> AddRecordForDate(this DataContext context, Guid langProfileId, DateTime time)
        {
            var profile = await context.UserLanguageProfiles.FirstOrDefaultAsync(p => p.LanguageProfileId == langProfileId);
            if (profile == null)
                return Result<Unit>.Failure($"Could not get profile with ID {langProfileId}");
            return Result<Unit>.Success(Unit.Value);
        }

        public static async Task<Result<Unit>> AddRecord(this DataContext context, LanguageProfileDto dto, DateTime date)
        {
            var profile = await context.UserLanguageProfiles
                .Include(p => p.DailyProfileHistory)
                .FirstOrDefaultAsync(p => p.LanguageProfileId == dto.LanguageProfileId);
            if (profile == null)
                return Result<Unit>.Failure($"Could not get profile with ID {dto.LanguageProfileId}");
            // check for an existing record from today
            var existingRecord = await context.DailyProfileRecords
            .FirstOrDefaultAsync(r => r.LanguageProfileId == dto.LanguageProfileId && r.CreatedAt.Date == date.Date);
            // if we find one, remove it from the context
            if(existingRecord != null)
                context.DailyProfileRecords.Remove(existingRecord);
            var record = new DailyProfileRecord
            {
                DailyProfileHistoryId = profile.DailyProfileHistory.DailyProfileHistoryId,
                DailyProfileHistory = profile.DailyProfileHistory,
                LanguageProfileId = profile.LanguageProfileId,
                CreatedAt = date,
                KnownWords = dto.KnownWords
            };
            context.DailyProfileRecords.Add(record);

            var success = await context.SaveChangesAsync() > 0;
            if (!success)
                return Result<Unit>.Failure("Could not save changes!");
            return Result<Unit>.Success(Unit.Value);
        }

        public static async Task<Result<Unit>> CreateDummyRecords(this DataContext context, Guid profileId, List<int> knownWordsValues, DateTime start)
        {
            var profile = await context.UserLanguageProfiles.FirstOrDefaultAsync(p => p.LanguageProfileId == profileId);
            var current = start;
            foreach(var wordCount in knownWordsValues)
            {
                var dto = new LanguageProfileDto
                {
                    LanguageProfileId = profile.LanguageProfileId,
                    Language = profile.Language,
                    KnownWords = wordCount
                };
                var addResult = await context.AddRecord(dto, current);
                if (!addResult.IsSuccess)
                    return Result<Unit>.Failure($"Could not add record! Error message: {addResult.Error}");
                current = current.AddDays(1.0f);
            }
            return Result<Unit>.Success(Unit.Value);
        }
    }
}