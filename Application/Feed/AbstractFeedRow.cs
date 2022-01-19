using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using Application.Feed.FeedRows;
using AutoMapper;
using Persistence;

namespace Application.Feed
{
    public abstract class AbstractFeedRow
    {
        public Guid LanguageProfileId { get {return languageProfileId;} }
        protected Guid languageProfileId;
        public AbstractFeedRow(Guid id)
        {
            languageProfileId = id;
        }
        public abstract Task<Result<List<ContentMetadataDto>>> GetContents(DataContext context, int max, IMapper mapper);
    }

    public static class RowFactory
    {
        public static AbstractFeedRow RowFor(FeedType type, Guid profileId, int max=5)
        {
            switch (type)
            {
                case FeedType.Newest:
                    return new NewestRow(profileId);
                case FeedType.RecentlyViewed:
                    return new RecentlyViewedRow(profileId);
                case FeedType.MostViewed:
                    return new MostViewedRow(profileId);
            }
            return null;
        }
    }
}