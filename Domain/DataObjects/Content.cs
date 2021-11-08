using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DataObjects
{
    public class Content
    {
        [Key]
        public Guid ContentId { get; set; }
        public string VideoUrl { get; set; }
        public string AudioUrl { get; set; }
        public string ContentType { get; set; }
        public string ContentName { get; set; }
        public string Language { get; set; }
        public string DateAdded { get; set; }
        public Transcript Transcript { get; set; }
    }
}