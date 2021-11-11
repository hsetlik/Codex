using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Domain.DataObjects
{
    public class CodexUser : IdentityUser
    {
        public string DisplayName { get; set; }

        public string NativeLanguage { get; set; }

        public ICollection<UserLanguageProfile> UserLanguageProfiles { get; set; } //EF will automatically create a one-to-many relationship
        public string LastStudiedLanguage { get; set; } // just to automatically set up default
        
    }
}