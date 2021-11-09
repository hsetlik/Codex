using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Domain.DataObjects;
using MediatR;
using Persistence;

namespace Application.DataObjectHandling.Transcripts
{
    public class ContentCreate
    {
        public class ContentCreateDto
        {
            public string ContentName { get; set; }
            public string ContentType { get; set; }
            public string Language { get; set; }
            public string VideoUrl { get; set; }
            public string AudioUrl { get; set; }
            public string FullText { get; set; }
        }

        public class Command : IRequest<Result<Unit>>
        {
            public ContentCreateDto Dto { get; set; }
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
                // create a content obj with empty transcript chunks
                var content = new Content
                {
                    ContentName = request.Dto.ContentName,
                    ContentType = request.Dto.ContentType,
                    Language = request.Dto.Language,
                    DateAdded = DateTime.Today.ToString(),
                    VideoUrl = request.Dto.VideoUrl,
                    AudioUrl = request.Dto.AudioUrl,
                    Transcript = new Transcript
                    {
                        Language = request.Dto.Language,
                        TranscriptChunks = new List<TranscriptChunk>()
                    }
                };
                //add chunks to the existing Transcript
                var words = request.Dto.FullText.Split(' ');
                const int chunkLength = 30;
                int index = 0;
                string currentChunk = "";
                var chunks = new List<string>();
                foreach(var word in words)
                {
                    currentChunk += word + ' ';
                    ++index;
                    if (index > chunkLength)
                    {
                        chunks.Add(currentChunk);
                        index = 0;
                        currentChunk = "";
                    }
                }
                foreach(var c in chunks)
                {
                    var chunk = new TranscriptChunk
                    {
                        ChunkText = c,
                        Transcript = content.Transcript,
                        Language = request.Dto.Language
                    };
                    content.Transcript.TranscriptChunks.Add(chunk);
                }
                _context.Contents.Add(content);
                var result = await _context.SaveChangesAsync() > 0;
                if (!result) return Result<Unit>.Failure("Could not create Content!");
                return Result<Unit>.Success(Unit.Value);
                
            }
        }

    }
}