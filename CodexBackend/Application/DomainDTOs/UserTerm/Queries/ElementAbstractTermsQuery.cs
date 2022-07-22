using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Parsing;

namespace Application.DomainDTOs.UserTerm.Queries
{
    public class ElementAbstractTermsQuery
    {
        public string ContentUrl { get; set; }
        public string ElementText { get; set; }
        public string Tag { get; set; }
        public string Language { get; set; }
    }
}