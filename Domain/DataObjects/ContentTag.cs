using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DataObjects
{
    public class ContentTag
    {
        [Key]
        public Guid ContentTagId { get; set; }
        public Content Content { get; set; }
        public Guid ContentId { get; set; }
        public string TagValue { get; set; }
    }
}