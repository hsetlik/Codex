using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.ProfileHistory
{
    public class LanguageDateQuery
    {
        public string Language { get; set; }
        public DateQuery DateQuery { get; set; }
    }
}