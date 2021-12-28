using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs
{
    public class AddTranslationQuery
    {
        public Guid UserTermId { get; set; }
        public string NewTranslation { get; set; }
    }
}