using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.Content
{
    public class ContentTagQuery
    {
        public Guid ContentId { get; set; }
        public string TagValue { get; set; }
    }
}