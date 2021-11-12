using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Extensions;
using Persistence;

namespace Application.DataObjectHandling.Contents
{
    public class EnsureContentTerms
    {
        public class Command : IRequest<Result<Unit>>
        {
            public ContentIdDto Dto { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
        private readonly DataContext _context;
            public Handler(DataContext context)
            {
            this._context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var content = await _context.Contents
                .Include(u => u.Transcript)
                .ThenInclude(t => t.TranscriptChunks)
                .FirstOrDefaultAsync( u => u.ContentId == request.Dto.ContentId);
                if (content == null) return Result<Unit>.Failure("Content not found");

                var tChunks = content.Transcript.TranscriptChunks;
                int wordIndex = 0;
                foreach(var chunk in tChunks)
                {
                    var words = chunk.ChunkText.Split(' ');
                    foreach(var word in words)
                    {
                        var result = await _context.CreateTerm(content.Language, word);
                        if (!result.IsSuccess) return Result<Unit>.Failure("Term for " + word + " could not be created at index " + wordIndex.ToString());
                        ++wordIndex;
                    }
                }
                return Result<Unit>.Success(Unit.Value);
            }
        }

    }
}