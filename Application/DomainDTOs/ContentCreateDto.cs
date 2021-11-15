using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs
{
    public class ContentCreateDto
        {
            public string ContentName { get; set; }
            public string ContentType { get; set; }
            public string Language { get; set; }
            public string VideoUrl { get; set; }
            public string AudioUrl { get; set; }
            public string FullText { get; set; }
        }
}