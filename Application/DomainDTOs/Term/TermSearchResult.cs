using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Parsing;

namespace Application.DomainDTOs.Term
{
    public class TermSearchResult
    {
        public string QueryValue { get; set; }
        public string Language { get; set; }
        public ContentSection ContentSection { get; set; }
        public int MatchElementIndex { get; set; }
    }
}