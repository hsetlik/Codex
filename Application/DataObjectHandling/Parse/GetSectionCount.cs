using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.Content;
using Application.Interfaces;
using MediatR;

namespace Application.DataObjectHandling.Parse
{
    public class GetSectionCount
    {
        public class Query : IRequest<Result<int>>
        {
            public ContentUrlDto Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<int>>
        {
        private readonly IParserService _parser;
            public Handler(IParserService parser)
            {
            this._parser = parser;
            }

            public async Task<Result<int>> Handle(Query request, CancellationToken cancellationToken)
            {
                var sections = await _parser.GetNumSections(request.Dto.ContentUrl);
                if (sections < 1)
                    return Result<int>.Failure("No sections found");
                return Result<int>.Success(sections);
            }
        }
    }
}