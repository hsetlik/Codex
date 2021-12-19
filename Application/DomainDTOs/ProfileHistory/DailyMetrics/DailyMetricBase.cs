using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistence;

namespace Application.DomainDTOs.ProfileHistory.DailyMetrics
{
     public abstract class DailyMetricBase<T>
    {
        public string MetricType { get {return this.GetType().ToString();} }
        public abstract bool IsPrepared { get; }
        public abstract DateTime DateTime { get; set; }
        public abstract T Value { get;  }
        public abstract Task PrepareAsync(DataContext context, Guid languageProfileId);
        
    }
}