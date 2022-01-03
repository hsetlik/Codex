using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.Content.Responses
{
    public class SavedContentDto
    {
        public Guid SavedContentId { get; set; }
        public DateTime SavedAt { get; set; }
        public string ContentUrl { get; set; }
        //foreign key
        public Guid LanguageProfileId { get; set; }
    }
}