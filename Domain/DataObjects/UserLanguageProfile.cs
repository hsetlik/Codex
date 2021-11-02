using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DataObjects
{
    public class UserLanguageProfile
    {
        public string Username { get; set; } // Foreign key
        public CodexUser User { get; set; } // Navigation property so that EF can map: one User -> many UserLanguageProfile
        public string Language { get; set; }
        public ICollection<UserTerm> UserTerms { get; set; } // EF will automatically configure a one-to-many relationship
    }
}