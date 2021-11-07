using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DataObjectHandling.UserTerms
{
    public class TermCreateDto
    {
        public Guid TermId { get; set; }
        public string FirstTranslation { get; set; }
        public Guid UserTermId { get; set; }
    }
}