using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DataObjects
{
    //basic class to hold translations
    public class Translation
    {
        public Guid TermId { get; set; }

        [Key]
        public string Value { get; set; }
    }
    public class UserTerm
    {
        public Guid UserTermId { get; set; }
        public Guid TermId { get; set; }
        public Term Term { get; set; }
        public string Username { get; set; }
        public UserLanguageProfile UserLanguageProfile { get; set; }
        public ICollection<Translation> Translations { get; set; } = new List<Translation>();
        public int TimesSeen { get; set; }
        public int KnowledgeLevel { get; set; }
        public float EaseFactor { get; set; }
        public int SrsInterval { get; set; }
    }
}