using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.ProfileHistory;
using Application.DomainDTOs.ProfileHistory.DailyData;
using Application.Extensions;
using Application.Interfaces;
using Persistence;

namespace Application.ProfileHistoryEngine
{
    public class ProfileHistoryEngine : IProfileHistoryEngine
    {
        public async Task<Result<DailyDataPoint>> GetDataPoint(DataPointQuery query, DataContext context)
        {
            return await context.GetDataPoint(query);
        }

        public async Task<Result<MetricGraph>> GetMetricGraph(MetricGraphQuery query, DataContext context)
        {
            return await context.GetMetricGraph(query);
        }
    }
}