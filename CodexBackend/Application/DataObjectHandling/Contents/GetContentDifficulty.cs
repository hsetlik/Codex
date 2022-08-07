using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.Content.Queries;
using Application.DomainDTOs.Content.Responses;
using Application.Extensions;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Persistence;

namespace Application.DataObjectHandling.Contents
{
    public class GetContentDifficulty
    {
        public class Query : IRequest<Result<ContentDifficultyDto>>
        {
            public ContentDifficultyQuery Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ContentDifficultyDto>>
        {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;
        private readonly IParserService _parser;
            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor, IParserService parser)
            {
            this._parser = parser;
            this._userAccessor = userAccessor;
            this._mapper = mapper;
            this._context = context;
            }

            public async Task<Result<ContentDifficultyDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.GetContentDifficulty(request.Dto.LanguageProfileId, request.Dto.ContentId, _mapper, _parser, _userAccessor);
            }
        }
    }
}