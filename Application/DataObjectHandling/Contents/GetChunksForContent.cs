using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Domain.DataObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.Contents
{
    public class TranscriptChunkDto
    {
        public Guid TranscriptChunkId { get; set; }
        public string Language { get; set; }
        public string ChunkText { get; set; }
    }
    public class GetChunksForContent
    {
        public static TranscriptChunkDto GetChunkDto(TranscriptChunk chunk)
        {
            return new TranscriptChunkDto
            {
                TranscriptChunkId = chunk.TranscriptChunkId,
                Language = chunk.Language,
                ChunkText = chunk.ChunkText
            };
        }
        public class ContentIdDto
        {
            public Guid ContentId { get; set; }
        }
        public class Command : IRequest<Result<List<TranscriptChunkDto>>>
        {
            public ContentIdDto Dto { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<List<TranscriptChunkDto>>>
        {
        private readonly DataContext _context;
            public Handler(DataContext context)
            {
            this._context = context;
            }

            public async Task<Result<List<TranscriptChunkDto>>> Handle(Command request, CancellationToken cancellationToken)
            {
                var content = await _context.Contents
                .Include(u => u.Transcript)
                .ThenInclude(t => t.TranscriptChunks)
                .FirstOrDefaultAsync(u => u.ContentId == request.Dto.ContentId);

                if (content == null)
                    return Result<List<TranscriptChunkDto>>.Failure("Content not found");
                var chunks = content.Transcript.TranscriptChunks;
                var output = new List<TranscriptChunkDto>();
                foreach(var chunk in chunks)
                {
                    output.Add(GetChunkDto(chunk));
                }
                return Result<List<TranscriptChunkDto>>.Success(output);
            }
        }
    }
}