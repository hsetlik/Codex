using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.FeedObjects
{
    public class FeedRow
    {
        public string FeedType { get; set; }
        public List<ContentMetadataDto> Contents { get; set; } = new List<ContentMetadataDto>();
    }
    public class Feed
    {
        public Guid LanguageProfileId { get; set; }
        public List<FeedRow> Rows { get; set; } = new List<FeedRow>();
    }
}