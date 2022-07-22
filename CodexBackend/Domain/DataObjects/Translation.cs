using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DataObjects
{ public class Translation
    {
        [Key]
        public Guid TranslationId { get; set; }
        public string TermValue { get; set; } //this is just = UserTerm.NormalizedTermValue
        public string TermLanguage { get; set; }
        public string UserValue { get; set; } // the value of the translation
        public string UserLanguage { get; set; } // the user's native language
        //Foreign key
        public Guid UserTermId { get; set; }
        public UserTerm UserTerm { get; set; }
    }
}