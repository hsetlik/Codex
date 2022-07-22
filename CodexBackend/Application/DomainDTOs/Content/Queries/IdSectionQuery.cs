using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.Content
{
    public class IdSectionQuery
    {
        public Guid ContentId { get; set; }
        public int Index { get; set; }
    }
}