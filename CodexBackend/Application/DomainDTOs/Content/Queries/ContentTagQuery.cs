using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.Content
{
    public class ContentTagDto
    {
        public Guid ContentId { get; set; }
        public string TagValue { get; set; }
        public string TagLanguage { get; set; }
    }
}