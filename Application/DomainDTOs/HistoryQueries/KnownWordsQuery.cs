using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.HistoryQueries
{
    public class KnownWordsQuery
    {
        public int NumDays { get; set; }
        public Guid UserLanguageProfileId { get; set; }
    }
}