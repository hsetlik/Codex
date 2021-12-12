using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DataObjects
{
    //basic class to hold translations
    public class Translation
    {
        [Key]
        public Guid TranslationId { get; set; }
        public string Value { get; set; }
        //Foreign key
        public Guid UserTermId { get; set; }
        public UserTerm UserTerm { get; set; }
    }
    
    public class UserTerm
    {
        [Key]
        public Guid UserTermId { get; set; }
        //Navigation properties
        public Guid LanguageProfileId { get; set; }
        public UserLanguageProfile UserLanguageProfile { get; set; }
        // second pair of nav. properties to link the Term entity
        public Guid TermId { get; set; }
        public Term Term { get; set; }
        public string NormalizedTermValue { get; set; }
        //ICollection to create the translations
        public ICollection<Translation> Translations { get; set; } = new List<Translation>();
        //Actual UserTerm specific data
        public int TimesSeen { get; set; }
        public float EaseFactor { get; set; }
        public int Rating { get; set; }
        public string DateTimeDue { get; set; }
        public float SrsIntervalDays { get; set; }
    }
}