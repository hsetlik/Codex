using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DataObjects
{
    public class UserTerm
    {
        public Guid TermId { get; set; }
        public string Username { get; set; }
        public ICollection<string> Translations { get; set; }
        public int TimesSeen { get; set; }
        public int KnowledgeLevel { get; set; }
        public float EaseFactor { get; set; }
        public int SrsInterval { get; set; }
    }
}