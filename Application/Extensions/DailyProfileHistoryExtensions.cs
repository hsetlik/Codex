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
            var records = await context.DailyProfileRecords
            .Where(rec => rec.LanguageProfileId == langProfileId)
            .ToListAsync();
            if (records == null)
                return Result<DailyProfileRecord>.Failure($"Could not get record for profile {langProfileId} at time: {time}");
            records = records.OrderByDescending(rec => Math.Abs((time - rec.CreatedAt).Milliseconds)).ToList();
            return Result<DailyProfileRecord>.Success(records.First());
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


        public static async Task<Result<Unit>> UpdateHistory(this DataContext context, ProfileIdDto dto)
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


    }
}