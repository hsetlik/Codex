using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.ContentViewRecord
{
    public class ContentViewRecordDto
    {
        public string Username { get; set; }
        public string ContentUrl { get; set; }
        public DateTime AccessedOn { get; set; }
    }
}