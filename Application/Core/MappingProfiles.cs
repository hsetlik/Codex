using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.Terms;
using Application.DataObjectHandling.UserTerms;
using Application.DomainDTOs;
using Application.DomainDTOs.Content;
using Application.DomainDTOs.Content.Responses;
using Application.DomainDTOs.ContentCollection.Responses;
using Application.DomainDTOs.UserLanguageProfile;
using Application.DomainDTOs.UserTerm;
using Application.Extensions;
using Application.Parsing;
using AutoMapper;
using Domain;
using Domain.DataObjects;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Content, ContentMetadataDto>();
            CreateMap<UserTerm, UserTermDetailsDto>();
            CreateMap<Translation, TranslationDto>();
            CreateMap<ContentSection, ContentSectionDto>();
            CreateMap<UserLanguageProfile, LanguageProfileDto>();
            CreateMap<UserTerm, AbstractTermDto>();
            CreateMap<SavedContent, SavedContentDto>();
            CreateMap<ContentCollectionEntry, ContentCollectionEntryDto>()
            .ForMember(c => c.ContentMetadataDto, r => r.MapFrom(s => s.Content));
            CreateMap<ContentCollection, ContentCollectionDto>()
            .ForMember(c => c.CollectionContents, r => r.MapFrom(s => s.Entries.Select(e => e.Content)));
            CreateMap<ContentCollectionDto, ContentCollection>();
        }
    }

    public static class MapperFactory
    {
        public static IMapper GetDefaultMapper()
        {
            var config = new MapperConfiguration(cfg => 
            {
                cfg.AddProfile<MappingProfiles>();
            });
            return new Mapper(config);
        }
    }
}