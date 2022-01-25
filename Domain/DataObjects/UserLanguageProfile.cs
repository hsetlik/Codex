using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DataObjects
{
    public class UserLanguageProfile
    {
        [Key]
        public Guid LanguageProfileId { get; set; }
        public string UserId { get; set; } // Foreign key
        public CodexUser User { get; set; } // Navigation property so that EF can map: one User -> many UserLanguageProfile
        public string Language { get; set; }
        public string UserLanguage { get; set; }
        //Naviagtion Property
        public ICollection<UserTerm> UserTerms { get; set; } = new List<UserTerm>(); // EF will automatically configure a one-to-many relationship
        public ICollection<Phrase> Phrases { get; set; } = new List<Phrase>();
        public int KnownWords { get; set; }
        public ICollection<ContentHistory> ContentHistories { get; set; } = new List<ContentHistory>();
        public DailyProfileHistory DailyProfileHistory { get; set; } = new DailyProfileHistory();
        public ICollection<SavedContent> SavedContents { get; set; } = new List<SavedContent>();
        public ICollection<SavedCollection> SavedCollections { get; set; } = new List<SavedCollection>();
        public ICollection<Content> CreatedContents { get; set; } = new List<Content>();
        public ICollection<ContentDifficulty> ContentDifficulties { get; set; } = new List<ContentDifficulty>();
    }
}