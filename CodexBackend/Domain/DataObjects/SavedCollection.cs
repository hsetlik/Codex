using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DataObjects
{
    //this is for a many-to-many w/ UserLanguageProfile and Collection
    public class SavedCollection
    {
        public Guid LanguageProfileId { get; set; }
        public UserLanguageProfile UserLanguageProfile { get; set; }
        public Guid CollectionId { get; set; }
        public Collection Collection { get; set; }
        public DateTime SavedAt { get; set; }
        public bool IsOwner { get; set; }
        
    }
}