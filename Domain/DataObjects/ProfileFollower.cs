using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DataObjects
{
    public class ProfileFollower
    {
        public Guid ProfileFollowerId { get; set; }
        public Guid LanguageProfileId { get; set; }
        public UserLanguageProfile UserLanguageProfile { get; set; }

        public Guid FollowerProfileId { get; set; }
        public UserLanguageProfile FollowerProfile { get; set; }
    }
}