using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DataObjects.DailyProfileMetrics
{
    public class DailyKnownWords : DailyMetricBase
    {
        public Guid DailyKnownWordsId { get; set; }
        public int KnownWords { get; set; }
    }
}