using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using Application.FeedObjects.FeedRows;
using AutoMapper;
using Persistence;

namespace Application.FeedObjects
{
    public abstract class FeedRowGenerator
    {
        public Guid LanguageProfileId { get {return languageProfileId;} }
        protected Guid languageProfileId;
        public FeedRowGenerator(Guid id)
        {
            languageProfileId = id;
        }
        public abstract Task<Result<List<ContentMetadataDto>>> GetContentList(DataContext context, int max, IMapper mapper);
    }

    public static class RowFactory
    {
        public static FeedRowGenerator RowFor(FeedType type, Guid profileId, int max=5)
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