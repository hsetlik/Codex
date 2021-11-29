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
        public UserLanguageProfile UserLanguageProfile { get; set; } //foreign key
        public Guid ContentId { get; set; } //which content this refers to
        public DateTime AccessedOn { get; set; }
    }
}