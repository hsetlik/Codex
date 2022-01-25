using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.Terms;
using Application.DataObjectHandling.UserTerms;
using Application.DomainDTOs;
using Application.DomainDTOs.Collection.Responses;
using Application.DomainDTOs.Content;
using Application.DomainDTOs.Content.Responses;
using Application.DomainDTOs.Phrase;
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
            CreateMap<Content, ContentMetadataDto>()
            .ForMember(c => c.ContentTags, src => src.MapFrom(m => m.ContentTags.Select(t => t.TagValue)));

            CreateMap<UserTerm, UserTermDetailsDto>();
            CreateMap<Translation, TranslationDto>();
            CreateMap<ContentDifficulty, ContentDifficultyDto>();
            CreateMap<ContentSection, ContentSectionDto>();
            CreateMap<UserLanguageProfile, LanguageProfileDto>()
            .ForMember(c => c.Username, r => r.MapFrom(p => p.User.UserName));
            CreateMap<UserTerm, AbstractTermDto>();
            CreateMap<SavedContent, SavedContentDto>();

            CreateMap<Collection, CollectionDto>()
            .ForMember(m => m.Contents, c => c.MapFrom(s => s.CollectionContents.Select(cc => cc.Content).ToList()))
            .ReverseMap();

            CreateMap<Phrase, PhraseDto>()
            .ForMember(p => p.Translations, c => c.MapFrom(r => r.Translations.Select(t => t.Value)));
            
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