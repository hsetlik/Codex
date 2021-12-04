using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using Application.DomainDTOs.Content;
using Application.Interfaces;
using MediatR;

namespace Application.DataObjectHandling.Parse
{
    public class GetContentMetadata
    {
        public class Query : IRequest<Result<ContentMetadataDto>>
        {
            public ContentUrlDto Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ContentMetadataDto>>
        {
        private readonly IParserService _parser;
            public Handler(IParserService parser)
            {
            this._parser = parser;
            }

            public async Task<Result<ContentMetadataDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var metadata = await _parser.GetContentMetadata(request.Dto.ContentUrl);
                if (metadata == null)
                    return Result<ContentMetadataDto>.Failure("could not create metadata");
                return Result<ContentMetadataDto>.Success(metadata);
            }
        }
    }
}