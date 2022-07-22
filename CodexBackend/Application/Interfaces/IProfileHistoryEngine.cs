using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.ProfileHistory;
using Application.DomainDTOs.ProfileHistory.DailyData;
using Persistence;

namespace Application.Interfaces
{
    public interface IProfileHistoryEngine
    {
        Task<Result<DailyDataPoint>> GetDataPoint(DataPointQuery query, DataContext context);
        Task<Result<MetricGraph>> GetMetricGraph(MetricGraphQuery query, DataContext context);
    }
}