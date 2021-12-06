using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DataObjects.DailyProfileMetrics
{
    public class DailyMetricBase
    {
        public Guid DailyProfileHistoryId { get; set; }
        public DailyProfileHistory DailyProfileHistory { get; set; }
        
        public DateTime Date { get; set; }

    }

}