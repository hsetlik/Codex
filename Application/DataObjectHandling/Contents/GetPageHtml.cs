using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.Content.Responses;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.Contents
{
    public class GetPageHtml
    {
        public class Query : IRequest<Result<ContentPageHtml>>
        {
            public Guid ContentId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ContentPageHtml>>
        {
        private readonly IParserService _parser;
        private readonly DataContext _context;
            public Handler(IParserService parser, DataContext context)
            {
            this._context = context;
            this._parser = parser;
            }

            public async Task<Result<ContentPageHtml>> Handle(Query request, CancellationToken cancellationToken)
            {
                var content = await _context.Contents.FirstOrDefaultAsync(c => c.ContentId == request.ContentId);
                if (content == null)
                    return Result<ContentPageHtml>.Failure($"No content found with ID: {request.ContentId}");
                var html = await _parser.GetHtml(content.ContentUrl);
                if (html == null)
                    return Result<ContentPageHtml>.Failure($"No valid HTML at: {content.ContentUrl}");
                return Result<ContentPageHtml>.Success(html);
            }
        }
    }
}