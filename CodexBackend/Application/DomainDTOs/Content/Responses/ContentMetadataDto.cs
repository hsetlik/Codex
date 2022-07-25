using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs
{
    public class ContentMetadataDto
        {
            public Guid ContentId { get; set; }
            public string ContentUrl { get; set; }
            public Guid LanguageProfileId { get; set; }
            public string Description { get; set; }
            public string CreatorUsername { get; set; }
            public string VideoId { get; set; }
            public string AudioUrl { get; set; }
            public string ContentType { get; set; }
            public string ContentName { get; set; }
            public string Language { get; set; }
            public int Bookmark { get; set; }
            public int NumSections { get; set; }
            public List<string> ContentTags { get; set; }
        }
}