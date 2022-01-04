using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DataObjects
{
    public class SavedContentCollection
    {
        [Key]
        public Guid SavedContentCollectionId { get; set; }

        public Guid LanguageProfileId { get; set; }
        public UserLanguageProfile UserLanguageProfile { get; set; }

        public Guid ContentCollectionId { get; set; }
        public ContentCollection ContentCollection { get; set; }
        public DateTime SavedAt { get; set; }
    }
}