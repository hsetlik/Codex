using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DataObjects
{
    public class ContentHistory
    {
        [Key]
        public Guid ContentHistoryId { get; set; }
        //Related entity: userLanguageProfile
        public UserLanguageProfile UserLanguageProfile { get; set; }
        public Guid LanguageProfileId { get; set; }
        //Columns
        public ICollection<ContentViewRecord> ContentViewRecords { get; set; } = new List<ContentViewRecord>();
        public string ContentUrl { get; set; }

    }
}