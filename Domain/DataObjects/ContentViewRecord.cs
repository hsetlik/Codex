using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DataObjects
{
    public class ContentViewRecord
    {
        [Key]
        public Guid ContentViewRecordId { get; set; }
        public Guid ContentHistoryId { get; set; } // foreign key
        public ContentHistory ContentHistory { get; set; }
        public string ContentUrl { get; set; } //which content this refers to
        public DateTime AccessedOn { get; set; }
    }
}