using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.Contents
{
    public class ContentIdDto
    {
        public Guid ContentId { get; set; }
    }
    public class GetChunkIdsForContent
    {
        public class Query : IRequest<Result<List<string>>>
        {
            public ContentIdDto Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<string>>>
        {
        private readonly DataContext _context;
            public Handler(DataContext context)
            {
            this._context = context;
            }

            public async Task<Result<List<string>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var content = await _context.Contents
                .Include(u => u.Transcript)
                .ThenInclude(u => u.TranscriptChunks)
                .FirstOrDefaultAsync(u => u.ContentId == request.Dto.ContentId);

                if(content == null)
                    return Result<List<string>>.Failure("Content not found");

                var output = new List<string>();
                var chunks = content.Transcript.TranscriptChunks;
                foreach(var c in chunks)
                {
                    output.Add(c.TranscriptChunkId.ToString());
                }

                return Result<List<string>>.Success(output);
            }
        }
    }
}