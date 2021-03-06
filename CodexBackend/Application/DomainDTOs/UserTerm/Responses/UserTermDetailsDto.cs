using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DataObjectHandling.UserTerms
{
    public class UserTermDetailsDto
    {
        public string NormalizedTermValue { get; set; }
        public int TimesSeen { get; set; }
        public float EaseFactor { get; set; }
        public int Rating { get; set; }
        public DateTime DateTimeDue { get; set; }
        public float SrsIntervalDays { get; set; } 
        public Guid UserTermId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Starred { get; set; }
    }
}