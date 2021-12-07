using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.Content;
using Application.Interfaces;
using Application.Parsing;
using MediatR;

namespace Application.DataObjectHandling.Parse
{
    public class GetSection
    {
        public class Query : IRequest<Result<ContentSection>>
        {
            public SectionQueryDto Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ContentSection>>
        {
        private readonly IParserService _parser;
            public Handler(IParserService parser)
            {
            this._parser = parser;
            }

            public async Task<Result<ContentSection>> Handle(Query request, CancellationToken cancellationToken)
            {
                var section = await _parser.GetSection(request.Dto.ContentUrl, request.Dto.Index);
                if (section == null)
                    return Result<ContentSection>.Failure("Could not load section");
                return Result<ContentSection>.Success(section);
            }
        }
    }
}