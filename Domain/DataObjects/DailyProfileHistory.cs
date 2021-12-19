using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DataObjects
{
    public class DailyProfileRecord
    {
        [Key]
        public Guid DailyProfileRecordId { get; set; } // Primary key
        public Guid DailyProfileHistoryId { get; set; } // Foreign key
        public DailyProfileHistory DailyProfileHistory { get; set; } // Nav property
        public Guid LanguageProfileId { get; set; } // second primary key
        public UserLanguageProfile UserLanguageProfile { get; set; } // second nav property
        // Columns
        public DateTime CreatedAt { get; set; }
        public int KnownWords { get; set; }

    }
    public class DailyProfileHistory
    {
        [Key]
        public Guid DailyProfileHistoryId { get; set; }
        public UserLanguageProfile UserLanguageProfile { get; set; }
        public Guid LanguageProfileId { get; set; }
        public ICollection<DailyProfileRecord> DailyProfileRecords { get; set; } = new List<DailyProfileRecord>();
        
    }
}