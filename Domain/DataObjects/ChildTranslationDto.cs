using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DataObjects
{
    public class ChildTranslationDto
    {
        public Guid UserTermId { get; set; }
        public string Value { get; set; }
    }
}