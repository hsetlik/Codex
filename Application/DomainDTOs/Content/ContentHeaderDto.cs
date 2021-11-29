using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.Content
{
    public class ContentHeaderDto
    {
        public bool HasVideo { get; set; }
        public bool HasAudio { get; set; }
        public string ContentType { get; set; }
        public string ContentName { get; set; }
        public string Language { get; set; }
        public string DateAdded { get; set; }
        public Guid ContentId { get; set; }
    }
}