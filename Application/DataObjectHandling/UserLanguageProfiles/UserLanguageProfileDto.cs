using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DataObjects;

namespace Application.DataObjectHandling.UserLanguageProfiles
{
    public class UserLanguageProfileDto
    {
        //public string UserId { get; set; } // Foreign key
        public string Language { get; set; }
    }
}