using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DataObjects;

namespace Application.DomainDTOs
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
        public PhraseDto()
        {

        }
        public PhraseDto(Phrase phrase)
        {
            this.PhraseId = phrase.PhraseId;
            this.LanguageProfileId = phrase.LanguageProfileId;
            this.Language = phrase.UserLanguageProfile.Language;
            this.Value = phrase.Value;
            this.TimesSeen = phrase.TimesSeen;
            this.EaseFactor = phrase.EaseFactor;
            this.Rating = phrase.Rating;
            this.DateTimeDue = phrase.DateTimeDue;
            this.CreatedAt = phrase.CreatedAt;
            this.Translations = new List<string>();

            foreach(var translation in phrase.Translations)
            {
                this.Translations.Add(translation.Value);
            }
        }
    }
}