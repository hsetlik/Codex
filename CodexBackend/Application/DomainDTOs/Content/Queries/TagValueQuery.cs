using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.Content
{
    public class ContentTagQuery
    {
        public string TagValue { get; set; }
        public string TagLanguage { get; set; }
        public string ContentLanguage { get; set; }
    }
}