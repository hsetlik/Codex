using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using Application.Extensions;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Persistence;

namespace Application.DataObjectHandling.Contents
{
    public class GetLanguageContents
    {
        public class Query : IRequest<Result<List<ContentMetadataDto>>>
        {
            public LanguageNameQuery Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<ContentMetadataDto>>>
        {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;
            public Handler(DataContext context, IUserAccessor userAccessor, IMapper mapper  )
            {
            this._mapper = mapper;
            this._userAccessor = userAccessor;
            this._context = context;
            }

            public async Task<Result<List<ContentMetadataDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var output = await _context.GetContentsForLanguage(_userAccessor.GetUsername(), request.Dto.Language, _mapper);
                if (!output.IsSuccess)
                    return Result<List<ContentMetadataDto>>.Failure($"Failed to get language contents! Error Message: {output.Error}");
                return Result<List<ContentMetadataDto>>.Success(output.Value);
            }
        }
    }
}