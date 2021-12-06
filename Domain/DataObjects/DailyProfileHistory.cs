using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Domain.DataObjects.DailyProfileMetrics;

namespace Domain.DataObjects
{
    public class DailyProfileHistory
    {
        [Key]
        public Guid DailyProfileHistoryId { get; set; }
        public UserLanguageProfile UserLanguageProfile { get; set; }
        public Guid LanguageProfileId { get; set; }
        // Every datapoint which we want to record daily gets its own list
        public ICollection<DailyKnownWords> DailyKnownWords { get; set; } = new List<DailyKnownWords>();
    }
}