using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Domain.DataObjects;
using MediatR;
using Persistence;
using Application.DomainDTOs;
using Application.Extensions;

namespace Application.DataObjectHandling.Transcripts
{
    public class CreateTranscript
    {
        public class Command : IRequest<Result<Unit>>
        {
            public CreateTranscriptDto CreateTranscriptDto { get; set; }
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
                // 1. create the transcript w/ extension method
                var transcriptResult = await request.CreateTranscriptDto.CreateTranscriptFrom(_context);
                if (!transcriptResult.IsSuccess)
                    return Result<Unit>.Failure("Could not get transcript result");
                _context.Transcripts.Add(transcriptResult.Value);
                var success = await _context.SaveChangesAsync() > 0;
                if (!success)
                    return Result<Unit>.Failure("Transcript could not be created");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}