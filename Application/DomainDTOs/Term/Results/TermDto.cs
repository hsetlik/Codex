using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DataObjectHandling.Terms
{
    public class TermDto
    {
        public string Value { get; set; }
        public string Language { get; set; }
    }

    public class RawTermDto : TermDto
    {

    }

    public class UserTermDto : TermDto
    {
        public float EaseFactor { get; set; }
        public float SrsIntervalDays { get; set; }
        public int Rating  { get; set; }
        public List<string> Translations { get; set; }
        public Guid UserTermId { get; set; }
        public int TimesSeen { get; set; }
        public bool Starred { get; set; }
    }

    
}