using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DataObjects
{
    public class ProfileFollowing
    {
        public Guid ProfileFollowingId { get; set; }
        public Guid LanguageProfileId { get; set; }
        public UserLanguageProfile UserLanguageProfile { get; set; }

        public Guid FollowingProfileId { get; set; }
        public UserLanguageProfile FollowingProfile { get; set; }
    }
}