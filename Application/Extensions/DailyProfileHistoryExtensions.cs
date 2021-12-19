using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Domain.DataObjects;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Extensions
{
    public static class DailyProfileHistoryExtensions
    {
        public static async Task<Result<DailyProfileRecord>> GetClosestRecord(this DataContext context, Guid langProfileId, DateTime time)
        {
            var record = await context.DailyProfileRecords
            .OrderByDescending(r => Math.Abs(time.CompareTo(r)))
            .FirstOrDefaultAsync(r => r.LanguageProfileId == langProfileId);
            if (record == null)
                return Result<DailyProfileRecord>.Failure("Could not load record");
            return Result<DailyProfileRecord>.Success(record);
        }
    }
}