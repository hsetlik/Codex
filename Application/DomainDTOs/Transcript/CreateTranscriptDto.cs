using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs
{
       public class CreateTranscriptDto
    {
        public string Language { get; set; }
        public string FullText { get; set; }
    }
}