using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.Terms;

namespace Application.DataObjectHandling.UserTerms
{
    public class UserTermCreateQuery
    {
        // User info not necessary since we have IUserAccessor
        //Use this as a key to find the appropriate UserLanguageProfile per ISO-169-1
        public string Language { get; set; }
        // Use this to retreive the Guid from the Terms table
        public string TermValue { get; set; }
        public string FirstTranslation { get; set; }
      
    }

}