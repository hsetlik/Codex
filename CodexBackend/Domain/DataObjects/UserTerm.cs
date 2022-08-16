using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DataObjects
{
    
    public class UserTerm
    {
        [Key]
        public Guid UserTermId { get; set; }
        //Navigation properties
        public Guid LanguageProfileId { get; set; }
        public UserLanguageProfile UserLanguageProfile { get; set; }
        // second pair of nav. properties to link the Term entity
        public string Language { get; set; }
        public string TermValue { get; set; }
        //ICollection to create the translations
        public ICollection<Translation> Translations { get; set; } = new List<Translation>();
        //Actual UserTerm specific data
        public int TimesSeen { get; set; }
        public float EaseFactor { get; set; }
        public int Rating { get; set; }
        public DateTime DateTimeDue { get; set; }
        public float SrsIntervalDays { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Starred { get; set; }
        public string OwnerUsername { get; set; }
    }
}