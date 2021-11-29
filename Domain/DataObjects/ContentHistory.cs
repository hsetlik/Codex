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
        public List<ContentViewRecord> ContentViewRecords { get; set; }

    }
}