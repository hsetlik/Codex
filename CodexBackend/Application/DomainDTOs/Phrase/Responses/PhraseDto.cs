using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DataObjects;

namespace Application.DomainDTOs.Phrase
{
    public class PhraseDto
    {
        public Guid PhraseId { get; set; }
        public Guid LanguageProfileId { get; set; }
        public string Language { get; set; }
        public string Value { get; set; }
        public List<string> Translations { get; set; }
        public int TimesSeen { get; set; }
        public float EaseFactor { get; set; }
        public int Rating { get; set; }
        public string DateTimeDue { get; set; }
        public float SrsIntervalDays { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}