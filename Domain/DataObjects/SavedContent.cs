using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DataObjects
{
    public class SavedContent
    {
        [Key]
        public Guid SavedContentId { get; set; }

        public DateTime SavedAt { get; set; }
        public string ContentUrl { get; set; }

        //foreign key
        public Guid LanguageProfileId { get; set; }
        public UserLanguageProfile UserLanguageProfile { get; set; }
    }
}