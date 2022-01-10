using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.Content;
using Application.Interfaces;
using MediatR;
using Persistence;

namespace Application.DataObjectHandling.Contents
{
    public class GetContentHtml
    {
        public class Query : IRequest<Result<string>>
        {
            public ContentIdQuery Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<string>>
        {
        private readonly IParserService _parser;
        private readonly DataContext _context;
            public Handler(IParserService parser, DataContext context)
            {
            this._context = context;
            this._parser = parser;
            }

            public async Task<Result<string>> Handle(Query request, CancellationToken cancellationToken)
            {
                var content = await _context.Contents.FindAsync(request.Dto.ContentId); 
                if (content == null)
                    return Result<string>.Failure($"Could not get content with ID: {request.Dto.ContentId}");
                var html = await _parser.GetRawHtml(content.ContentUrl);
                if (html == "no valid html")
                    return Result<string>.Failure($"Could not get HTML at URL: {content.ContentUrl}");
                return Result<string>.Success(html);
            }
        }
    }
}