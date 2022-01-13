using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.Content.Responses;
using Application.Interfaces;
using MediatR;
using Persistence;

namespace Application.DataObjectHandling.Contents
{
    public class GetContentHtml
    {
        public class Query : IRequest<Result<ContentPageHtml>>
        {
            public Guid ContentId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ContentPageHtml>>
        {
        private readonly DataContext _context;
        private readonly IParserService _parser;
            public Handler(DataContext context, IParserService parser)
            {
            this._parser = parser;
            this._context = context;
            }

            public async Task<Result<ContentPageHtml>> Handle(Query request, CancellationToken cancellationToken)
            {
                var content = await _context.Contents.FindAsync(request.ContentId);
                if (content == null)
                    return Result<ContentPageHtml>.Failure($"No content found with ID: {request.ContentId}");
                var page = await _parser.GetHtml(content.ContentUrl);
                if (page == null)
                    return Result<ContentPageHtml>.Failure($"No page found at URL: {content.ContentUrl}");
                return Result<ContentPageHtml>.Success(page);
            }
        }
    }
}