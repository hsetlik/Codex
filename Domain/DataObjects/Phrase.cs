using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DataObjects
{
    public class PhraseTranslation
    {
        [Key]
        public Guid TranslationId { get; set; }
        public string Value { get; set; }
        public Guid PhraseId { get; set; }
        public Phrase Phrase { get; set; }
    }
    public class Phrase
    {
        [Key]
        public Guid PhraseId { get; set; }
        //foreign key
        public Guid LanguageProfileId { get; set; }
        public UserLanguageProfile UserLanguageProfile { get; set; }
        public string Value { get; set; }
        // same as UserTerm from here down 
        public ICollection<PhraseTranslation> Translations { get; set; } = new List<PhraseTranslation>();
        public int TimesSeen { get; set; }
        public float EaseFactor { get; set; }
        public int Rating { get; set; }
        public string DateTimeDue { get; set; }
        public float SrsIntervalDays { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}