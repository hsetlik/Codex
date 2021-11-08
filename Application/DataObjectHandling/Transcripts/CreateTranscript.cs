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
    public class CreateTranscriptDto
    {
        public string Language { get; set; }
        public string FullText { get; set; }
    }
    public class CreateTranscript
    {
        public class Command : IRequest<Result<Unit>>
        {
            public CreateTranscriptDto CreateTranscriptDto { get; set; }

            public static List<string> ToChunks(string input, int chunkLength=50)
            {
                var list = new List<string>();
                var words = input.Split(' ');
                string currentChunk = "";
                int index = 0;
                foreach(var word in words)
                {
                    currentChunk += word + ' ';
                    ++index;
                    if(index > chunkLength)
                    {
                        list.Add(currentChunk);
                        currentChunk = "";
                        index = 0;
                    }
                }
                list.Add(currentChunk);
                return list;
            }
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
                // 1. Create an empty Transcript and upload it to Db
                var tGuid = Guid.NewGuid();
                var transcriptA = new Transcript
                {
                    TranscriptId = tGuid,
                    Language = request.CreateTranscriptDto.Language,
                    TranscriptChunks = new List<TranscriptChunk>()
                };
                
                _context.Transcripts.Add(transcriptA);
                var transcriptResult = await _context.SaveChangesAsync() > 0;
                if (! transcriptResult) return Result<Unit>.Failure("could not create transcript");

                var transcript = await _context.Transcripts.FindAsync(tGuid);
                if(transcript == null) return Result<Unit>.Failure("No matching Transcript!");
                // 2. Add chunks to Transcript and save again
                var chunks = Command.ToChunks(request.CreateTranscriptDto.FullText);
                foreach(var c in chunks)
                {
                    Console.WriteLine("CHUNK: " + c);
                    var chunk = new TranscriptChunk
                    {
                        ChunkText = c,
                        Transcript = transcript,
                        Language = transcript.Language
                    };
                    transcript.TranscriptChunks.Add(chunk);
                }
                var chunksResult = await _context.SaveChangesAsync() > 0;
                if (! chunksResult) return Result<Unit>.Failure("could not create transcript");
                return Result<Unit>.Success(Unit.Value); 
            }
        }
    }
}