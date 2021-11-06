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
        public string UserId { get; set; } // Foreign key
        public CodexUser User { get; set; } // Navigation property so that EF can map: one User -> many UserLanguageProfile
        public string Language { get; set; }
        public ICollection<UserTerm> UserTerms { get; set; } = new List<UserTerm>(); // EF will automatically configure a one-to-many relationship
    }
}