using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Feed
{
    public class Feed
    {
        private Guid _profileId;
        public Feed(Guid profileId)
        {
            _profileId = profileId;
        }
        
    }
}