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
    public class GetSectionAtSeconds
    {
        public class Query : IRequest<Result<ContentSection>>
        {
            public SectionAtSecondsQuery Dto { get; set; }
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
                int seconds = request.Dto.Seconds;
                foreach(var section in sections)
                {
                    int startSeconds = section.TextElements.First().StartSeconds;
                    int endSeconds = section.TextElements.Last().EndSeconds;
                    if (seconds >= startSeconds && seconds < endSeconds)
                    {
                        return Result<ContentSection>.Success(section);
                    }
                }
                return Result<ContentSection>.Failure($"Could not load section of {request.Dto.ContentUrl} at {request.Dto.Seconds} seconds");
            }
        }
    }
}