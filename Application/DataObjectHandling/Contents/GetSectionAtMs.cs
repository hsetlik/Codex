using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.Content.Queries;
using Application.Interfaces;
using Application.Parsing;
using MediatR;

namespace Application.DataObjectHandling.Contents
{
    public class GetSectionAtMs
    {
        public class Query : IRequest<Result<ContentSection>>
        {
            public SectionAtMsQuery Dto { get; set; }
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
                var sections = await _parser.GetAllSections(request.Dto.ContentUrl);
                if (sections == null)
                    return Result<ContentSection>.Failure($"Could not get section for ${request.Dto.ContentUrl}");
                int ms = request.Dto.Ms;
                foreach(var section in sections)
                {
                    int startMs = section.TextElements.First().StartMs;
                    int endMs = section.TextElements.Last().EndMs;
                    if (ms >= startMs && ms < endMs)
                    {
                        return Result<ContentSection>.Success(section);
                    }
                }
                return Result<ContentSection>.Failure($"Could not load section of {request.Dto.ContentUrl} at {request.Dto.Ms} seconds");
            }
        }
    }
}