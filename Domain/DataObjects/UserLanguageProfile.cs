using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DataObjects
{
    public class UserLanguageProfile
    {
        public string Username { get; set; }
        public CodexUser User { get; set; }
        public string Language { get; set; }
        public ICollection<UserTerm> UserTerms { get; set; }
    }
}