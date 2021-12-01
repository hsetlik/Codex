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
    public class GetParagraph
    {
        public class Query : IRequest<Result<ContentParagraph>>
        {
            public ParagraphQueryDto Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ContentParagraph>>
        {
        private readonly IParserService _parser;
            public Handler(IParserService parser)
            {
            this._parser = parser;
            }

            public async Task<Result<ContentParagraph>> Handle(Query request, CancellationToken cancellationToken)
            {
                var paragraph = await _parser.GetParagraph(request.Dto.ContentUrl, request.Dto.Index);
                if (paragraph == null)
                    return Result<ContentParagraph>.Failure("Could not load paragraph");
                return Result<ContentParagraph>.Success(paragraph);
            }
        }
    }
}