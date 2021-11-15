using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DataObjects
{
    public class Term
    {
        public string NormalizedValue { get; set; } //the actual word
        public string Language { get; set; } //languages are encoded as strings as per ISO 169-1
        //Primary key & navigation property for UserTerm
        public Guid TermId { get; set; }
    }
}